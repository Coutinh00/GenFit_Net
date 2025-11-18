using GenFitNet.Application.DTOs;
using GenFitNet.Infrastructure.Data;
using GenFitNet.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace GenFitNet.Application.Services;

public class VagaService : IVagaService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<VagaService> _logger;

    public VagaService(ApplicationDbContext context, ILogger<VagaService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PagedResultDTO<VagaDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10, bool? ativa = null)
    {
        _logger.LogInformation("Buscando vagas - Página: {PageNumber}, Tamanho: {PageSize}, Ativa: {Ativa}", 
            pageNumber, pageSize, ativa);

        var query = _context.Vagas.AsQueryable();

        if (ativa.HasValue)
        {
            query = query.Where(v => v.Ativa == ativa.Value);
        }

        var totalCount = await query.CountAsync();

        var vagas = await query
            .OrderByDescending(v => v.DataCriacao)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(v => new VagaDTO
            {
                Id = v.Id,
                Titulo = v.Titulo,
                Descricao = v.Descricao,
                Requisitos = v.Requisitos,
                Localizacao = v.Localizacao,
                SalarioMinimo = v.SalarioMinimo,
                SalarioMaximo = v.SalarioMaximo,
                TipoContrato = v.TipoContrato,
                DataCriacao = v.DataCriacao,
                DataAtualizacao = v.DataAtualizacao,
                Ativa = v.Ativa,
                TotalCandidatos = v.Candidatos.Count
            })
            .ToListAsync();

        return new PagedResultDTO<VagaDTO>
        {
            Data = vagas,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<VagaDTO?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Buscando vaga com ID: {Id}", id);

        var vaga = await _context.Vagas
            .Include(v => v.Candidatos)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (vaga == null)
        {
            _logger.LogWarning("Vaga com ID {Id} não encontrada", id);
            return null;
        }

        return new VagaDTO
        {
            Id = vaga.Id,
            Titulo = vaga.Titulo,
            Descricao = vaga.Descricao,
            Requisitos = vaga.Requisitos,
            Localizacao = vaga.Localizacao,
            SalarioMinimo = vaga.SalarioMinimo,
            SalarioMaximo = vaga.SalarioMaximo,
            TipoContrato = vaga.TipoContrato,
            DataCriacao = vaga.DataCriacao,
            DataAtualizacao = vaga.DataAtualizacao,
            Ativa = vaga.Ativa,
            TotalCandidatos = vaga.Candidatos.Count
        };
    }

    public async Task<VagaDTO> CreateAsync(CreateVagaDTO dto)
    {
        _logger.LogInformation("Criando nova vaga: {Titulo}", dto.Titulo);

        var vaga = new Vaga
        {
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            Requisitos = dto.Requisitos,
            Localizacao = dto.Localizacao,
            SalarioMinimo = dto.SalarioMinimo,
            SalarioMaximo = dto.SalarioMaximo,
            TipoContrato = dto.TipoContrato,
            DataCriacao = DateTime.UtcNow,
            Ativa = true
        };

        _context.Vagas.Add(vaga);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Vaga criada com sucesso - ID: {Id}", vaga.Id);

        return new VagaDTO
        {
            Id = vaga.Id,
            Titulo = vaga.Titulo,
            Descricao = vaga.Descricao,
            Requisitos = vaga.Requisitos,
            Localizacao = vaga.Localizacao,
            SalarioMinimo = vaga.SalarioMinimo,
            SalarioMaximo = vaga.SalarioMaximo,
            TipoContrato = vaga.TipoContrato,
            DataCriacao = vaga.DataCriacao,
            DataAtualizacao = vaga.DataAtualizacao,
            Ativa = vaga.Ativa,
            TotalCandidatos = 0
        };
    }

    public async Task<VagaDTO?> UpdateAsync(int id, UpdateVagaDTO dto)
    {
        _logger.LogInformation("Atualizando vaga com ID: {Id}", id);

        var vaga = await _context.Vagas.FindAsync(id);
        if (vaga == null)
        {
            _logger.LogWarning("Vaga com ID {Id} não encontrada para atualização", id);
            return null;
        }

        if (!string.IsNullOrEmpty(dto.Titulo))
            vaga.Titulo = dto.Titulo;
        if (!string.IsNullOrEmpty(dto.Descricao))
            vaga.Descricao = dto.Descricao;
        if (!string.IsNullOrEmpty(dto.Requisitos))
            vaga.Requisitos = dto.Requisitos;
        if (!string.IsNullOrEmpty(dto.Localizacao))
            vaga.Localizacao = dto.Localizacao;
        if (dto.SalarioMinimo.HasValue)
            vaga.SalarioMinimo = dto.SalarioMinimo.Value;
        if (dto.SalarioMaximo.HasValue)
            vaga.SalarioMaximo = dto.SalarioMaximo.Value;
        if (!string.IsNullOrEmpty(dto.TipoContrato))
            vaga.TipoContrato = dto.TipoContrato;
        if (dto.Ativa.HasValue)
            vaga.Ativa = dto.Ativa.Value;

        vaga.DataAtualizacao = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Vaga atualizada com sucesso - ID: {Id}", id);

        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Deletando vaga com ID: {Id}", id);

        var vaga = await _context.Vagas.FindAsync(id);
        if (vaga == null)
        {
            _logger.LogWarning("Vaga com ID {Id} não encontrada para exclusão", id);
            return false;
        }

        _context.Vagas.Remove(vaga);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Vaga deletada com sucesso - ID: {Id}", id);
        return true;
    }
}

