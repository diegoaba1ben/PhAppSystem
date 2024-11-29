using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Context;
using PhAppUser.Infrastructure.Repositories.Interfaces;

namespace PhAppUser.Infrastructure.Repositories.Implementations
{
    public class RolRepository : GenericRepository<Rol>, IRolRepository
    {
        public RolRepository(PhAppUserDbContext context, ILogger<RolRepository> logger) : base(context, logger) { }

        public async Task<Rol?> BuscarPorNombreAsync(string nombre)
        {
            return await _context.Set<Rol>()
                .FirstOrDefaultAsync(r => r.Nombre == nombre);
        }

        public async Task<bool> TienePermisoAsync(Guid rolId, Guid permisoId)
        {
            return await _context.Set<Rol>()
                .AnyAsync(r => r.Id == rolId && r.Permisos.Any(p => p.Id == permisoId));
        }
    }
}
