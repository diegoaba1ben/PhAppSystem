using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhAppUser.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace PhAppUser.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenericController<T> : ControllerBase where T : class
    {
        protected readonly IGenericRepository<T> _repository;
        protected readonly ILogger<GenericController<T>> _logger;

        public GenericController(IGenericRepository<T> repository, ILogger<GenericController<T>> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // Obtener todos los elementos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> GetAll()
        {
            try
            {
                var items = await _repository.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los elementos del repositorio.");
                return StatusCode(500, "Ocurrió un error al procesar su solicitud.");
            }

        }

        // Crear un nuevo elemento
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] T entity)
        {
            try
            {
                if (entity == null)
                {
                    _logger.LogWarning("Intento de creación de un objeto nulo en {EntityName}.", typeof(T).Name);
                    return BadRequest("El objeto no puede ser nulo.");
                }

                await _repository.AddAsync(entity);
                _logger.LogInformation("Se creó un nuevo registro de {EntityName}.", typeof(T).Name);
                return CreatedAtAction(nameof(GetAll), entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo registro en {EntityName}.", typeof(T).Name);
                return StatusCode(500, "Ocurrió un error al procesar su solicitud.");
            }
        }


        // Actualizar un elemento existente
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] T entity)
        {
            try
            {
                if (entity == null)
                {
                    _logger.LogWarning("Intento de actualización de un objeto nulo en {EntityName}.", typeof(T).Name);
                    return BadRequest("El objeto no puede ser nulo.");
                }

                var existingEntity = await _repository.GetByIdAsync(id);
                if (existingEntity == null)
                {
                    _logger.LogWarning("Intento de actualización fallido: No se encontró la entidad con ID {Id} en {EntityName}.", id, typeof(T).Name);
                    return NotFound($"No se encontró la entidad con ID {id}.");
                }

                await _repository.UpdateAsync(entity);
                _logger.LogInformation("Registro actualizado exitosamente en {EntityName} con ID {Id}.", typeof(T).Name, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el registro en {EntityName} con ID {Id}.", typeof(T).Name, id);
                return StatusCode(500, "Ocurrió un error al procesar su solicitud.");
            }
        }


        // Eliminar un elemento por ID
        [HttpDelete("{id}")]
public async Task<ActionResult> Delete(Guid id)
{
    try
    {
        var existingEntity = await _repository.GetByIdAsync(id);
        if (existingEntity == null)
        {
            _logger.LogWarning("Intento de eliminación fallido: No se encontró la entidad con ID {Id} en {EntityName}.", id, typeof(T).Name);
            return NotFound($"No se encontró la entidad con ID {id}.");
        }

        await _repository.DeleteAsync(id);
        _logger.LogInformation("Registro eliminado exitosamente en {EntityName} con ID {Id}.", typeof(T).Name, id);
        return NoContent();
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al eliminar el registro en {EntityName} con ID {Id}.", typeof(T).Name, id);
        return StatusCode(500, "Ocurrió un error al procesar su solicitud.");
    }
}

    }
}
