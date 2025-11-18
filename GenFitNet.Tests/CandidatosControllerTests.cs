using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GenFitNet.API.Controllers.V1;
using GenFitNet.Application.DTOs;
using GenFitNet.Application.Services;
using GenFitNet.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Moq;

namespace GenFitNet.Tests;

public class CandidatosControllerTests
{
    private readonly ApplicationDbContext _context;
    private readonly ICandidatoService _candidatoService;
    private readonly CandidatosController _controller;

    public CandidatosControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        var logger = new Mock<ILogger<CandidatoService>>().Object;
        _candidatoService = new CandidatoService(_context, logger);
        
        var controllerLogger = new Mock<ILogger<CandidatosController>>().Object;
        _controller = new CandidatosController(_candidatoService, controllerLogger);
    }

    [Fact]
    public async Task GetAll_DeveRetornarListaPaginada()
    {
        // Arrange
        await SeedDataAsync();

        // Act
        var result = await _controller.GetAll(pageNumber: 1, pageSize: 10);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var pagedResult = okResult?.Value as PagedResultDTO<CandidatoDTO>;
        
        pagedResult.Should().NotBeNull();
        pagedResult!.PageNumber.Should().Be(1);
        pagedResult.PageSize.Should().Be(10);
    }

    [Fact]
    public async Task Search_ComFiltros_DeveRetornarCandidatosFiltrados()
    {
        // Arrange
        await SeedDataAsync();
        var filtro = new CandidatoFiltroDTO
        {
            Nome = "João",
            AreaAtuacao = ".NET"
        };

        // Act
        var result = await _controller.Search(filtro, pageNumber: 1, pageSize: 10);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var pagedResult = okResult?.Value as PagedResultDTO<CandidatoDTO>;
        
        pagedResult.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_ComIdValido_DeveRetornarCandidato()
    {
        // Arrange
        await SeedDataAsync();
        var candidatoId = 1;

        // Act
        var result = await _controller.GetById(candidatoId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Create_ComDadosValidos_DeveCriarCandidato()
    {
        // Arrange
        var createDto = new CreateCandidatoDTO
        {
            Nome = "Novo Candidato",
            Email = "novo@email.com",
            Telefone = "(11) 98765-4321",
            Cidade = "São Paulo",
            Estado = "SP",
            Formacao = "Ciência da Computação",
            AnosExperiencia = 5,
            AreaAtuacao = "Desenvolvimento",
            ResumoProfissional = "Resumo profissional",
            LinkedIn = "linkedin.com/in/novo"
        };

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult?.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task Update_ComIdValido_DeveAtualizarCandidato()
    {
        // Arrange
        await SeedDataAsync();
        var candidatoId = 1;
        var updateDto = new UpdateCandidatoDTO
        {
            Nome = "Nome Atualizado"
        };

        // Act
        var result = await _controller.Update(candidatoId, updateDto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Delete_ComIdValido_DeveDeletarCandidato()
    {
        // Arrange
        await SeedDataAsync();
        var candidatoId = 1;

        // Act
        var result = await _controller.Delete(candidatoId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    private async Task SeedDataAsync()
    {
        if (!_context.Candidatos.Any())
        {
            _context.Candidatos.Add(new Infrastructure.Models.Candidato
            {
                Id = 1,
                Nome = "João Silva",
                Email = "joao@email.com",
                Telefone = "(11) 98765-4321",
                Cidade = "São Paulo",
                Estado = "SP",
                Formacao = "Ciência da Computação",
                AnosExperiencia = 5,
                AreaAtuacao = "Desenvolvimento .NET",
                ResumoProfissional = "Resumo",
                LinkedIn = "linkedin.com/in/joao",
                DataCadastro = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }
    }
}

