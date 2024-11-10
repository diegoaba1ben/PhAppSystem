using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Repositories.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class RepLegalController : ControllerBase
{
    private readonly IRepLegalRepository _repLegalRepository;
    private readonly ILogger<RepLegalController> _logger = null!;

    public RepLegalController(IRepLegalRepository repLegalRepository)
    {
        _repLegalRepository = repLegalRepository;
    }

    /// <summary>
    /// Crea un nuevo registro de representante legal.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CrearRepLegal([FromBody] RepLegal repLegal)
    {
        try
        {
            if (await _repLegalRepository.ExisteCertLegalAsync(repLegal.CertLegal))
            {
                return BadRequest("Ya existe un representante legal con el mismo número de radicación.");
            }

            if (await _repLegalRepository.ExisteSuperposicionDeFechasAsync(repLegal.FechaInicio, repLegal.FechaFinal))
            {
                return BadRequest("Las fechas ingresadas se superponen con otro periodo de representación activa.");
            }

            // Guardar el nuevo RepLegal en la base de datos
            await _repLegalRepository.AddAsync(repLegal);

            return Ok(repLegal);
        }
        catch (Exception ex)
        {
            // Registro de log de error o cualquier manejo de error adicional
            _logger.LogError(ex,"Error al crear un representante legal.");
            return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
        }
    }

    /// <summary>
    /// Obtiene una lista de representantes legales activos.
    /// </summary>
    [HttpGet("activos")]
    public async Task<IActionResult> ObtenerRepresentantesActivos()
    {
        try
        {
            var representantes = await _repLegalRepository.ObtenerRepresentantesActivosAsync();
            return Ok(representantes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener representantes legales activos");
            return StatusCode(500, "Ocurrió un error al recuperar los representantes activos.");
        }
    }

    /// <summary>
    /// Obtiene el historial de representantes legales.
    /// </summary>
    [HttpGet("historial")]
    public async Task<IActionResult> ObtenerHistorialRepresentantes()
    {
        try
        {
            var historial = await _repLegalRepository.ObtenerHistorialRepresentantesAsync();
            return Ok(historial);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error alobtenr el historial de representantes legales");
            return StatusCode(500, "Ocurrió un error al recuperar el historial de representantes legales.");
        }
    }
}

