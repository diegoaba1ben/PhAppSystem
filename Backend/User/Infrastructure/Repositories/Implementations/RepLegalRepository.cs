using Microsoft.EntityFrameworkCore;
using PhAppUser.Domain.Entities;
using PhAppUser.Domain.Enums;
using PhAppUser.Infrastructure.Context;
using PhAppUser.Infrastructure.Repositories.Interfaces;

namespace PhAppUser.Infrastructure.Repositories.Implementations
{
    /// <summary>
    /// Repositorio para manejar operaciones relacionadas con la entidad RepLegal.
    /// </summary>
    public class RepLegalRepository : GenericRepository<RepLegal>, IRepLegalRepository
    {
        public RepLegalRepository(PhAppUserDbContext context, ILogger<RepLegalRepository> logger) : base(context, logger) { }

        /// <summary>
        /// Verifica si un número de certificación legal ya existe.
        /// </summary>
        /// <param name="certLegal">Número de certificación legal a verificar.</param>
        /// <returns>Verdadero si el número de certificación ya existe; de lo contrario, falso.</returns>
        public async Task<bool> ExisteCertLegalAsync(string certLegal)
        {
            return await _context.Set<RepLegal>().AnyAsync(r => r.CertLegal == certLegal);
        }

        /// <summary>
        /// Obtiene los representantes legales activos en función de las fechas actuales.
        /// </summary>
        /// <returns>Lista de representantes legales activos.</returns>
        public async Task<IEnumerable<RepLegal>> ObtenerRepresentantesActivosAsync()
        {
            var fechaActual = DateTime.Now;
            return await _context.Set<RepLegal>()
                .Where(r => r.FechaInicio <= fechaActual && r.FechaFinal >= fechaActual)
                .ToListAsync();
        }

        /// <summary>
        /// Verifica si existe superposición de fechas con los registros existentes.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio a verificar.</param>
        /// <param name="fechaFinal">Fecha final a verificar.</param>
        /// <returns>Verdadero si hay superposición; de lo contrario, falso.</returns>
        public async Task<bool> ExisteSuperposicionDeFechasAsync(DateTime fechaInicio, DateTime fechaFinal)
        {
            return await _context.Set<RepLegal>().AnyAsync(r =>
                (fechaInicio >= r.FechaInicio && fechaInicio <= r.FechaFinal) ||
                (fechaFinal >= r.FechaInicio && fechaFinal <= r.FechaFinal) ||
                (fechaInicio <= r.FechaInicio && fechaFinal >= r.FechaFinal));
        }

        /// <summary>
        /// Obtiene el historial de representantes legales ordenado por fecha de inicio.
        /// </summary>
        /// <returns>Lista del historial de representantes legales.</returns>
        public async Task<IEnumerable<RepLegal>> ObtenerHistorialRepresentantesAsync()
        {
            return await _context.Set<RepLegal>()
                .OrderByDescending(r => r.FechaInicio)
                .ToListAsync();
        }

        /// <summary>
        /// Valida la posibilidad de crear un representante legal como empleado o contratista.
        /// </summary>
        /// <param name="cuentaUsuarioId">ID del usuario asociado.</param>
        /// <param name="tipoContrato">Tipo de contrato del representante legal.</param>
        /// <returns>Verdadero si la validación es exitosa; lanza una excepción si no lo es.</returns>
        public async Task<bool> ValidarCreacionRepLegalAsync(Guid cuentaUsuarioId, TipoContrato tipoContrato)
        {
            // Verifica si el usuario ya está registrado como representante legal.
            var existeRepLegal = await _context.RepLegals.AnyAsync(rl => rl.CuentaUsuarioId == cuentaUsuarioId);
            if (existeRepLegal)
            {
                throw new InvalidOperationException($"El usuario con ID {cuentaUsuarioId} ya está registrado como representante legal.");
            }

            // Verifica si el usuario existe.
            var cuentaUsuario = await _context.CuentasUsuarios
                .FirstOrDefaultAsync(cu => cu.Id == cuentaUsuarioId);

            if (cuentaUsuario == null)
            {
                throw new KeyNotFoundException($"No se encontró un usuario con ID {cuentaUsuarioId}.");
            }

            // Valida los datos tributarios si es contratista.
            if (tipoContrato == TipoContrato.PrestadorDeServicios)
            {
                if (string.IsNullOrEmpty(cuentaUsuario.RazonSocial) || !cuentaUsuario.TipoIdTrib.HasValue)
                {
                    throw new InvalidOperationException("Los contratistas deben tener RazonSocial y TipoIdTrib definidos.");
                }
            }

            // Verifica que el tipo de contrato coincida.
            if (cuentaUsuario.TipoContrato != tipoContrato)
            {
                throw new InvalidOperationException($"El tipo de contrato del usuario no coincide con el tipo especificado: {tipoContrato}.");
            }

            return true;
        }
    }
}
