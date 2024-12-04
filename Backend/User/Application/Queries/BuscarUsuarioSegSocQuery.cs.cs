using Microsoft.EntityFrameworkCore;
using PhAppUser.Application.DTOs;
using PhAppUser.Domain.Enums;
using PhAppUser.Infrastructure.Context;

namespace PhAppUser.Application.Queries
{
    /// <summary>
    /// Servicio para buscar usuarios con información de salud y pensión.
    /// </summary>
    public class BuscarUsuarioSegSocQueryService
    {
        private readonly PhAppUserDbContext _context;

        /// <summary>
        /// Constructor para inicializar el contexto de base de datos.
        /// </summary>
        /// <param name="context">Contexto de base de datos de PhAppUser.</param>
        public BuscarUsuarioSegSocQueryService(PhAppUserDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Realiza una búsqueda de usuarios utilizando criterios como nombre, apellido o identificación.
        /// </summary>
        /// <param name="nombre">Fragmento del nombre del usuario.</param>
        /// <param name="apellido">Fragmento del apellido del usuario.</param>
        /// <param name="identificacion">Fragmento de la identificación del usuario.</param>
        /// <returns>Lista de usuarios con información de salud y pensión.</returns>
        /// <exception cref="ArgumentException">Se lanza si los criterios de búsqueda no son válidos.</exception>
        public async Task<List<UsuarioSegSocQDto>> BuscarUsuarioSegSocQuery(string? nombre, string? apellido, string? identificacion)
        {
            // Validar los criterios de entrada
            ValidarEntradas(nombre, apellido, identificacion);

            // Realizar la consulta
            return await _context.CuentasUsuarios
                .Include(cu => cu.Salud)
                .Include(cu => cu.Pension)
                .Where(cu =>
                    (string.IsNullOrEmpty(nombre) || cu.NombresCompletos.Contains(nombre)) &&
                    (string.IsNullOrEmpty(apellido) || cu.ApellidosCompletos.Contains(apellido)) &&
                    (string.IsNullOrEmpty(identificacion) || cu.Identificacion.Contains(identificacion))
                )
                .Select(cu => new UsuarioSegSocQDto
                {
                    Id = cu.Id,
                    NombresCompletos = $"{cu.NombresCompletos} {cu.ApellidosCompletos}",
                    Salud = new SaludDto
                    {
                        Numero = cu.Salud != null ? cu.Salud.Numero : "No asignado",
                        EsActivo = cu.Salud != null && cu.Salud.EsActivo
                    },
                    Pension = new PensionDto
                    {
                        Numero = cu.Pension != null ? cu.Pension.Numero : "No asignado",
                        EsActivo = cu.Pension != null && cu.Pension.EsActivo
                    }
                })
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Valida los criterios de búsqueda para asegurar que sean válidos.
        /// </summary>
        /// <param name="nombre">Fragmento del nombre del usuario.</param>
        /// <param name="apellido">Fragmento del apellido del usuario.</param>
        /// <param name="identificacion">Fragmento de la identificación del usuario.</param>
        /// <exception cref="ArgumentException">Se lanza si no se proporcionan criterios válidos.</exception>
        private void ValidarEntradas(string? nombre, string? apellido, string? identificacion)
        {
            if (string.IsNullOrEmpty(nombre) && string.IsNullOrEmpty(apellido) && string.IsNullOrEmpty(identificacion))
            {
                throw new ArgumentException("Debe proporcionar al menos un criterio de búsqueda.");
            }

            if (!string.IsNullOrEmpty(nombre) && nombre.Length < 3)
            {
                throw new ArgumentException("El nombre debe tener al menos 3 caracteres.");
            }

            if (!string.IsNullOrEmpty(apellido) && apellido.Length < 3)
            {
                throw new ArgumentException("El apellido debe tener al menos 3 caracteres.");
            }

            if (!string.IsNullOrEmpty(identificacion) && (identificacion.Length < 3 || !identificacion.All(char.IsDigit)))
            {
                throw new ArgumentException("La identificación debe tener al menos 3 caracteres y contener solo dígitos.");
            }
        }
    }
}
