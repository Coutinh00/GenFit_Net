using Microsoft.AspNetCore.Mvc;
using GenFitNet.Application.DTOs;
using GenFitNet.Application.Services;
using GenFitNet.API.Helpers;
using System.Diagnostics;

namespace GenFitNet.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class VagasController : ControllerBase
{
    private readonly IVagaService _vagaService;
    private readonly ILogger<VagasController> _logger;
    private static readonly ActivitySource ActivitySource = new("GenFitNet.API.Vagas");

    public VagasController(IVagaService vagaService, ILogger<VagasController> logger)
    {
        _vagaService = vagaService;
        _logger = logger;
    }

    /// <summary>
    /// Lista todas as vagas com paginação
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDTO<VagaDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResultDTO<VagaDTO>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool? ativa = null)
    {
        using var activity = ActivitySource.StartActivity("GetAllVagas");
        activity?.SetTag("pageNumber", pageNumber);
        activity?.SetTag("pageSize", pageSize);
        activity?.SetTag("ativa", ativa);

        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
        {
            _logger.LogWarning("Parâmetros de paginação inválidos: PageNumber={PageNumber}, PageSize={PageSize}", 
                pageNumber, pageSize);
            return BadRequest(new { message = "Parâmetros de paginação inválidos. pageNumber deve ser >= 1 e pageSize entre 1 e 100." });
        }

        var result = await _vagaService.GetAllAsync(pageNumber, pageSize, ativa);
        
        // Adicionar links HATEOAS
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        result.Links = HateoasHelper.GeneratePagedLinks(pageNumber, result.TotalPages, baseUrl, "v1", "vagas");

        return Ok(result);
    }

    /// <summary>
    /// Obtém uma vaga específica por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(VagaDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VagaDTO>> GetById(int id)
    {
        using var activity = ActivitySource.StartActivity("GetVagaById");
        activity?.SetTag("vagaId", id);

        var vaga = await _vagaService.GetByIdAsync(id);
        
        if (vaga == null)
        {
            _logger.LogWarning("Vaga com ID {Id} não encontrada", id);
            return NotFound(new { message = $"Vaga com ID {id} não encontrada." });
        }

        // Adicionar links HATEOAS
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var links = HateoasHelper.GenerateVagaLinks(id, baseUrl, "v1");
        
        return Ok(new { data = vaga, links });
    }

    /// <summary>
    /// Cria uma nova vaga
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(VagaDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VagaDTO>> Create([FromBody] CreateVagaDTO dto)
    {
        using var activity = ActivitySource.StartActivity("CreateVaga");

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Dados inválidos para criação de vaga");
            return BadRequest(ModelState);
        }

        var vaga = await _vagaService.CreateAsync(dto);
        
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var links = HateoasHelper.GenerateVagaLinks(vaga.Id, baseUrl, "v1");

        return CreatedAtAction(
            nameof(GetById),
            new { id = vaga.Id, version = "1.0" },
            new { data = vaga, links });
    }

    /// <summary>
    /// Atualiza uma vaga existente
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(VagaDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VagaDTO>> Update(int id, [FromBody] UpdateVagaDTO dto)
    {
        using var activity = ActivitySource.StartActivity("UpdateVaga");
        activity?.SetTag("vagaId", id);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var vaga = await _vagaService.UpdateAsync(id, dto);
        
        if (vaga == null)
        {
            _logger.LogWarning("Vaga com ID {Id} não encontrada para atualização", id);
            return NotFound(new { message = $"Vaga com ID {id} não encontrada." });
        }

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var links = HateoasHelper.GenerateVagaLinks(id, baseUrl, "v1");

        return Ok(new { data = vaga, links });
    }

    /// <summary>
    /// Deleta uma vaga
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        using var activity = ActivitySource.StartActivity("DeleteVaga");
        activity?.SetTag("vagaId", id);

        var deleted = await _vagaService.DeleteAsync(id);
        
        if (!deleted)
        {
            _logger.LogWarning("Vaga com ID {Id} não encontrada para exclusão", id);
            return NotFound(new { message = $"Vaga com ID {id} não encontrada." });
        }

        return NoContent();
    }
}

