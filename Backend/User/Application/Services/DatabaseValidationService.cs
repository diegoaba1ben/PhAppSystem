using Microsoft.EntityFrameworkCore;
using PhAppUser.Infrastructure.Context;

namespace PhAppUser.Application.Services.Validation
{
    public class DatabaseValidationService
    {
        private readonly PhAppUserDbContext _context;

        public DatabaseValidationService(PhAppUserDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Verifica si el nombre de usuario es único.
        /// </summary>
        public async Task<bool> NombreUsuarioEsUnicoAsync(string nombreUsuario)
        {
            return !await _context.CuentasUsuarios.AnyAsync(cu => cu.NombreUsuario == nombreUsuario);
        }

        /// <summary>
        /// Verifica si la identificación es única.
        /// </summary>
        public async Task<bool> IdentificacionEsUnicaAsync(string identificacion)
        {
            return !await _context.CuentasUsuarios.AnyAsync(cu => cu.Identificacion == identificacion);
        }

        /// <summary>
        /// Verifica si el número de certificación legal de RepLegal es único.
        /// </summary>
        public async Task<bool> CertLegalEsUnicoAsync(string certLegal)
        {
            return !await _context.RepLegals.AnyAsync(rl => rl.CertLegal == certLegal);
        }
    }
}
