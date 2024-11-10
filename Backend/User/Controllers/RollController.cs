using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Repositories.Interfaces;

namespace PhAppUser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolController : ControllerBase
    {
        private readonly IRolRepository _rolRepository;
        private readonly ILogger<RolController> _logger;

        public RolController(IRolRepository rolRepository, ILogger<RolController> logger)
        {
            _rolRepository = rolRepository;
            _logger = logger;
        }

        [HttpGet("nombre/{nombre}")]
        public async Task<IActionResult> ObtenerPorNombre(string nombre)
        {
            var rol = await _rolRepository.BuscarPorNombreAsync(nombre);
            if (rol == null)
            {
                return NotFound("Rol no encontrado.");
            }
            return Ok(rol);
        }

        [HttpGet("{rolId}/permiso/{permisoId}")]
        public async Task<IActionResult> VerificarPermiso(Guid rolId, Guid permisoId)
        {
            bool tienePermiso = await _rolRepository.TienePermisoAsync(rolId, permisoId);
            return Ok(tienePermiso);
        }
    }
}

