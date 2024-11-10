using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Repositories.Interfaces;

namespace PhAppUser.Infrastructure.Repositories.Implementations
{
    public class PermisoRepository : GenericRepository<Permiso>, IPermisoRepository
    {
        public PermisoRepository(DbContext context) : base(context) { }

        public async Task<Permiso?> BuscarPorCodigoAsync(string codigo)
        {
            return await _context.Set<Permiso>().FirstOrDefaultAsync(p => p.Codigo == codigo);
        }

        public async Task<bool> ExisteCodigoAsync(string codigo)
        {
            return await _context.Set<Permiso>().AnyAsync(p => p.Codigo == codigo);
        }

        public async Task<IEnumerable<Permiso>> ObtenerPermisosPorRolAsync(Guid rolId)
        {
            return await _context.Set<Permiso>()
                .Where(p => p.Roles.Any(r => r.Id == rolId))
                .ToListAsync();
        }
    }
}
