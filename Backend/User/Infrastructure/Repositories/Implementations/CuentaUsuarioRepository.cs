using Microsoft.EntityFrameworkCore;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Repositories.Interfaces;
using PhAppUser.Infrastructure.Context;
using PhAppUser.Domain.Enums;
using PhAppUser.Infrastructure.Helpers;
using PhAppUser.Application.DTOs;

namespace PhAppUser.Infrastructure.Repositories.Implementations
{
    public class CuentaUsuarioRepository : GenericRepository<CuentaUsuario>, ICuentaUsuarioRepository
    {
        private readonly ILogger<CuentaUsuarioRepository> _logger;

        public CuentaUsuarioRepository(PhAppUserDbContext context, ILogger<CuentaUsuarioRepository> logger) : base(context, logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<CuentaUsuario>> SearchUsuarioAsync(string searchTerm)
        {
            return await ExceptionHandler.HandleAsync(async () =>
            {
                return await _context.CuentasUsuarios
                    .Where(cu => cu.NombresCompletos.Contains(searchTerm) ||
                                 cu.ApellidosCompletos.Contains(searchTerm) ||
                                 cu.Identificacion.Contains(searchTerm))
                    .ToListAsync();
            },
            _logger,
            $"Error al realizar la búsqueda de usuarios con el término {searchTerm}");
        }

        public async Task<IEnumerable<CuentaUsuario>> GetUsuariosActivosAsync()
        {
            return await ExceptionHandler.HandleAsync(async () =>
            {
                return await _context.CuentasUsuarios
                    .Where(cu => cu.EsActivo)
                    .ToListAsync();
            },
            _logger,
            "Error al obtener usuarios activos");
        }

        public async Task<IEnumerable<CuentaUsuario>> GetUsuarioByPerfilAsync(Guid perfilId)
        {
            return await ExceptionHandler.HandleAsync(async () =>
            {
                return await _context.CuentasUsuarios
                    .Where(cu => cu.Perfiles.Any(p => p.Id == perfilId))
                    .ToListAsync();
            },
            _logger,
            $"Error al obtener usuarios por perfil con ID {perfilId}");
        }

        public async Task<IEnumerable<CuentaUsuario>> GetUsuarioByRolesAsync()
        {
            return await ExceptionHandler.HandleAsync(async () =>
            {
                return await _context.CuentasUsuarios
                    .Include(cu => cu.Perfiles)
                        .ThenInclude(perfil => perfil.Roles)
                    .Where(cu => cu.Perfiles.Any(p => p.Roles.Any()))
                    .ToListAsync();
            },
            _logger,
            "Error al obtener usuarios con perfiles y roles");
        }

        public async Task<IEnumerable<CuentaUsuario>> GetUsuariosByTipoContratoAsync(TipoContrato tipoContrato)
        {
            return await ExceptionHandler.HandleAsync(async () =>
            {
                return await _context.CuentasUsuarios
                    .Where(cu => cu.TipoContrato == tipoContrato)
                    .ToListAsync();
            },
            _logger,
            $"Error al obtener usuarios por tipo de contrato: {tipoContrato}");
        }

        public async Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new ArgumentException("El nombre de usuario no puede ser nulo o vacío");

            return await ExceptionHandler.HandleAsync(async () =>
            {
                return await _context.CuentasUsuarios
                    .AnyAsync(cu => cu.NombreUsuario == nombreUsuario);
            },
            _logger,
            $"Error al validar si el nombre de usuario existe: {nombreUsuario}");
        }

        public async Task<IEnumerable<CuentaUsuario>> GetUsuariosByUltimoLoginAsync(DateTime date)
        {
            return await ExceptionHandler.HandleAsync(async () =>
            {
                return await _context.CuentasUsuarios
                    .Where(cu => cu.FechaUltimoLogin >= date)
                    .ToListAsync();
            },
            _logger,
            $"Error al obtener usuarios por última fecha de inicio de sesión desde {date}");
        }

