using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Context;
using PhAppUser.Infrastructure.Repositories.Interfaces;
using Serilog;

namespace PhAppUser.Infrastructure.Repositories.Implementations
{
    public class PerfilRepository : GenericRepository<Perfil>, IPerfilRepository
    {
        public PerfilRepository(PhAppUserDbContext context, ILogger<PerfilRepository> logger) : base(context, logger) { }

        // Métodos básicos de búsqueda y validación
        public async Task<Perfil?> BuscarPorIdAsync(Guid perfilId)
        {
            try
            {
                return await _context.Set<Perfil>()
                    .Include(p => p.CuentaUsuarios)
                    .Include(p => p.Roles)
                    .FirstOrDefaultAsync(p => p.Id == perfilId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al buscar el perfil con Id {PerfilId}", perfilId);
                throw new Exception($"Error al buscar el perfil con Id {perfilId}", ex);
            }
        }

        public async Task<bool> ExistePerfilAsync(Guid perfilId)
        {
            try
            {
                return await _context.Set<Perfil>()
                    .AnyAsync(p => p.Id == perfilId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al verificar si existe el perfil con Id {PerfilId}", perfilId);
                throw new Exception($"Error al verificar si existe el perfil con Id {perfilId}", ex);
            }
        }

        // Métodos de consultas relacionales
        public async Task<IEnumerable<Perfil>> ObtenerPerfilesConUsuariosAsync()
        {
            try
            {
                return await _context.Set<Perfil>()
                    .Include(p => p.CuentaUsuarios)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al obtener los perfiles con usuarios");
                throw new Exception("Error al obtener los perfiles con usuarios", ex);
            }
        }

        public async Task<IEnumerable<Perfil>> ObtenerPerfilesPorAreaAsync(Guid areaId)
        {
            try
            {
                return await _context.Set<Perfil>()
                    .Where(p => p.AreaId == areaId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al obtener perfiles por área {AreaId}", areaId);
                throw new Exception($"Error al obtener perfiles por área {areaId}", ex);
            }
        }

        public async Task<IEnumerable<Perfil>> ObtenerPerfilesConRolesAsync()
        {
            try
            {
                return await _context.Set<Perfil>()
                    .Include(p => p.Roles)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al obtener los perfiles con roles");
                throw new Exception("Error al obtener los perfiles con roles", ex);
            }
        }

        // Métodos de consultas avanzadas
        public async Task<bool> TieneRolAsync(Guid perfilId, Guid rolId)
        {
            try
            {
                return await _context.Set<Perfil>()
                    .AnyAsync(p => p.Id == perfilId && p.Roles.Any(r => r.Id == rolId));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al verificar si el perfil {PerfilId} tiene el rol {RolId}", perfilId, rolId);
                throw new Exception($"Error al verificar si el perfil {perfilId} tiene el rol {rolId}", ex);
            }
        }

        public async Task<IEnumerable<Rol>> ObtenerRolesYPermisosDePerfilAsync(Guid perfilId)
        {
            try
            {
                var perfil = await _context.Set<Perfil>()
                    .Include(p => p.Roles)
                    .ThenInclude(r => r.Permisos)
                    .FirstOrDefaultAsync(p => p.Id == perfilId);

                return perfil?.Roles ?? new List<Rol>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al obtener los roles y permisos del perfil {PerfilId}", perfilId);
                throw new Exception($"Error al obtener los roles y permisos del perfil {perfilId}", ex);
            }
        }

        // Métodos de gestión avanzada
        public async Task ActualizarRolesAsync(Guid perfilId, ICollection<Rol> nuevosRoles)
        {
            try
            {
                var perfil = await _context.Set<Perfil>()
                    .Include(p => p.Roles)
                    .FirstOrDefaultAsync(p => p.Id == perfilId);

                if (perfil != null)
                {
                    perfil.Roles.Clear();
                    perfil.Roles = nuevosRoles;

                    _context.Set<Perfil>().Update(perfil);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al actualizar los roles del perfil {PerfilId}", perfilId);
                throw new Exception($"Error al actualizar los roles del perfil {perfilId}", ex);
            }
        }

        public async Task EliminarPerfilConRelacionesAsync(Guid perfilId)
        {
            try
            {
                var perfil = await _context.Set<Perfil>()
                    .Include(p => p.CuentaUsuarios)
                    .Include(p => p.Roles)
                    .FirstOrDefaultAsync(p => p.Id == perfilId);

                if (perfil != null)
                {
                    perfil.CuentaUsuarios.Clear();
                    perfil.Roles.Clear();

                    _context.Set<Perfil>().Remove(perfil);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al eliminar el perfil con Id {PerfilId} y sus relaciones", perfilId);
                throw new Exception($"Error al eliminar el perfil con Id {perfilId} y sus relaciones", ex);
            }
        }
    }
}

