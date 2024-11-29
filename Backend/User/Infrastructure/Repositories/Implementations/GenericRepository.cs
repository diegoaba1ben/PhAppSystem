using Microsoft.EntityFrameworkCore;
using PhAppUser.Infrastructure.Repositories.Interfaces;
using PhAppUser.Infrastructure.Helpers;
using PhAppUser.Infrastructure.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhAppUser.Infrastructure.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly PhAppUserDbContext _context;
        private readonly ILogger<GenericRepository<T>> _logger;

        public GenericRepository(PhAppUserDbContext context, ILogger<GenericRepository<T>> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await ExceptionHandler.HandleAsync(
                async () => await _context.Set<T>().ToListAsync(),
                _logger,
                $"Error al obtener todos los registros de {typeof(T).Name}."
            );
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await ExceptionHandler.HandleAsync(
                async () => await _context.Set<T>().FindAsync(id),
                _logger,
                $"Error al obtener el registro con ID {id} de {typeof(T).Name}."
            );
        }

        public async Task AddAsync(T entity)
        {
            await ExceptionHandler.HandleAsync(
                async () =>
                {
                    await _context.Set<T>().AddAsync(entity);
                    await _context.SaveChangesAsync();
                },
                _logger,
                $"Error al intentar agregar un nuevo registro a {typeof(T).Name}."
            );
        }

        public async Task UpdateAsync(T entity)
        {
            await ExceptionHandler.HandleAsync(
                async () =>
                {
                    _context.Set<T>().Update(entity);
                    await _context.SaveChangesAsync();
                },
                _logger,
                $"Error al intentar actualizar un registro en {typeof(T).Name}."
            );
        }

        public async Task DeleteAsync(Guid id)
        {
            await ExceptionHandler.HandleAsync(
                async () =>
                {
                    var entity = await GetByIdAsync(id);
                    if (entity == null)
                    {
                        var message = $"No se encontró el registro con ID {id} en {typeof(T).Name}.";
                        _logger.LogWarning(message);
                        throw new KeyNotFoundException(message);
                    }

                    _context.Set<T>().Remove(entity);
                    await _context.SaveChangesAsync();
                },
                _logger,
                $"Error al intentar eliminar un registro en {typeof(T).Name}."
            );
        }
    }
}
