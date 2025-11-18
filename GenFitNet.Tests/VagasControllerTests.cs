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

public class VagasControllerTests
{
    private readonly ApplicationDbContext _context;
    private readonly IVagaService _vagaService;
    private readonly VagasController _controller;

    public VagasControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        var logger = new Mock<ILogger<VagaService>>().Object;
        _vagaService = new VagaService(_context, logger);
        
        var controllerLogger = new Mock<ILogger<VagasController>>().Object;
        _controller = new VagasController(_vagaService, controllerLogger);
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
        var pagedResult = okResult?.Value as PagedResultDTO<VagaDTO>;
        
        pagedResult.Should().NotBeNull();
        pagedResult!.Data.Should().NotBeEmpty();
        pagedResult.TotalCount.Should().BeGreaterThan(0);
        pagedResult.PageNumber.Should().Be(1);
        pagedResult.PageSize.Should().Be(10);
    }

    [Fact]
    public async Task GetById_ComIdValido_DeveRetornarVaga()
    {
        // Arrange
        await SeedDataAsync();
        var vagaId = 1;

        // Act
        var result = await _controller.GetById(vagaId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult?.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetById_ComIdInvalido_DeveRetornarNotFound()
    {
        // Arrange
        await SeedDataAsync();
        var vagaId = 999;

        // Act
        var result = await _controller.GetById(vagaId);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Create_ComDadosValidos_DeveCriarVaga()
    {
        // Arrange
        var createDto = new CreateVagaDTO
        {
            Titulo = "Nova Vaga",
            Descricao = "Descrição da nova vaga",
            Requisitos = "Requisitos da vaga",
            Localizacao = "São Paulo - SP",
            SalarioMinimo = 5000,
            SalarioMaximo = 8000,
            TipoContrato = "CLT"
        };

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult?.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task Update_ComIdValido_DeveAtualizarVaga()
    {
        // Arrange
        await SeedDataAsync();
        var vagaId = 1;
        var updateDto = new UpdateVagaDTO
        {
            Titulo = "Título Atualizado"
        };

        // Act
        var result = await _controller.Update(vagaId, updateDto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Delete_ComIdValido_DeveDeletarVaga()
    {
        // Arrange
        await SeedDataAsync();
        var vagaId = 1;

        // Act
        var result = await _controller.Delete(vagaId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    private async Task SeedDataAsync()
    {
        if (!_context.Vagas.Any())
        {
            _context.Vagas.Add(new Infrastructure.Models.Vaga
            {
                Id = 1,
                Titulo = "Desenvolvedor .NET",
                Descricao = "Descrição",
                Requisitos = "Requisitos",
                Localizacao = "São Paulo - SP",
                SalarioMinimo = 5000,
                SalarioMaximo = 8000,
                TipoContrato = "CLT",
                DataCriacao = DateTime.UtcNow,
                Ativa = true
            });

            await _context.SaveChangesAsync();
        }
    }
}

