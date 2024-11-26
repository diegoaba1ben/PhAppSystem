using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Context;
using PhAppUser.Infrastructure.Repositories.Interfaces;
using Serilog;

namespace PhAppUser.Infrastructure.Repositories.Implementations
{
    public class SaludRepository : GenericRepository<Salud>, ISaludRepository
    {
        public SaludRepository(PhAppUserDbContext context) : base(context) { }

        /// <summary>
        /// Verifica si ya existe un registro con el mismo número de afiliación.
        /// </summary>
        public async Task<bool> ExisteNumeroAsync(string numero)
        {
            try
            {
                return await _context.Set<Salud>().AnyAsync(s => s.Numero == numero);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al verificar la existencia del número de afiliación {Numero} en Salud", numero);
                throw new Exception("Ocurrió un error al validar la existencia del número de afiliación en Salud.", ex);
            }
        }
    }
}
