using Microsoft.EntityFrameworkCore;
using PhAppUser.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog;

namespace PhAppUser.Infrastructure.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext _context;

        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _context.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al obtener todos los registros de {EntityName}", typeof(T).Name);
                throw new Exception($"Error al obtener todos los registros de {typeof(T).Name}.", ex);
            }
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al obtener el registro con ID {Id} de {EntityName}", id, typeof(T).Name);
                throw new Exception($"Error al obtener el registro con ID {id} de {typeof(T).Name}.", ex);
            }
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                Log.Error(dbEx, "Error al intentar agregar un nuevo registro a {EntityName}", typeof(T).Name);
                throw new Exception($"Error al intentar agregar un nuevo registro a {typeof(T).Name}.", dbEx);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error general al intentar agregar un nuevo registro a {EntityName}", typeof(T).Name);
                throw new Exception($"Error general al intentar agregar un nuevo registro a {typeof(T).Name}.", ex);
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                Log.Error(dbEx, "Error de concurrencia al intentar actualizar un registro en {EntityName}", typeof(T).Name);
                throw new Exception($"Error de concurrencia al intentar actualizar un registro en {typeof(T).Name}.", dbEx);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error general al intentar actualizar un registro en {EntityName}", typeof(T).Name);
                throw new Exception($"Error general al intentar actualizar un registro en {typeof(T).Name}.", ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity == null)
                {
                    Log.Warning("No se encontró el registro con ID {Id} en {EntityName} para eliminar.", id, typeof(T).Name);
                    throw new KeyNotFoundException($"No se encontró el registro con ID {id} en {typeof(T).Name}.");
                }

                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                Log.Error(dbEx, "Error al intentar eliminar un registro en {EntityName}", typeof(T).Name);
                throw new Exception($"Error al intentar eliminar un registro en {typeof(T).Name}.", dbEx);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error general al intentar eliminar un registro en {EntityName}", typeof(T).Name);
                throw new Exception($"Error general al intentar eliminar un registro en {typeof(T).Name}.", ex);
            }
        }
    }
}

