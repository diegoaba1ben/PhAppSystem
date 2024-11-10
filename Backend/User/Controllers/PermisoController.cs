using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Repositories.Interfaces;

namespace PhAppUser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermisoController : ControllerBase
    {
        private readonly IPermisoRepository _permisoRepository;
        private readonly ILogger<PermisoController> _logger;

        public PermisoController(IPermisoRepository permisoRepository, ILogger<PermisoController> logger)
        {
            _permisoRepository = permisoRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CrearPermiso([FromBody] Permiso permiso)
        {
            try
            {
                if (await _permisoRepository.ExisteCodigoAsync(permiso.Codigo))
                {
                    return BadRequest("El código del permiso ya está en uso.");
                }

                await _permisoRepository.AddAsync(permiso);
                return Ok(permiso);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un permiso.");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }

        [HttpGet("{codigo}")]
        public async Task<IActionResult> ObtenerPorCodigo(string codigo)
        {
            var permiso = await _permisoRepository.BuscarPorCodigoAsync(codigo);
            if (permiso == null)
            {
                return NotFound("Permiso no encontrado.");
            }
            return Ok(permiso);
        }

        [HttpGet("porRol/{rolId}")]
        public async Task<IActionResult> ObtenerPermisosPorRol(Guid rolId)
        {
            var permisos = await _permisoRepository.ObtenerPermisosPorRolAsync(rolId);
            return Ok(permisos);
        }
    }
}

