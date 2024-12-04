using Microsoft.AspNetCore.Mvc;
using PhAppUser.Application.DTOs;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Repositories.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace PhAppUser.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaludController : GenericController<Salud>
    {
        private readonly ISaludRepository _saludRepository;
        private readonly IMapper _mapper;

        public SaludController(ISaludRepository saludRepository, IMapper mapper, ILogger<GenericController<Salud>> logger)
            : base(saludRepository, logger)
        {
            _saludRepository = saludRepository ?? throw new ArgumentNullException(nameof(saludRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaludDto>>> GetAllSaludAsync()
        {
            var saludEntities = await _saludRepository.GetAllAsync();
            var saludDtos = _mapper.Map<IEnumerable<SaludDto>>(saludEntities);
            return Ok(saludDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SaludDto>> GetSaludByIdAsync(Guid id)
        {
            var saludEntity = await _saludRepository.GetByIdAsync(id);
            if (saludEntity == null)
            {
                return NotFound(new { message = $"No se encontró la afiliación de salud con ID {id}." });
            }

            var saludDto = _mapper.Map<SaludDto>(saludEntity);
            return Ok(saludDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSaludAsync([FromBody] SaludDto saludDto)
        {
            var saludEntity = _mapper.Map<Salud>(saludDto);
            await _saludRepository.AddAsync(saludEntity);
            return CreatedAtAction(nameof(GetSaludByIdAsync), new { id = saludEntity.Id }, saludDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSaludAsync(Guid id, [FromBody] SaludDto saludDto)
        {
            var saludEntity = await _saludRepository.GetByIdAsync(id);
            if (saludEntity == null)
            {
                return NotFound(new { message = $"No se encontró la afiliación de salud con ID {id}." });
            }

            _mapper.Map(saludDto, saludEntity);
            await _saludRepository.UpdateAsync(saludEntity);

            return NoContent();
        }

        [HttpPatch("{id}/inactivar")]
        public async Task<IActionResult> Inactivar(Guid id)
        {
            var saludEntity = await ObtenerSaludEntityPorIdAsync(id);
            if (saludEntity == null)
            {
                return NotFound(new { message = $"No se encontró la afiliación de salud con ID {id}." });
            }

            saludEntity.EsActivo = false;
            await _saludRepository.UpdateAsync(saludEntity);
            return NoContent();
        }

        [HttpPatch("{id}/reactivar")]
        public async Task<IActionResult> Reactivar(Guid id)
        {
            var saludEntity = await ObtenerSaludEntityPorIdAsync(id);
            if (saludEntity == null)
            {
                return NotFound(new { message = $"No se encontró la afiliación de salud con ID {id}." });
            }

            saludEntity.EsActivo = true;
            await _saludRepository.UpdateAsync(saludEntity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSaludAsync(Guid id)
        {
            var saludEntity = await ObtenerSaludEntityPorIdAsync(id);
            if (saludEntity == null)
            {
                return NotFound(new { message = $"No se encontró la afiliación de salud con ID {id}." });
            }

            await _saludRepository.DeleteAsync(id);
            return NoContent();
        }

        // Método privado para reutilizar lógica de obtención de entidades.
        private async Task<Salud?> ObtenerSaludEntityPorIdAsync(Guid id)
        {
            return await _saludRepository.GetByIdAsync(id);
        }
    }
}

