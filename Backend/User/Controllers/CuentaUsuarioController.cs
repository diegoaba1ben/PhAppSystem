using Microsoft.AspNetCore.Mvc;
using PhAppUser.Domain.Entities;
using PhAppUser.Domain.Enums;
using PhAppUser.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhAppUser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentaUsuarioController : ControllerBase
    {
        private readonly ICuentaUsuarioRepository _cuentaUsuarioRepository;

        public CuentaUsuarioController(ICuentaUsuarioRepository cuentaUsuarioRepository)
        {
            _cuentaUsuarioRepository = cuentaUsuarioRepository;
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
    }
}
