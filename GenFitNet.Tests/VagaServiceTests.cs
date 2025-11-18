using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using GenFitNet.Application.DTOs;
using GenFitNet.Application.Services;
using GenFitNet.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Moq;

namespace GenFitNet.Tests;

public class VagaServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly IVagaService _service;

    public VagaServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        var logger = new Mock<ILogger<VagaService>>().Object;
        _service = new VagaService(_context, logger);
    }

    [Fact]
    public async Task CreateAsync_DeveCriarVagaComSucesso()
    {
        // Arrange
        var dto = new CreateVagaDTO
        {
            Titulo = "Nova Vaga",
            Descricao = "Descrição",
            Requisitos = "Requisitos",
            Localizacao = "São Paulo - SP",
            SalarioMinimo = 5000,
            SalarioMaximo = 8000,
            TipoContrato = "CLT"
        };

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Titulo.Should().Be(dto.Titulo);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarListaPaginada()
    {
        // Arrange
        await SeedDataAsync();

        // Act
        var result = await _service.GetAllAsync(pageNumber: 1, pageSize: 10);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().NotBeEmpty();
        result.TotalCount.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetByIdAsync_ComIdValido_DeveRetornarVaga()
    {
        // Arrange
        await SeedDataAsync();
        var vagaId = 1;

        // Act
        var result = await _service.GetByIdAsync(vagaId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(vagaId);
    }

    [Fact]
    public async Task UpdateAsync_ComIdValido_DeveAtualizarVaga()
    {
        // Arrange
        await SeedDataAsync();
        var vagaId = 1;
        var updateDto = new UpdateVagaDTO
        {
            Titulo = "Título Atualizado"
        };

        // Act
        var result = await _service.UpdateAsync(vagaId, updateDto);

        // Assert
        result.Should().NotBeNull();
        result!.Titulo.Should().Be("Título Atualizado");
    }

    [Fact]
    public async Task DeleteAsync_ComIdValido_DeveDeletarVaga()
    {
        // Arrange
        await SeedDataAsync();
        var vagaId = 1;

        // Act
        var result = await _service.DeleteAsync(vagaId);

        // Assert
        result.Should().BeTrue();
        var vaga = await _service.GetByIdAsync(vagaId);
        vaga.Should().BeNull();
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