        public async Task<IEnumerable<CuentaUsuario>> GetUsuarioAfiliacionCompletaAsync()
        {
            return await ExceptionHandler.HandleAsync(async () =>
            {
                return await _context.CuentasUsuarios
                    .Include(cu => cu.Salud)
                    .Include(cu => cu.Pension)
                    .Where(cu => cu.Salud != null && cu.Pension != null)
                    .ToListAsync();
            },
            _logger, "Error al obtener usuarios con afiliación completa.");
        }

        public async Task<IEnumerable<CuentaUsuario>> GetUsuariosAfiliacionPendienteAsync()
        {
            return await ExceptionHandler.HandleAsync(async () =>
            {
                return await _context.CuentasUsuarios
                    .Include(cu => cu.Salud)
                    .Include(cu => cu.Pension)
                    .Where(cu => cu.Afiliacion == Afiliacion.Parcial &&
                    (cu.Salud == null || cu.Pension == null) &&
                    cu.DiasPendientes.HasValue && cu.DiasPendientes > 0)
                    .ToListAsync();
            },
            _logger,
            "Error al obtener usuarios con afiliaciones pendientes");
        }

        public async Task InactivarUsuarioAsync(Guid usuarioId, DateTime fechaInactivacion)
        {
            await ExceptionHandler.HandleAsync(async () =>
            {
                var usuario = await _context.CuentasUsuarios
                    .Include(cu => cu.Perfiles)
                        .ThenInclude(p => p.Roles)
                    .FirstOrDefaultAsync(cu => cu.Id == usuarioId);

                if (usuario == null)
                    throw new KeyNotFoundException($"No se encontró el usuario con ID {usuarioId}");

                usuario.EsActivo = false;
                usuario.FechaInactivacion = fechaInactivacion;

                foreach (var perfil in usuario.Perfiles)
                {
                    perfil.Roles.Clear();
                }
                usuario.Perfiles.Clear();

                _context.CuentasUsuarios.Update(usuario);
                await _context.SaveChangesAsync();
            },
            _logger,
            $"Error al intentar inactivar el usuario con ID {usuarioId}");
        }

        public async Task<IEnumerable<UsuarioInactivoDto>> GetUsuariosInactivosAsync()
        {
            return await ExceptionHandler.HandleAsync(async () =>
            {
                return await _context.CuentasUsuarios
                    .Where(cu => !cu.EsActivo) // Usuarios inactivos
                    .Select(cu => new UsuarioInactivoDto
                    {
                        NombresCompletos = cu.NombresCompletos,
                        ApellidosCompletos = cu.ApellidosCompletos,
                        Identificacion = cu.Identificacion,
                        FechaInactivacion = cu.FechaInactivacion ?? DateTime.MinValue,
                        MotivoInactivacion = cu.FechaInactivacion.HasValue
                            ? "Automática por afiliación"
                            : "Inactivación manual",
                        NombreUsuario = cu.NombreUsuario
                    })
                    .ToListAsync();
            }, _logger, nameof(GetUsuariosInactivosAsync));
        }

        public async Task DesbloqUsuXAfiliacionAsync(Guid usuarioId)
        {
            {
                await ExceptionHandler.HandleAsync(async () =>
                {
                    var usuario = await _context.CuentasUsuarios
                        .Include(cu => cu.Salud)
                        .Include(cu =>cu.Pension)
                        .FirstOrDefaultAsync(cu => cu.Id == usuarioId);
                    if(usuario == null)
                    throw new KeyNotFoundException("No se encontró el usuario con ID {usuarioId}");
                    if(usuario.Bloqueado && usuario.Salud != null && usuario.Pension != null)
                    {
                        usuario.Bloqueado = false;
                        usuario.Intento = 0;
                        _context.CuentasUsuarios.Update(usuario);
                        await _context.SaveChangesAsync();
                    }
                },
                _logger, $"Error al intentar desbloquear el usuario con ID{usuarioId}");  
            }   
        }
        public async Task<bool> ExisteIdentificacionAsync(string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion))
                throw new ArgumentException("La identificación no puede ser nula o vacía", nameof(identificacion));

            return await ExceptionHandler.HandleAsync(async () =>
            {
                return await _context.CuentasUsuarios
                    .AnyAsync(cu => cu.Identificacion == identificacion);
            },
            _logger,
            $"Error al validar si la identificación ya existe: {identificacion}");
        }
    }    
}
