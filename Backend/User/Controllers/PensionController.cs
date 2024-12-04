using Microsoft.AspNetCore.Mvc;
using PhAppUser.Application.DTOs;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Repositories.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

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
            var pensionEntity = await ObtenerPensionEntityPorIdAsync(id);
            if (pensionEntity == null)
            {
                return NotFound(new { message = $"No se encontró la afiliación de pensión con ID {id}." });
            }

            var pensionDto = _mapper.Map<PensionDto>(pensionEntity);
            return Ok(pensionDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePensionAsync([FromBody] PensionDto pensionDto)
        {
            var pensionEntity = _mapper.Map<Pension>(pensionDto);
            if (pensionEntity == null)
            {
                return BadRequest(new { message = "El mapeo del objeto falló. Verifique los datos enviados." });
            }

            await _pensionRepository.AddAsync(pensionEntity);
            return CreatedAtAction(nameof(GetPensionByIdAsync), new { id = pensionEntity.Id }, pensionDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePensionAsync(Guid id, [FromBody] PensionDto pensionDto)
        {
            var pensionEntity = await ObtenerPensionEntityPorIdAsync(id);
            if (pensionEntity == null)
            {
                return NotFound(new { message = $"No se encontró la afiliación de pensión con ID {id}." });
            }

            _mapper.Map(pensionDto, pensionEntity);
            await _pensionRepository.UpdateAsync(pensionEntity);

            return NoContent();
        }

        [HttpPatch("{id}/inactivar")]
        public async Task<IActionResult> Inactivar(Guid id)
        {
            var pensionEntity = await ObtenerPensionEntityPorIdAsync(id);
            if (pensionEntity == null)
            {
                return NotFound(new { message = $"No se encontró la afiliación de pensión con ID {id}." });
            }

            pensionEntity.EsActivo = false;
            await _pensionRepository.UpdateAsync(pensionEntity);
            return NoContent();
        }

        [HttpPatch("{id}/reactivar")]
        public async Task<IActionResult> Reactivar(Guid id)
        {
            var pensionEntity = await ObtenerPensionEntityPorIdAsync(id);
            if (pensionEntity == null)
            {
                return NotFound(new { message = $"No se encontró la afiliación de pensión con ID {id}." });
            }

            pensionEntity.EsActivo = true;
            await _pensionRepository.UpdateAsync(pensionEntity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePensionAsync(Guid id)
        {
            var pensionEntity = await ObtenerPensionEntityPorIdAsync(id);
            if (pensionEntity == null)
            {
                return NotFound(new { message = $"No se encontró la afiliación de pensión con ID {id}." });
            }

            await _pensionRepository.DeleteAsync(id);
            return NoContent();
        }

        // Método privado para reutilizar lógica de obtención de entidades.
        private async Task<Pension?> ObtenerPensionEntityPorIdAsync(Guid id)
        {
            return await _pensionRepository.GetByIdAsync(id);
        }
    }
}


