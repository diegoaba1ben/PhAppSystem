using Microsoft.AspNetCore.Mvc;
using PhAppUser.Application.Queries;
using System;
using System.Threading.Tasks;

namespace PhAppUser.Controllers
{
    /// <summary>
    /// Controlador para gestionar auditorías relacionadas con usuarios.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriaController : ControllerBase
    {
        private readonly AuditoriaQuery _auditoriaQuery;

        public AuditoriaController(AuditoriaQuery auditoriaQuery)
        {
            _auditoriaQuery = auditoriaQuery;
        }

        /// <summary>
        /// Obtiene usuarios con intentos de aplazamiento excedidos o bloqueados.
        /// </summary>
        /// <returns>Lista de usuarios con problemas en la afiliación.</returns>
        [HttpGet("usuarios-con-problemas-afiliacion")]
        public async Task<IActionResult> ObtenerUsuariosConProblemasDeAfiliacion()
        {
            var usuarios = await _auditoriaQuery.ObtUsuariosConProblemasDeAfiliacionAsync();
            if (!usuarios.Any())
                return NotFound("No se encontraron usuarios con problemas de afiliación.");
            return Ok(usuarios);
        }

        /// <summary>
        /// Obtiene usuarios con afiliación parcial que han vencido el plazo de días.
        /// </summary>
        /// <param name="fechaCorte">Fecha límite para evaluar las afiliaciones.</param>
        /// <returns>Lista de usuarios con afiliación parcial.</returns>
        [HttpGet("usuarios-afiliacion-parcial")]
        public async Task<IActionResult> ObtenerUsuariosConAfiliacionParcial([FromQuery] DateTime fechaCorte)
        {
            var usuarios = await _auditoriaQuery.ObtUsuariosConAfiliacionParcialAsync(fechaCorte);
            if (!usuarios.Any())
                return NotFound("No se encontraron usuarios con afiliación parcial.");
            return Ok(usuarios);
        }
    }
}
