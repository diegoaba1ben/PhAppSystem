using Microsoft.AspNetCore.Mvc;
using PhAppUser.Application.Queries;
using PhAppUser.Application.DTOs;

namespace PhAppUser.Controllers
{
    /// <summary>
    /// Controlador para gestionar consultas avanzadas de usuarios.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AdvancedController : ControllerBase
    {
        private readonly AdvancedQuery _advancedQuery;

        public AdvancedController(AdvancedQuery advancedQuery)
        {
            _advancedQuery = advancedQuery;
        }

        /// <summary>
        /// Consulta avanzada para buscar usuarios con perfiles, roles y permisos.
        /// </summary>
        /// <param name="nombre">Nombre del usuario (opcional).</param>
        /// <param name="apellido">Apellido del usuario (opcional).</param>
        /// <param name="identificacion">Número de identificación del usuario (opcional).</param>
        /// <returns>Lista de usuarios con la información requerida.</returns>
        [HttpGet("usuarios-avanzados")]
        public async Task<IActionResult> ObtenerUsuariosAvanzados(
            [FromQuery] string? nombre,
            [FromQuery] string? apellido,
            [FromQuery] string? identificacion)
        {
            try
            {
                // Llamar al método de consulta avanzada
                var usuarios = await _advancedQuery.ObtUsuariosAvanzadosAsync(nombre, apellido, identificacion);

                // Validar si se encontraron resultados
                if (!usuarios.Any())
                {
                    return NotFound("No se encontraron usuarios con los criterios especificados.");
                }

                // Retornar la lista de usuarios
                return Ok(usuarios);
            }
            catch (ArgumentException ex)
            {
                // Manejar la excepción de parámetros inválidos
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Obtiene usuarios activos con sus perfiles y permisos
        /// </summary>
        /// <returns>lista de usuarios activos con sus perfiles y permisos</returns>
        public async Task<IActionResult> ObtUsuariosActivos()
        {
            try
            {
                // Llamar al método de consulta
                var usuarios = await _advancedQuery.ObtUsuariosActivosAsync();

                // Validar si se encontraron resultados
                if (!usuarios.Any())
                {
                    return NotFound("No se encontraron usuarios activos");
                }
                // Entrega la lista de usuarios
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                return StatusCode(500, $"Eror interno del servidor: {ex.Message}");
            }
        }
        /// <summary>
        /// Obtiene un usuario específico por su identificación.
        /// </summary>
        /// <param name="identificacion">Número de identificación del usuario.</param>
        /// <returns>Información completa del usuario.</returns>
        [HttpGet("usuario-por-identificacion")]
        public async Task<IActionResult> ObtUsuarioPorIdentificacionAsync([FromBody] string identificacion)
        {
            try
            {
                // LLamado al método de consulta.
                var usuario = await _advancedQuery.ObtUsuarioPorIdentificacionAsync(identificacion);

                // Validar si se encontró el usuario
                if (usuario == null)
                {
                    return NotFound($"No se encontró un usuario con el número de identificación {identificacion}.");
                }
                // Toma la información del usuario
                return Ok(usuario);
            }
            catch (ArgumentException ex)
            {
                // Manejar la excepción
                return BadRequest($"Error en la solicitud: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Manejo de errores inesperados
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        [HttpGet("usuarios-especiales")]
        public async Task<IActionResult> ObtUsuariosEspeciales()
        {
            try
            {
                // Llamado al método de consulta
                var usuarios = await _advancedQuery.ObtUsuariosEspecialesAsync();

                // Validación de resultados
                if (usuarios.Any())
                {
                    return NotFound("No se encontraron usuarios con los roles de representantes legales");
                }
                // Devuelve la lista de usuarios
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                // Manejo de errores inesperados
                return StatusCode(500, $"Error interno del servidor: {ex:message}");
            }
        }
        /// <summary>
        /// Obtiene la lista de usuarios con su estado de afiliación a la seguridad social
        /// </summary>
        /// <returns> Lista de usuarios con afiliaciones a salud y pensión</returns>
        [HttpGet("afiliaciones-ss")]
        public async Task<IActionResult> ObtAfiliacionesSegSUsuarios()
        {
            try
            {
                // Llamada al método de consulta
                var afiliaciones = await _advancedQuery.ObtAfiliacionesSegSUsuariosAsync();
                // Validación de resultados
                if (!afiliaciones.Any())
                {
                    return NotFound("No se encontraron usuarios con afiliaciones a la seguridad social");
                }
                return Ok(afiliaciones);
            }
            catch (Exception ex)
            {
                // Manejo de errores inesperados
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        /// <summary>
        /// Obtiene usuarios inactivos con información detallada.
        /// </summary>
        /// <returns>Lista de usuarios inactivos</returns>
        [HttpGet("usuarios-inactivos")]
        public async Task<IActionResult> ObtUsuariosInactivos()
        {
            try
            {
                // Llamada al método de consulta en AdvancedQuery
                var usuariosInactivos = await _advancedQuery.ObtUsuariosInactivosAsync();

                //Validación de los resultados
                if (!usuariosInactivos.Any())
                {
                    return NotFound("No se encontraron usuarios inactivos");
                }
                //Retorna la lista de los usuarios inactivos
                return Ok(usuariosInactivos);
            }
            catch (Exception ex)
            {
                // Manejo de errores inesperados
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        /// <summary>
        /// Obtiene usuarios que no tienen afiliación completa hasta una fecha de corte.
        /// </summary>
        /// <param name="fechaCorte">Fecha límite para evaluar las afiliaciones.</param>
        /// <returns>Lista de usuarios con afiliación parcial.</returns>
        [HttpGet("usuarios-sin-afiliacion")]
        public async Task<IActionResult> ObtUsuariosSinAfiliacion([FromQuery] DateTime fechaCorte)
        {
            try
            {
                // Llamar al método de consulta con la fecha de corte
                var usuariosSinAfiliacion = await _advancedQuery.ObtUsuariosSinAfiliacionAsync(fechaCorte);

                // Validar si hay resultados
                if (!usuariosSinAfiliacion.Any())
                {
                    return Ok($"Todos los usuarios tienen afiliaciones completas hasta la fecha {fechaCorte.ToShortDateString()}.");
                }

                // Retornar la lista de usuarios sin afiliación
                return Ok(usuariosSinAfiliacion);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

    }
}
