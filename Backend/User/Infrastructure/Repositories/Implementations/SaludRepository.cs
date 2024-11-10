using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Repositories.Interfaces;

namespace PhAppUser.Infrastructure.Repositories.Implementations
{
    public class SaludRepository : GenericRepository<Salud>, ISaludRepository
    {
        public SaludRepository(DbContext context) : base(context)
        {}
        //Implementaciones.
        public async Task<bool> ExisteNumeroAsync(string numero)
        {
            return await _context.Set<Salud>().AnyAsync(s => s.Numero == numero);
        }
    } 
}