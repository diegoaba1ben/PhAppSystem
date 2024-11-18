using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhAppUser.Infrastructure.Context;
using PhAppUser.Application.DTOs;

namespace PhAppUser.Application.Queries
{
    public class CuentaUsuarioQueries
    {
        private readonly PhAppUserDbContext _context;

        public CuentaUsuarioQueries(PhAppUserDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene la lista de usuarios con los perfiles asociados.
        /// </summary>
        /// <returns>Lista de usuarios con perfiles.</returns>
        public async Task<List<CuentaUsuarioDto>> ObtenerUsuariosConPerfilesAsync()
        {
            return await _context.CuentasUsuarios
                .Include(cu => cu.Perfiles)
                .ThenInclude(p => p.Roles) // Incluir roles asociados a perfiles
                .Select(cu => new CuentaUsuarioDto
                {
                    Id = cu.Id,
                    NombresCompletos = cu.NombresCompletos,
                    ApellidosCompletos = cu.ApellidosCompletos,
                    TipoId = cu.TipoId,
                    Identificacion = cu.Identificacion,
                    Direccion = cu.Direccion,
                    Ciudad = cu.Ciudad,
                    Telefono = cu.Telefono,
                    Email = cu.Email,
                    EsActivo = cu.EsActivo,
                    FechaRegistro = cu.FechaRegistro,
                    FechaInactivacion = cu.FechaInactivacion,
                    NombreUsuario = cu.NombreUsuario,
                    FechaUltimoLogin = cu.FechaUltimoLogin,
                    TipoCuenta = cu.TipoCuenta,
                    TarjProf = cu.TarjProf,
                    TipoContrato = cu.TipoContrato,
                    SujetoRetencion = cu.SujetoRetencion,
                    TipoIdTrib = cu.TipoIdTrib,
                    RazonSocial = cu.RazonSocial,
                    Afiliacion = cu.Afiliacion,
                    DiasPendientes = cu.DiasPendientes,
                    FechaCreacion = cu.FechaCreacion,
                    // Añadir perfiles y roles asociados
                    Perfiles = cu.Perfiles.Select(p => new PerfilDto
                    {
                        Id = p.Id,
                        Area =p.Area.Nombre,
                        Roles = p.Roles.Select(r => r.Nombre).ToList(),
                    }).ToList()
                })
                .ToListAsync();
        }
    }
}
