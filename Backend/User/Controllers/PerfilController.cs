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
    public class PerfilController : ControllerBase
    {
        private readonly IPerfilRepository _perfilRepository;
        private readonly ILogger<PerfilController> _logger;

        public PerfilController(IPerfilRepository perfilRepository, ILogger<PerfilController> logger)
        {
            _perfilRepository = perfilRepository;
            _logger = logger;
        }

        // Obtener un perfil por su ID
        [HttpGet("{perfilId}")]
        public async Task<IActionResult> ObtenerPorId(Guid perfilId)
        {
            var perfil = await _perfilRepository.BuscarPorIdAsync(perfilId);
            if (perfil == null)
                return NotFound("Perfil no encontrado.");

            return Ok(perfil);
        }

        // Crear un nuevo perfil
        [HttpPost]
        public async Task<IActionResult> CrearPerfil([FromBody] Perfil perfil)
        {
            await _perfilRepository.AddAsync(perfil);
            return CreatedAtAction(nameof(ObtenerPorId), new { perfilId = perfil.Id }, perfil);
        }

        // Eliminar un perfil con relaciones
        [HttpDelete("{perfilId}/conRelaciones")]
        public async Task<IActionResult> EliminarPerfilConRelaciones(Guid perfilId)
        {
            try
            {
                await _perfilRepository.EliminarPerfilConRelacionesAsync(perfilId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el perfil con relaciones.");
                return StatusCode(500, "Error al procesar la solicitud.");
            }
        }

        // Actualizar roles en un perfil
        [HttpPut("{perfilId}/actualizarRoles")]
        public async Task<IActionResult> ActualizarRoles(Guid perfilId, [FromBody] ICollection<Rol> nuevosRoles)
        {
            try
            {
                await _perfilRepository.ActualizarRolesAsync(perfilId, nuevosRoles);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar los roles del perfil.");
                return StatusCode(500, "Error al procesar la solicitud.");
            }
        }

        // Obtener todos los perfiles con usuarios
        [HttpGet("conUsuarios")]
        public async Task<IActionResult> ObtenerPerfilesConUsuarios()
        {
            var perfiles = await _perfilRepository.ObtenerPerfilesConUsuariosAsync();
            return Ok(perfiles);
        }

        // Obtener perfiles por área
        [HttpGet("porArea/{areaId}")]
        public async Task<IActionResult> ObtenerPerfilesPorArea(Guid areaId)
        {
            var perfiles = await _perfilRepository.ObtenerPerfilesPorAreaAsync(areaId);
            return Ok(perfiles);
        }

        // Obtener todos los perfiles con roles
        [HttpGet("conRoles")]
        public async Task<IActionResult> ObtenerPerfilesConRoles()
        {
            var perfiles = await _perfilRepository.ObtenerPerfilesConRolesAsync();
            return Ok(perfiles);
        }

        // Verificar si un perfil tiene un rol específico
        [HttpGet("{perfilId}/tieneRol/{rolId}")]
        public async Task<IActionResult> TieneRol(Guid perfilId, Guid rolId)
        {
            var tieneRol = await _perfilRepository.TieneRolAsync(perfilId, rolId);
            return Ok(tieneRol);
        }

        // Obtener roles y permisos de un perfil
        [HttpGet("{perfilId}/rolesPermisos")]
        public async Task<IActionResult> ObtenerRolesYPermisos(Guid perfilId)
        {
            var roles = await _perfilRepository.ObtenerRolesYPermisosDePerfilAsync(perfilId);
            return Ok(roles);
        }
    }
}
