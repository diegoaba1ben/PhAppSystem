using Microsoft.AspNetCore.Mvc;
using PhAppUser.Domain.Entities;
using PhAppUser.Domain.Enums;
using PhAppUser.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhAppUser.Infrastructure.Repositories.Implementations;
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
            ISaludRepository  saludRepository,
            IPensionRepository pensionRepository,
            IPerfilRepository perfilRepository)
        {
            _cuentaUsuarioRepository = cuentaUsuarioRepository ?? throw new ArgumentNullException(nameof(cuentaUsuarioRepository));
            _saludRepository = _saludRepository ?? throw new ArgumentNullException(nameof(saludRepository));
            _pensionRepository = _pensionRepository ?? throw new ArgumentNullException(nameof(pensionRepository));
            _perfilRepository = _perfilRepository ?? throw new ArgumentNullException(nameof(perfilRepository));

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
        [HttpPut("{id}/inactivar")]
        public async Task<IActionResult> InactivarUsuario(Guid id, [FromBody] DateTime fechaInactivacion)
        {
            try
            {
                await _cuentaUsuarioRepository.InactivarUsuarioAsync(id, fechaInactivacion);
                return NoContent(); // Código 204
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }
        [HttpPut("{id}/inactivar-relaciones")]
        public async Task<IActionResult> InactivarUsuarioConRelaciones(Guid id, [FromBody] DateTime fechaInactivacion)
        {
            try
            {
                // Obtener usuario
                var usuario = await _cuentaUsuarioRepository.GetByIdAsync(id);
                if (usuario == null)
                {
                    return NotFound(new { mensaje = $"Usuario con ID {id} no encontrado." });
                }

                // Validar intentos y afiliación
                if (usuario.Afiliacion == Afiliacion.Parcial && usuario.Intento >= 2)
                {
                    // Obtener afiliaciones relacionadas
                    var afiliacionSalud = await _saludRepository.GetByIdAsync(id);
                    var afiliacionPension = await _pensionRepository.GetByIdAsync(id);

                    // Verificar ausencia de números de afiliación
                    bool saludIncompleta = afiliacionSalud == null || string.IsNullOrEmpty(afiliacionSalud.Numero);
                    bool pensionIncompleta = afiliacionPension == null || string.IsNullOrEmpty(afiliacionPension.Numero);

                    if (saludIncompleta || pensionIncompleta)
                    {
                        // Proceder con la inactivación del usuario
                        usuario.EsActivo = false;
                        usuario.FechaInactivacion = fechaInactivacion;
                        await _cuentaUsuarioRepository.UpdateAsync(usuario);

                        // Inactivar Salud
                        if (afiliacionSalud != null)
                        {
                            afiliacionSalud.EsActivo = false;
                            afiliacionSalud.FechaInactivacion = fechaInactivacion;
                            await _saludRepository.UpdateAsync(afiliacionSalud);
                        }

                        // Inactivar Pensión
                        if (afiliacionPension != null)
                        {
                            afiliacionPension.EsActivo = false;
                            afiliacionPension.FechaInactivacion = fechaInactivacion;
                            await _pensionRepository.UpdateAsync(afiliacionPension);
                        }

                        // Desvincular perfiles asociados
                        var perfilesRelacionados = await _perfilRepository.ObtenerPerfilesConUsuariosAsync();
                        foreach (var perfil in perfilesRelacionados.Where(p => p.CuentaUsuarios.Any(cu => cu.Id == id)))
                        {
                            perfil.CuentaUsuarios = perfil.CuentaUsuarios.Where(cu => cu.Id != id).ToList();
                            await _perfilRepository.UpdateAsync(perfil);
                        }

                        return Ok(new { mensaje = "Usuario y sus relaciones inactivados correctamente debido a la falta de afiliación." });
                    }
                }

                return BadRequest(new
                {
                    mensaje = "El usuario no cumple con las condiciones para ser inactivado en este caso."
                });
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Error al inactivar el usuario con ID {Id} y sus relaciones", id);
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }
        // Reactivación del usuario por ingresar el número de afiliación faltante.
        [HttpPost("{id}/reactivar")]
        public async Task<IActionResult> ReactivarUsuario(Guid id, [FromBody] ReactivacionRequest request)
        {
            try
            {
                // Obtener el usuario por ID
                var usuario = await _cuentaUsuarioRepository.GetByIdAsync(id);

                if (id == Guid.Empty)
                {
                    return BadRequest(new {mensaje = $"El ID del usuario no puede estar vacío." });
                }
                if (usuario == null)
                {
                    return NotFound(new { mensaje = $"Usuario con ID {id} no encontrado." });
                }

                // Validación: El usuario debe estar bloqueado
                if (!usuario.Bloqueado)
                {
                    return BadRequest(new { mensaje = "El usuario no está bloqueado." });
                }

                // Actualizar Salud
                if (!string.IsNullOrEmpty(request.NumeroSalud))
                {
                    var afiliacionSalud = await _saludRepository.GetByIdAsync(id);
                    if (afiliacionSalud != null)
                    {
                        afiliacionSalud.Numero = request.NumeroSalud;
                        afiliacionSalud.EsActivo = true;
                        await _saludRepository.UpdateAsync(afiliacionSalud);
                    }
                }

                // Actualizar Pensión
                if (!string.IsNullOrEmpty(request.NumeroPension))
                {
                    var afiliacionPension = await _pensionRepository.GetByIdAsync(id);
                    if (afiliacionPension != null)
                    {
                        afiliacionPension.Numero = request.NumeroPension;
                        afiliacionPension.EsActivo = true;
                        await _pensionRepository.UpdateAsync(afiliacionPension);
                    }
                }

                // Reactivar usuario
                usuario.EsActivo = true;
                usuario.Bloqueado = false;
                usuario.Intento = 0; //Aquí se reinician los intentos o plazos fallidos.
                await _cuentaUsuarioRepository.UpdateAsync(usuario);

                return Ok(new { mensaje = "Usuario reactivado exitosamente." });
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Error al intentar reactivar usuario con ID {Id}", id);
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }
        /// <summary>
        /// Obtiene el reporte de usuarios inactivos.
        /// </summary>
        [HttpGet("usuarios-inactivos")]
        public async Task<ActionResult<IEnumerable<UsuarioInactivoDto>>> GetUsuariosInactivosAsync()
        {
            try
            {
                var usuariosInactivos = await _cuentaUsuarioRepository.GetUsuariosInactivosAsync();

                if (usuariosInactivos == null || !usuariosInactivos.Any())
                {
                    return NotFound(new { mensaje = "No se encontraron usuarios inactivos." });
                }

                return Ok(usuariosInactivos);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Error al obtener el reporte de usuarios inactivos.");
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error inesperado al obtener el reporte de usuarios inactivos.",
                    detalle = ex.Message
                });
            }
        }

    }
}
