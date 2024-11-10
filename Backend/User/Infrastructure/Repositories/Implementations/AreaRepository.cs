using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Repositories.Interfaces;
using PhAppUser.Infrastructure.Context;

namespace PhAppUser.Infrastructure.Repositories.Implementations
{
    public class AreaRepository : GenericRepository<Area>, IAreaRepository
    {
        public AreaRepository(PhAppUserDbContext context) : base(context)
        {}

        public async Task<Area?> GetAreaByGuidAsync(Guid areaId)
        {
            return await _context.Set<Area>()
                .FirstOrDefaultAsync(a => a.Id == areaId);
        }
    }
}
