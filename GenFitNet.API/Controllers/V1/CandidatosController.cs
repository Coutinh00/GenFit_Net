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
public class CandidatosController : ControllerBase
{
    private readonly ICandidatoService _candidatoService;
    private readonly ILogger<CandidatosController> _logger;
    private static readonly ActivitySource ActivitySource = new("GenFitNet.API.Candidatos");

    public CandidatosController(ICandidatoService candidatoService, ILogger<CandidatosController> logger)
    {
        _candidatoService = candidatoService;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os candidatos com paginação
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDTO<CandidatoDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResultDTO<CandidatoDTO>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        using var activity = ActivitySource.StartActivity("GetAllCandidatos");
        activity?.SetTag("pageNumber", pageNumber);
        activity?.SetTag("pageSize", pageSize);

        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
        {
            _logger.LogWarning("Parâmetros de paginação inválidos: PageNumber={PageNumber}, PageSize={PageSize}", 
                pageNumber, pageSize);
            return BadRequest(new { message = "Parâmetros de paginação inválidos. pageNumber deve ser >= 1 e pageSize entre 1 e 100." });
        }

        var result = await _candidatoService.GetAllAsync(pageNumber, pageSize);
        
        // Adicionar links HATEOAS
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        result.Links = HateoasHelper.GeneratePagedLinks(pageNumber, result.TotalPages, baseUrl, "v1", "candidatos");

        return Ok(result);
    }

    /// <summary>
    /// Pesquisa candidatos com filtros
    /// </summary>
    [HttpPost("search")]
    [ProducesResponseType(typeof(PagedResultDTO<CandidatoDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResultDTO<CandidatoDTO>>> Search(
        [FromBody] CandidatoFiltroDTO filtro,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        using var activity = ActivitySource.StartActivity("SearchCandidatos");
        activity?.SetTag("pageNumber", pageNumber);
        activity?.SetTag("pageSize", pageSize);

        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
        {
            _logger.LogWarning("Parâmetros de paginação inválidos: PageNumber={PageNumber}, PageSize={PageSize}", 
                pageNumber, pageSize);
            return BadRequest(new { message = "Parâmetros de paginação inválidos. pageNumber deve ser >= 1 e pageSize entre 1 e 100." });
        }

        var result = await _candidatoService.SearchAsync(filtro, pageNumber, pageSize);
        
        // Adicionar links HATEOAS
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        result.Links = HateoasHelper.GeneratePagedLinks(pageNumber, result.TotalPages, baseUrl, "v1", "candidatos/search");

        return Ok(result);
    }

    /// <summary>
    /// Obtém um candidato específico por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CandidatoDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CandidatoDTO>> GetById(int id)
    {
        using var activity = ActivitySource.StartActivity("GetCandidatoById");
        activity?.SetTag("candidatoId", id);

        var candidato = await _candidatoService.GetByIdAsync(id);
        
        if (candidato == null)
        {
            _logger.LogWarning("Candidato com ID {Id} não encontrado", id);
            return NotFound(new { message = $"Candidato com ID {id} não encontrado." });
        }

        // Adicionar links HATEOAS
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var links = HateoasHelper.GenerateCandidatoLinks(id, baseUrl, "v1");
        
        return Ok(new { data = candidato, links });
    }

    /// <summary>
    /// Cria um novo candidato
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CandidatoDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CandidatoDTO>> Create([FromBody] CreateCandidatoDTO dto)
    {
        using var activity = ActivitySource.StartActivity("CreateCandidato");

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Dados inválidos para criação de candidato");
            return BadRequest(ModelState);
        }

        var candidato = await _candidatoService.CreateAsync(dto);
        
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var links = HateoasHelper.GenerateCandidatoLinks(candidato.Id, baseUrl, "v1");

        return CreatedAtAction(
            nameof(GetById),
            new { id = candidato.Id, version = "1.0" },
            new { data = candidato, links });
    }

    /// <summary>
    /// Atualiza um candidato existente
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CandidatoDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CandidatoDTO>> Update(int id, [FromBody] UpdateCandidatoDTO dto)
    {
        using var activity = ActivitySource.StartActivity("UpdateCandidato");
        activity?.SetTag("candidatoId", id);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var candidato = await _candidatoService.UpdateAsync(id, dto);
        
        if (candidato == null)
        {
            _logger.LogWarning("Candidato com ID {Id} não encontrado para atualização", id);
            return NotFound(new { message = $"Candidato com ID {id} não encontrado." });
        }

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var links = HateoasHelper.GenerateCandidatoLinks(id, baseUrl, "v1");

        return Ok(new { data = candidato, links });
    }

    /// <summary>
    /// Deleta um candidato
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        using var activity = ActivitySource.StartActivity("DeleteCandidato");
        activity?.SetTag("candidatoId", id);

        var deleted = await _candidatoService.DeleteAsync(id);
        
        if (!deleted)
        {
            _logger.LogWarning("Candidato com ID {Id} não encontrado para exclusão", id);
            return NotFound(new { message = $"Candidato com ID {id} não encontrado." });
        }

        return NoContent();
    }
}

