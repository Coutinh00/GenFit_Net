using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using GenFitNet.Infrastructure.Data;
using GenFitNet.Application.Services;
using Serilog;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Serilog para Logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/genfitnet-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Configuração do Tracing com ActivitySource
var activitySource = new ActivitySource("GenFitNet.API");
builder.Services.AddSingleton(activitySource);

// Add services to the container.
builder.Services.AddControllers();

// Configuração do Entity Framework Core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=(localdb)\\mssqllocaldb;Database=GenFitNetDB;Trusted_Connection=True;MultipleActiveResultSets=true";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Registro dos serviços
builder.Services.AddScoped<IVagaService, VagaService>();
builder.Services.AddScoped<ICandidatoService, CandidatoService>();

// Configuração do Versionamento da API
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Configuração do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "GenFitNet API",
        Version = "v1",
        Description = "API RESTful para gestão de vagas e candidatos - Sistema de RH",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "GenFitNet",
            Email = "contato@genfitnet.com"
        }
    });
});

// Configuração de Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("database")
    .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("API está funcionando"));

// Configuração de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GenFitNet API v1");
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

// Middleware de Tracing
app.Use(async (context, next) =>
{
    using var activity = activitySource.StartActivity($"{context.Request.Method} {context.Request.Path}");
    activity?.SetTag("http.method", context.Request.Method);
    activity?.SetTag("http.path", context.Request.Path);
    
    await next();
    
    activity?.SetTag("http.status_code", context.Response.StatusCode);
});

app.MapControllers();

// Endpoint de Health Check
app.MapHealthChecks("/health");

// Endpoint detalhado de Health Check
app.MapHealthChecks("/health/detailed", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.TotalMilliseconds
            })
        });
        await context.Response.WriteAsync(result);
    }
});

// Aplicar migrations automaticamente em desenvolvimento
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        try
        {
            dbContext.Database.Migrate();
            Log.Information("Migrations aplicadas com sucesso");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro ao aplicar migrations");
        }
    }
}

Log.Information("GenFitNet API iniciada");

app.Run();

