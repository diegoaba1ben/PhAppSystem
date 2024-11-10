using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Repositories.Interfaces;


namespace PhAppUser.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class SaludController : ControllerBase
    {
        private readonly ISaludRepository _saludRepository;
        private readonly ILogger<SaludController> _logger;

        public SaludController(ISaludRepository saludRepository, ILogger<SaludController> logger)
        {
            _saludRepository = saludRepository;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> CrearSalud([FromBody] Salud salud)
        {
            try{
                if(await _saludRepository.ExisteNumeroAsync(salud.Numero))
                {
                    return BadRequest("El número de afiliación ya existe en la base de datos.");
                }
                await _saludRepository.AddAsync(salud);
                return Ok(salud);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error al crear un registro en la entidad salud.");
                return StatusCode(500, "Ocurrión un error al procesar la solicitud.");
            }
        }
    }
}