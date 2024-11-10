using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Repositories.Interfaces;

namespace PhAppUser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PensionController : ControllerBase
    {
        private readonly IPensionRepository _pensionRepository;
        private readonly ILogger<PensionController> _logger;

        public PensionController(IPensionRepository pensionRepository, ILogger<PensionController> logger)
        {
            _pensionRepository = pensionRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CrearPension([FromBody] Pension pension)
        {
            try
            {
                if (await _pensionRepository.ExisteNumeroAsync(pension.Numero))
                {
                    return BadRequest("El número de afiliación ya está en uso.");
                }

                await _pensionRepository.AddAsync(pension);
                return Ok(pension);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un registro de Pensión.");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }
    }
}
