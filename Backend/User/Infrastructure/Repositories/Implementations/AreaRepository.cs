using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Context;
using PhAppUser.Infrastructure.Repositories.Interfaces;

namespace PhAppUser.Infrastructure.Repositories.Implementations
{
    public class AreaRepository : GenericRepository<Area>, IAreaRepository
    {
        public AreaRepository(PhAppUserDbContext context, ILogger<AreaRepository> logger) : base(context, logger) { }

        public async Task<Area?> BuscarPorNombreAsync(string nombre)
        {
            return await _context.Set<Area>()
                .FirstOrDefaultAsync(a => a.Nombre == nombre);
        }

        public async Task<IEnumerable<Area>> ObtenerAreasConRolesAsync()
        {
            return await _context.Set<Area>()
                .Include(a => a.Roles) // Incluir los roles asociados a cada área
                .ToListAsync();
        }
    }
}
