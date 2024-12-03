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

        public CuentaUsuarioController(
            ICuentaUsuarioRepository cuentaUsuarioRepository,
            ISaludRepository saludRepository,
            IPensionRepository pensionRepository,
            IPerfilRepository perfilRepository)
        {
            _cuentaUsuarioRepository = cuentaUsuarioRepository ?? throw new ArgumentNullException(nameof(cuentaUsuarioRepository));
            _saludRepository = saludRepository ?? throw new ArgumentNullException(nameof(saludRepository));
            _pensionRepository = pensionRepository ?? throw new ArgumentNullException(nameof(pensionRepository));

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
            try
            {
                var usuario = await _cuentaUsuarioRepository.GetByIdAsync(id);

                if (usuario == null)
                {
                    return NotFound(new { mensaje = $"Usuario con ID {id} no encontrado." });
                }

                usuario.InactivarUsuario(fechaInactivacion);
                await _cuentaUsuarioRepository.UpdateAsync(usuario);

                return Ok(new { message = "Usuario y sus afiliaciones inactivados correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor.", detalle = ex.Message });
            }
        }

        // Reactivar usuario
        // Reactivar usuario
        [HttpPut("{id}/reactivar")]
        public async Task<IActionResult> ReactivarUsuario(Guid id, [FromBody] ReactivacionRequest request)
        {
            try
            {
                var usuario = await _cuentaUsuarioRepository.GetByIdAsync(id);

                if (usuario == null)
                {
                    return NotFound(new { mensaje = $"Usuario con ID {id} no encontrado." });
                }

                usuario.ReactivarUsuario();

                // Actualizar afiliaciones si es necesario
                if (!string.IsNullOrEmpty(request.NumeroSalud))
                {
                    var afiliacionSalud = await _saludRepository.GetByIdAsync(usuario.Id);
                    afiliacionSalud?.ActualizarEstado(true);
                    if (afiliacionSalud != null)
                        afiliacionSalud.Numero = request.NumeroSalud;
                    await _saludRepository.UpdateAsync(afiliacionSalud!);
                }

                if (!string.IsNullOrEmpty(request.NumeroPension))
                {
                    var afiliacionPension = await _pensionRepository.GetByIdAsync(usuario.Id);
                    afiliacionPension?.ActualizarEstado(true);
                    if (afiliacionPension != null)
                        afiliacionPension.Numero = request.NumeroPension;
                    await _pensionRepository.UpdateAsync(afiliacionPension!);
                }

                await _cuentaUsuarioRepository.UpdateAsync(usuario);
                return Ok(new { mensaje = "Usuario y sus afiliaciones reactivados correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor.", detalle = ex.Message });
            }
        }

        // Obtener usuarios inactivos
        [HttpGet("usuarios-inactivos")]
        public async Task<ActionResult<IEnumerable<UsuarioInactivoDto>>> GetUsuariosInactivosAsync()
        {
            try
            {
                var usuariosInactivos = await _cuentaUsuarioRepository.GetUsuariosInactivosAsync();
                if (!usuariosInactivos.Any())
                {
                    return NotFound(new { mensaje = "No se encontraron usuarios inactivos." });
                }

                return Ok(usuariosInactivos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado al obtener usuarios inactivos.", detalle = ex.Message });
            }
        }
    }
}

