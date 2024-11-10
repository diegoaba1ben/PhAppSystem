using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhAppUser.Infrastructure.Repositories.Interfaces;

namespace PhAppUser.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenericController<T> : ControllerBase where T : class
    {
        protected readonly IGenericRepository<T> _repository;

        public GenericController(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        // Obtener todos los elementos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> GetAll()
        {
            var items = await _repository.GetAllAsync();
            return Ok(items);
        }

        // Crear un nuevo elemento
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] T entity)
        {
            await _repository.AddAsync(entity);
            return CreatedAtAction(nameof(GetAll), entity);
        }

        // Actualizar un elemento existente
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] T entity)
        {
            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                return NotFound();
            }
            await _repository.UpdateAsync(entity);
            return NoContent();
        }

        // Eliminar un elemento por ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                return NotFound();
            }
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
