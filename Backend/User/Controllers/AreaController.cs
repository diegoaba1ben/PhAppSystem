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
    public class AreaController : ControllerBase
    {
        private readonly IAreaRepository _areaRepository;
        private readonly ILogger<AreaController> _logger;

        public AreaController(IAreaRepository areaRepository, ILogger<AreaController> logger)
        {
            _areaRepository = areaRepository;
            _logger = logger;
        }

        [HttpGet("nombre/{nombre}")]
        public async Task<IActionResult> ObtenerPorNombre(string nombre)
        {
            var area = await _areaRepository.BuscarPorNombreAsync(nombre);
            if (area == null)
            {
                return NotFound("Área no encontrada.");
            }
            return Ok(area);
        }

        [HttpGet("conRoles")]
        public async Task<IActionResult> ObtenerAreasConRoles()
        {
            var areas = await _areaRepository.ObtenerAreasConRolesAsync();
            return Ok(areas);
        }
    }
}
