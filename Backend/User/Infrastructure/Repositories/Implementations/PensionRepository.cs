using Microsoft.EntityFrameworkCore;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Context;
using PhAppUser.Infrastructure.Repositories.Interfaces;
using Serilog;

namespace PhAppUser.Infrastructure.Repositories.Implementations
{
    public class PensionRepository : GenericRepository<Pension>, IPensionRepository
    {
        public PensionRepository(PhAppUserDbContext context, ILogger<PensionRepository> logger) : base(context, logger) { }

        /// <summary>
        /// Verifica si ya existe un registro con el mismo número de afiliación en pensión.
        /// </summary>
        public async Task<bool> ExisteNumeroAsync(string numero)
        {
            try
            {
                return await _context.Set<Pension>().AnyAsync(p => p.Numero == numero);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al verificar la existencia del número de afiliación {Numero}", numero);
                throw new Exception("Ocurrió un error al validar la existencia del número de afiliación", ex);
            }
        }
    }
}

