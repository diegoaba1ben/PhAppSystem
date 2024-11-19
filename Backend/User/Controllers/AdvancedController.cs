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
    }
}
