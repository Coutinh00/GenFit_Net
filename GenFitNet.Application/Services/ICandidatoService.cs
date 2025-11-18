using GenFitNet.Application.DTOs;

namespace GenFitNet.Application.Services;

public interface ICandidatoService
{
    Task<PagedResultDTO<CandidatoDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
    Task<PagedResultDTO<CandidatoDTO>> SearchAsync(CandidatoFiltroDTO filtro, int pageNumber = 1, int pageSize = 10);
    Task<CandidatoDTO?> GetByIdAsync(int id);
    Task<CandidatoDTO> CreateAsync(CreateCandidatoDTO dto);
    Task<CandidatoDTO?> UpdateAsync(int id, UpdateCandidatoDTO dto);
    Task<bool> DeleteAsync(int id);
}

