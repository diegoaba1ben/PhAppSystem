using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Repositories.Interfaces;

namespace PhAppUser.Infrastructure.Repositories.Implementations
{
    public class PerfilRepository : GenericRepository<Perfil>, IPerfilRepository
    {
        public PerfilRepository(DbContext context) : base(context) { }

        // Métodos básicos de búsqueda y validación
        public async Task<Perfil?> BuscarPorIdAsync(Guid perfilId)
        {
            return await _context.Set<Perfil>()
                .Include(p => p.CuentaUsuarios)
                .Include(p => p.Roles)
                .FirstOrDefaultAsync(p => p.Id == perfilId);
        }

        public async Task<bool> ExistePerfilAsync(Guid perfilId)
        {
            return await _context.Set<Perfil>()
                .AnyAsync(p => p.Id == perfilId);
        }

        // Métodos de consultas relacionales
        public async Task<IEnumerable<Perfil>> ObtenerPerfilesConUsuariosAsync()
        {
            return await _context.Set<Perfil>()
                .Include(p => p.CuentaUsuarios)
                .ToListAsync();
        }

        public async Task<IEnumerable<Perfil>> ObtenerPerfilesPorAreaAsync(Guid areaId)
        {
            return await _context.Set<Perfil>()
                .Where(p => p.AreaId == areaId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Perfil>> ObtenerPerfilesConRolesAsync()
        {
            return await _context.Set<Perfil>()
                .Include(p => p.Roles)
                .ToListAsync();
        }

        // Métodos de consultas avanzadas
        public async Task<bool> TieneRolAsync(Guid perfilId, Guid rolId)
        {
            return await _context.Set<Perfil>()
                .AnyAsync(p => p.Id == perfilId && p.Roles.Any(r => r.Id == rolId));
        }

        public async Task<IEnumerable<Rol>> ObtenerRolesYPermisosDePerfilAsync(Guid perfilId)
        {
            var perfil = await _context.Set<Perfil>()
                .Include(p => p.Roles)
                .ThenInclude(r => r.Permisos)
                .FirstOrDefaultAsync(p => p.Id == perfilId);

            return perfil?.Roles ?? new List<Rol>();
        }

        // Métodos de gestión avanzada
        public async Task ActualizarRolesAsync(Guid perfilId, ICollection<Rol> nuevosRoles)
        {
            var perfil = await _context.Set<Perfil>()
                .Include(p => p.Roles)
                .FirstOrDefaultAsync(p => p.Id == perfilId);

            if (perfil != null)
            {
                // Limpiar roles existentes y asignar los nuevos roles
                perfil.Roles.Clear();
                perfil.Roles = nuevosRoles;

                _context.Set<Perfil>().Update(perfil);
                await _context.SaveChangesAsync();
            }
        }

        public async Task EliminarPerfilConRelacionesAsync(Guid perfilId)
        {
            var perfil = await _context.Set<Perfil>()
                .Include(p => p.CuentaUsuarios)
                .Include(p => p.Roles)
                .FirstOrDefaultAsync(p => p.Id == perfilId);

            if (perfil != null)
            {
                // Desvincular usuarios y roles del perfil antes de eliminarlo
                perfil.CuentaUsuarios.Clear();
                perfil.Roles.Clear();

                _context.Set<Perfil>().Remove(perfil);
                await _context.SaveChangesAsync();
            }
        }
    }
}
