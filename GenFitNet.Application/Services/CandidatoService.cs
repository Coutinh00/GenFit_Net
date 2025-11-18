using GenFitNet.Application.DTOs;
using GenFitNet.Infrastructure.Data;
using GenFitNet.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace GenFitNet.Application.Services;

public class CandidatoService : ICandidatoService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CandidatoService> _logger;

    public CandidatoService(ApplicationDbContext context, ILogger<CandidatoService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PagedResultDTO<CandidatoDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        _logger.LogInformation("Buscando candidatos - Página: {PageNumber}, Tamanho: {PageSize}", 
            pageNumber, pageSize);

        var query = _context.Candidatos.AsQueryable();

        var totalCount = await query.CountAsync();

        var candidatos = await query
            .Include(c => c.Vaga)
            .OrderByDescending(c => c.DataCadastro)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CandidatoDTO
            {
                Id = c.Id,
                Nome = c.Nome,
                Email = c.Email,
                Telefone = c.Telefone,
                Cidade = c.Cidade,
                Estado = c.Estado,
                Formacao = c.Formacao,
                AnosExperiencia = c.AnosExperiencia,
                AreaAtuacao = c.AreaAtuacao,
                ResumoProfissional = c.ResumoProfissional,
                LinkedIn = c.LinkedIn,
                DataCadastro = c.DataCadastro,
                DataAtualizacao = c.DataAtualizacao,
                VagaId = c.VagaId,
                VagaTitulo = c.Vaga != null ? c.Vaga.Titulo : null
            })
            .ToListAsync();

        return new PagedResultDTO<CandidatoDTO>
        {
            Data = candidatos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PagedResultDTO<CandidatoDTO>> SearchAsync(CandidatoFiltroDTO filtro, int pageNumber = 1, int pageSize = 10)
    {
        _logger.LogInformation("Pesquisando candidatos com filtros - Página: {PageNumber}, Tamanho: {PageSize}", 
            pageNumber, pageSize);

        var query = _context.Candidatos.AsQueryable();

        if (!string.IsNullOrEmpty(filtro.Nome))
        {
            query = query.Where(c => c.Nome.Contains(filtro.Nome));
        }

        if (!string.IsNullOrEmpty(filtro.Email))
        {
            query = query.Where(c => c.Email.Contains(filtro.Email));
        }

        if (!string.IsNullOrEmpty(filtro.Cidade))
        {
            query = query.Where(c => c.Cidade.Contains(filtro.Cidade));
        }

        if (!string.IsNullOrEmpty(filtro.Estado))
        {
            query = query.Where(c => c.Estado == filtro.Estado);
        }

        if (!string.IsNullOrEmpty(filtro.AreaAtuacao))
        {
            query = query.Where(c => c.AreaAtuacao.Contains(filtro.AreaAtuacao));
        }

        if (filtro.AnosExperienciaMinimo.HasValue)
        {
            query = query.Where(c => c.AnosExperiencia >= filtro.AnosExperienciaMinimo.Value);
        }

        if (filtro.VagaId.HasValue)
        {
            query = query.Where(c => c.VagaId == filtro.VagaId.Value);
        }

        if (!string.IsNullOrEmpty(filtro.Formacao))
        {
            query = query.Where(c => c.Formacao.Contains(filtro.Formacao));
        }

        var totalCount = await query.CountAsync();

        var candidatos = await query
            .Include(c => c.Vaga)
            .OrderByDescending(c => c.DataCadastro)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CandidatoDTO
            {
                Id = c.Id,
                Nome = c.Nome,
                Email = c.Email,
                Telefone = c.Telefone,
                Cidade = c.Cidade,
                Estado = c.Estado,
                Formacao = c.Formacao,
                AnosExperiencia = c.AnosExperiencia,
                AreaAtuacao = c.AreaAtuacao,
                ResumoProfissional = c.ResumoProfissional,
                LinkedIn = c.LinkedIn,
                DataCadastro = c.DataCadastro,
                DataAtualizacao = c.DataAtualizacao,
                VagaId = c.VagaId,
                VagaTitulo = c.Vaga != null ? c.Vaga.Titulo : null
            })
            .ToListAsync();

        return new PagedResultDTO<CandidatoDTO>
        {
            Data = candidatos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<CandidatoDTO?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Buscando candidato com ID: {Id}", id);

        var candidato = await _context.Candidatos
            .Include(c => c.Vaga)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (candidato == null)
        {
            _logger.LogWarning("Candidato com ID {Id} não encontrado", id);
            return null;
        }

        return new CandidatoDTO
        {
            Id = candidato.Id,
            Nome = candidato.Nome,
            Email = candidato.Email,
            Telefone = candidato.Telefone,
            Cidade = candidato.Cidade,
            Estado = candidato.Estado,
            Formacao = candidato.Formacao,
            AnosExperiencia = candidato.AnosExperiencia,
            AreaAtuacao = candidato.AreaAtuacao,
            ResumoProfissional = candidato.ResumoProfissional,
            LinkedIn = candidato.LinkedIn,
            DataCadastro = candidato.DataCadastro,
            DataAtualizacao = candidato.DataAtualizacao,
            VagaId = candidato.VagaId,
            VagaTitulo = candidato.Vaga?.Titulo
        };
    }

    public async Task<CandidatoDTO> CreateAsync(CreateCandidatoDTO dto)
    {
        _logger.LogInformation("Criando novo candidato: {Nome}", dto.Nome);

        var candidato = new Candidato
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Telefone = dto.Telefone,
            Cidade = dto.Cidade,
            Estado = dto.Estado,
            Formacao = dto.Formacao,
            AnosExperiencia = dto.AnosExperiencia,
            AreaAtuacao = dto.AreaAtuacao,
            ResumoProfissional = dto.ResumoProfissional,
            LinkedIn = dto.LinkedIn,
            VagaId = dto.VagaId,
            DataCadastro = DateTime.UtcNow
        };

        _context.Candidatos.Add(candidato);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Candidato criado com sucesso - ID: {Id}", candidato.Id);

        return await GetByIdAsync(candidato.Id) ?? throw new InvalidOperationException("Erro ao criar candidato");
    }

    public async Task<CandidatoDTO?> UpdateAsync(int id, UpdateCandidatoDTO dto)
    {
        _logger.LogInformation("Atualizando candidato com ID: {Id}", id);

        var candidato = await _context.Candidatos.FindAsync(id);
        if (candidato == null)
        {
            _logger.LogWarning("Candidato com ID {Id} não encontrado para atualização", id);
            return null;
        }

        if (!string.IsNullOrEmpty(dto.Nome))
            candidato.Nome = dto.Nome;
        if (!string.IsNullOrEmpty(dto.Email))
            candidato.Email = dto.Email;
        if (!string.IsNullOrEmpty(dto.Telefone))
            candidato.Telefone = dto.Telefone;
        if (!string.IsNullOrEmpty(dto.Cidade))
            candidato.Cidade = dto.Cidade;
        if (!string.IsNullOrEmpty(dto.Estado))
            candidato.Estado = dto.Estado;
        if (!string.IsNullOrEmpty(dto.Formacao))
            candidato.Formacao = dto.Formacao;
        if (dto.AnosExperiencia.HasValue)
            candidato.AnosExperiencia = dto.AnosExperiencia.Value;
        if (!string.IsNullOrEmpty(dto.AreaAtuacao))
            candidato.AreaAtuacao = dto.AreaAtuacao;
        if (!string.IsNullOrEmpty(dto.ResumoProfissional))
            candidato.ResumoProfissional = dto.ResumoProfissional;
        if (!string.IsNullOrEmpty(dto.LinkedIn))
            candidato.LinkedIn = dto.LinkedIn;
        if (dto.VagaId.HasValue)
            candidato.VagaId = dto.VagaId;

        candidato.DataAtualizacao = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Candidato atualizado com sucesso - ID: {Id}", id);

        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Deletando candidato com ID: {Id}", id);

        var candidato = await _context.Candidatos.FindAsync(id);
        if (candidato == null)
        {
            _logger.LogWarning("Candidato com ID {Id} não encontrado para exclusão", id);
            return false;
        }

        _context.Candidatos.Remove(candidato);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Candidato deletado com sucesso - ID: {Id}", id);
        return true;
    }
}

