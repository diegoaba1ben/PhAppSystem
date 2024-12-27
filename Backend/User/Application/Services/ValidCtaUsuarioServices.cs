using System;
using Microsoft.EntityFrameworkCore;
using PhAppUser.Infrastructure.Context;

namespace PhAppUser.Application.Services
{
    /// <summary>
    /// Servicio para manejar validaciones relacionadas conla entidad CuentaUsuario.
    /// </summary>
    public class ValidCtaUsuariosService
    {
        private readonly PhAppUserDbContext _context;
        public ValidCtaUsuariosService(PhAppUserDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        /// <summary>
        /// Verifica si el nombre de usuario es único en el sistema.
        /// </summary>
        /// <param name="nombreUsuario"> Nombre del usuario a verificar</param>
        /// <returns>Verdadero si el nombre es único, falso si el nombre ya existe.</returns>
        public async Task<bool> NombreUsuarioEsUnicoAsync(string nombreUsuario)
        {
            // Validación de nombre de usuario único
            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                throw new ArgumentNullException("El nombre del usuario no puede ser nulo o vacío.");
                }
                return !await _context.CuentasUsuarios
                    .AnyAsync(cu => cu.NombreUsuario == nombreUsuario);
            }
        /// <summary>
        /// Verifica si la certificación es única en el sistema.
        /// </summary>
        /// <param name="certLegal">Número de certificación a verificar.</param>
        /// <returns>Verdadero si la certificación es única, falso si la certificación ya existe.</returns>
        public async Task<bool> CertLegalEsUnicoAsync(string certLegal)
        {
            // Validación de certificación única
            if (string.IsNullOrWhiteSpace(certLegal))
            {
                throw new ArgumentNullException("El número de certificación no puede ser nulo o vacío.");
            }
            return !await _context.RepLegals
                .AnyAsync(rl => rl.CertLegal == certLegal);
        }
    }
}

