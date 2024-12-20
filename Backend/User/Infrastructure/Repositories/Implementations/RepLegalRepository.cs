using Microsoft.EntityFrameworkCore;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Context;
using PhAppUser.Infrastructure.Repositories.Interfaces;

namespace PhAppUser.Infrastructure.Repositories.Implementations
{
    public class RepLegalRepository : GenericRepository<RepLegal>, IRepLegalRepository
    {
       

        public RepLegalRepository(PhAppUserDbContext context, ILogger<RepLegalRepository> logger) : base(context, logger) {}

        public async Task<bool> ExisteCertLegalAsync(string certLegal)
        {
            return await _context.Set<RepLegal>().AnyAsync(r => r.CertLegal == certLegal);
        }

        public async Task<IEnumerable<RepLegal>> ObtenerRepresentantesActivosAsync()
        {
            var fechaActual = DateTime.Now;
            return await _context.Set<RepLegal>()
                .Where(r => r.FechaInicio <= fechaActual && r.FechaFinal >= fechaActual)
                .ToListAsync();
        }

        public async Task<bool> ExisteSuperposicionDeFechasAsync(DateTime fechaInicio, DateTime fechaFinal)
        {
            return await _context.Set<RepLegal>().AnyAsync(r =>
                (fechaInicio >= r.FechaInicio && fechaInicio <= r.FechaFinal) ||
                (fechaFinal >= r.FechaInicio && fechaFinal <= r.FechaFinal) ||
                (fechaInicio <= r.FechaInicio && fechaFinal >= r.FechaFinal));
        }

        public async Task<IEnumerable<RepLegal>> ObtenerHistorialRepresentantesAsync()
        {
            return await _context.Set<RepLegal>()
                .OrderByDescending(r => r.FechaInicio)
                .ToListAsync();
        }
    }
}

