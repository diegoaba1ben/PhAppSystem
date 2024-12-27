using Microsoft.EntityFrameworkCore;
using PhAppUser.Infrastructure.Context;
using System;
using System.Threading.Tasks;

namespace PhAppUser.Application.Services.Validation
{
    public class DtoValidationService
    {
        private readonly PhAppUserDbContext _context;

        public DtoValidationService(PhAppUserDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Verifica si el nombre de usuario es único.
        /// </summary>
        public async Task<bool> NombreUsuarioEsUnicoAsync(string nombreUsuario)
        {
            if (string.IsNullOrEmpty(nombreUsuario))
                throw new ArgumentException("El nombre de usuario no puede ser nulo o vacío.");

            return !await _context.CuentasUsuarios.AnyAsync(cu => cu.NombreUsuario == nombreUsuario);
        }

        /// <summary>
        /// Verifica si la identificación es única.
        /// </summary>
        public async Task<bool> IdentificacionEsUnicaAsync(string identificacion)
        {
            if (string.IsNullOrEmpty(identificacion))
                throw new ArgumentException("La identificación no puede ser nula o vacía.");

            return !await _context.CuentasUsuarios.AnyAsync(cu => cu.Identificacion == identificacion);
        }

        /// <summary>
        /// Verifica si el número de certificación legal es único.
        /// </summary>
        public async Task<bool> CertLegalEsUnicoAsync(string certLegal)
        {
            if (string.IsNullOrEmpty(certLegal))
                throw new ArgumentException("El número de certificado legal no puede ser nulo o vacío.");

            return !await _context.RepLegals.AnyAsync(rl => rl.CertLegal == certLegal);
        }
    }
}

