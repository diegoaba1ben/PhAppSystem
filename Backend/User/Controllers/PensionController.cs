using Microsoft.AspNetCore.Mvc;
using PhAppUser.Application.DTOs;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Repositories.Interfaces;
using AutoMapper;

namespace PhAppUser.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PensionController : ControllerBase
    {
        private readonly IPensionRepository _pensionRepository;
        private readonly IMapper _mapper;

        public PensionController(IPensionRepository pensionRepository, IMapper mapper)
        {
            _pensionRepository = pensionRepository ?? throw new ArgumentNullException(nameof(pensionRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PensionDto>>> GetAllPensionAsync()
        {
            var pensionEntities = await _pensionRepository.GetAllAsync();
            var pensionDtos = _mapper.Map<IEnumerable<PensionDto>>(pensionEntities);
            return Ok(pensionDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PensionDto>> GetPensionByIdAsync(Guid id)
        {
            var pensionEntity = await _pensionRepository.GetByIdAsync(id);
            if (pensionEntity == null)
            {
                return NotFound(new { mensaje = $"No se encontró la afiliación de pensión con ID {id}." });
            }

            var pensionDto = _mapper.Map<PensionDto>(pensionEntity);
            return Ok(pensionDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePensionAsync([FromBody] PensionDto pensionDto)
        {
            var pensionEntity = _mapper.Map<Pension>(pensionDto);
            await _pensionRepository.AddAsync(pensionEntity);
            return CreatedAtAction(nameof(GetPensionByIdAsync), new { id = pensionEntity.Id }, pensionDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePensionAsync(Guid id, [FromBody] PensionDto pensionDto)
        {
            var pensionEntity = await _pensionRepository.GetByIdAsync(id);
            if (pensionEntity == null)
            {
                return NotFound(new { mensaje = $"No se encontró la afiliación de pensión con ID {id}." });
            }

            _mapper.Map(pensionDto, pensionEntity);
            await _pensionRepository.UpdateAsync(pensionEntity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePensionAsync(Guid id)
        {
            var pensionEntity = await _pensionRepository.GetByIdAsync(id);
            if (pensionEntity == null)
            {
                return NotFound(new { mensaje = $"No se encontró la afiliación de pensión con ID {id}." });
            }

            await _pensionRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}

