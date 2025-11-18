using GenFitNet.Application.DTOs;

namespace GenFitNet.Application.Services;

public interface IVagaService
{
    Task<PagedResultDTO<VagaDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10, bool? ativa = null);
    Task<VagaDTO?> GetByIdAsync(int id);
    Task<VagaDTO> CreateAsync(CreateVagaDTO dto);
    Task<VagaDTO?> UpdateAsync(int id, UpdateVagaDTO dto);
    Task<bool> DeleteAsync(int id);
}

