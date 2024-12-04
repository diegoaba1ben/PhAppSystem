using Microsoft.AspNetCore.Mvc;
using PhAppUser.Domain.Entities;
using PhAppUser.Domain.Enums;
using PhAppUser.Infrastructure.Repositories.Interfaces;
using PhAppUser.Application.DTOs;

namespace PhAppUser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentaUsuarioController : ControllerBase
    {
        private readonly ICuentaUsuarioRepository _cuentaUsuarioRepository;
        private readonly ISaludRepository _saludRepository;
        private readonly IPensionRepository _pensionRepository;
        private readonly IPerfilRepository _perfilRepository;

        public CuentaUsuarioController(
            ICuentaUsuarioRepository cuentaUsuarioRepository,
            ISaludRepository saludRepository,
            IPensionRepository pensionRepository,
            IPerfilRepository perfilRepository)
        {
            _cuentaUsuarioRepository = cuentaUsuarioRepository ?? throw new ArgumentNullException(nameof(cuentaUsuarioRepository));
            _saludRepository = saludRepository ?? throw new ArgumentNullException(nameof(saludRepository));
            _pensionRepository = pensionRepository ?? throw new ArgumentNullException(nameof(pensionRepository));
            _perfilRepository = perfilRepository ?? throw new ArgumentNullException(nameof(perfilRepository));

        }

        // Método de búsqueda avanzada
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CuentaUsuario>>> SearchUsuarioAsync(string searchTerm)
        {
            var usuarios = await _cuentaUsuarioRepository.SearchUsuarioAsync(searchTerm);
            return Ok(usuarios);
        }

        // Obtener usuarios activos
        [HttpGet("activos")]
        public async Task<ActionResult<IEnumerable<CuentaUsuario>>> GetUsuariosActivosAsync()
        {
            var usuariosActivos = await _cuentaUsuarioRepository.GetUsuariosActivosAsync();
            return Ok(usuariosActivos);
        }
        // Usuarios inactivos
        [HttpGet("usuarios-inactivos")]
        public async Task<ActionResult<IEnumerable<UsuarioInactivoDto>>> GetUsuariosInactivosAsync()
        {
            try
            {
                var usuariosInactivos = await _cuentaUsuarioRepository.GetUsuariosInactivosAsync();
                if (!usuariosInactivos.Any())
                {
                    return NotFound(new { message = "No se encontraron usuarios inactivos." });
                }

                return Ok(usuariosInactivos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado al obtener usuarios inactivos.", detalle = ex.Message });
            }
        }


        // Obtener usuarios por perfil
        [HttpGet("perfil/{perfilId}")]
        public async Task<ActionResult<IEnumerable<CuentaUsuario>>> GetUsuarioByPerfilAsync(Guid perfilId)
        {
            var usuarios = await _cuentaUsuarioRepository.GetUsuarioByPerfilAsync(perfilId);
            return Ok(usuarios);
        }

        // Obtener usuarios con roles
        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<CuentaUsuario>>> GetUsuarioByRolesAsync()
        {
            var usuariosConRoles = await _cuentaUsuarioRepository.GetUsuarioByRolesAsync();
            return Ok(usuariosConRoles);
        }

        // Obtener usuarios por tipo de contrato
        [HttpGet("contrato/{tipoContrato}")]
        public async Task<ActionResult<IEnumerable<CuentaUsuario>>> GetUsuariosByTipoContratoAsync(TipoContrato tipoContrato)
        {
            var usuarios = await _cuentaUsuarioRepository.GetUsuariosByTipoContratoAsync(tipoContrato);
            return Ok(usuarios);
        }

        // Verificar si existe un nombre de usuario
        [HttpGet("existe/{nombreUsuario}")]
        public async Task<ActionResult<bool>> ExisteNombreUsuarioAsync(string nombreUsuario)
        {
            var existe = await _cuentaUsuarioRepository.ExisteNombreUsuarioAsync(nombreUsuario);
            return Ok(existe);
        }

        // Obtener usuarios por última fecha de inicio de sesión
        [HttpGet("ultimologin/{date}")]
        public async Task<ActionResult<IEnumerable<CuentaUsuario>>> GetUsuariosByUltimoLoginAsync(DateTime date)
        {
            var usuarios = await _cuentaUsuarioRepository.GetUsuariosByUltimoLoginAsync(date);
            return Ok(usuarios);
        }

        // Obtener usuarios con afiliaciones pendientes
        [HttpGet("afiliacionespendientes")]
        public async Task<ActionResult<IEnumerable<CuentaUsuario>>> GetUsuariosAfiliacionPendienteAsync()
        {
            var usuarios = await _cuentaUsuarioRepository.GetUsuariosAfiliacionPendienteAsync();
            return Ok(usuarios);
        }

        // Inactivar usuario
        [HttpPut("{id}/inactivar")]
        public async Task<IActionResult> InactivarUsuario(Guid id, [FromBody] DateTime fechaInactivacion)
        {
            var usuario = await _cuentaUsuarioRepository.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound(new { message = $"Usuario con ID {id} no encontrado." });
            }

            usuario.InactivarUsuario(fechaInactivacion);
            await _cuentaUsuarioRepository.UpdateAsync(usuario);

            return Ok(new { message = "Usuario inactivado correctamente." });
        }

        [HttpPut("{id}/reactivar")]
        public async Task<IActionResult> ReactivarUsuario(Guid id, [FromBody] ReactivacionRequest request)
        {
            // Obtener el usuario por ID
            var usuario = await _cuentaUsuarioRepository.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound(new { message = $"Usuario con ID {id} no encontrado." });
            }

            // Reactivar el usuario
            usuario.ReactivarUsuario();

            // Actualizar afiliaciones de salud y pensión
            await ActualizaAfiliacionSalud(usuario.Id, request.NumeroSalud);
            await ActualizaAfiliacionPension(usuario.Id, request.NumeroPension);

            // Guardar cambios en el usuario
            await _cuentaUsuarioRepository.UpdateAsync(usuario);

            return Ok(new { message = "Usuario y sus afiliaciones reactivados correctamente." });
        }

        private async Task ActualizaAfiliacionSalud(Guid usuarioId, string? numeroSalud)
        {
            if (!string.IsNullOrEmpty(numeroSalud))
            {
                // Obtener afiliación de salud por ID de usuario
                var afiliacionSalud = await _saludRepository.GetByIdAsync(usuarioId);
                if (afiliacionSalud != null)
                {
                    // Actualizar estado y número de afiliación
                    afiliacionSalud.ActualizarEstado(true);
                    afiliacionSalud.Numero = numeroSalud;

                    // Guardar cambios
                    await _saludRepository.UpdateAsync(afiliacionSalud);
                }
            }
        }

        private async Task ActualizaAfiliacionPension(Guid usuarioId, string? numeroPension)
        {
            if (!string.IsNullOrEmpty(numeroPension))
            {
                // Obtener afiliación de pensión por ID de usuario
                var afiliacionPension = await _pensionRepository.GetByIdAsync(usuarioId);
                if (afiliacionPension != null)
                {
                    // Actualizar estado y número de afiliación
                    afiliacionPension.ActualizarEstado(true);
                    afiliacionPension.Numero = numeroPension;

                    // Guardar cambios
                    await _pensionRepository.UpdateAsync(afiliacionPension);
                }
            }
        }
    }
}

