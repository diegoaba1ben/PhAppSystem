using Microsoft.AspNetCore.Mvc;
using PhAppUser.Application.DTOs;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Repositories.Interfaces;
using AutoMapper;

namespace PhAppUser.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaludController : ControllerBase
    {
        private readonly ISaludRepository _saludRepository;
        private readonly IMapper _mapper;

        public SaludController(ISaludRepository saludRepository, IMapper mapper)
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
                return NotFound(new { mensaje = $"No se encontró la afiliación de salud con ID {id}." });
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
                return NotFound(new { mensaje = $"No se encontró la afiliación de salud con ID {id}." });
            }

            _mapper.Map(saludDto, saludEntity);
            await _saludRepository.UpdateAsync(saludEntity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSaludAsync(Guid id)
        {
            var saludEntity = await _saludRepository.GetByIdAsync(id);
            if (saludEntity == null)
            {
                return NotFound(new { mensaje = $"No se encontró la afiliación de salud con ID {id}." });
            }

            await _saludRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
