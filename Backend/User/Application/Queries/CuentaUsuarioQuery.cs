using Microsoft.EntityFrameworkCore;
using PhAppUser.Application.DTOs;
using PhAppUser.Domain.Enums;
using PhAppUser.Infrastructure.Context;

namespace PhAppUser.Application.Queries
{
    public class UsuarioQueryService
    {
        private readonly PhAppUserDbContext _context;

        public UsuarioQueryService(PhAppUserDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<UsuarioDatosBasicosQDto>> BuscarUsuariosPorCriteriosAsync(string? nombre, string? apellido, string? identificacion)
        {
            // Validamos las entradas
            ValidarEntradas(nombre, apellido, identificacion);

            return await _context.CuentasUsuarios
                .Where(cu =>
                    (string.IsNullOrEmpty(nombre) || cu.NombresCompletos.Contains(nombre)) &&
                    (string.IsNullOrEmpty(apellido) || cu.ApellidosCompletos.Contains(apellido)) &&
                    (string.IsNullOrEmpty(identificacion) || cu.Identificacion.Contains(identificacion))
                )
                .Select(cu => new UsuarioDatosBasicosQDto
                {
                    Id = cu.Id,
                    NombresCompletos = $"{cu.NombresCompletos} {cu.ApellidosCompletos}",
                    Direccion = cu.Direccion,
                    Telefono = cu.Telefono,
                    Email = cu.Email,
                    TipoContrato = cu.TipoContrato == TipoContrato.Empleado ? "Empleado" : "Contratista"
                })
                .AsNoTracking()
                .ToListAsync();
        }

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

            if (!string.IsNullOrEmpty(identificacion) && identificacion.Length < 3)
            {
                throw new ArgumentException("La identificación debe tener al menos 3 caracteres.");
            }
        }

    }
}
