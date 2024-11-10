using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Repositories.Interfaces;

namespace PhAppUser.Infrastructure.Repositories.Implementations
{
    public class PensionRepository : GenericRepository<Pension>, IPensionRepository
    {
        public PensionRepository(DbContext context) : base(context) { }

        public async Task<bool> ExisteNumeroAsync(string numero)
        {
            return await _context.Set<Pension>().AnyAsync(p => p.Numero == numero);
        }
    }
}
