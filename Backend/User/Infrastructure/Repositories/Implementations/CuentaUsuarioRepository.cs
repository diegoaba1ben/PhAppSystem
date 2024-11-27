using Microsoft.EntityFrameworkCore;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Repositories.Interfaces;
using PhAppUser.Infrastructure.Context;
using PhAppUser.Domain.Enums;
using Serilog;
using PhAppUser.Application.DTOs;

namespace PhAppUser.Infrastructure.Repositories.Implementations
{
    public class CuentaUsuarioRepository : GenericRepository<CuentaUsuario>, ICuentaUsuarioRepository
    {
        public CuentaUsuarioRepository(PhAppUserDbContext context) : base(context)
        {
        }

        // Método de búsqueda avanzada
        public async Task<IEnumerable<CuentaUsuario>> SearchUsuarioAsync(string searchTerm)
        {
            try
            {
                return await _context.Set<CuentaUsuario>()
                .Where(cu => cu.NombresCompletos.Contains(searchTerm) ||
                             cu.ApellidosCompletos.Contains(searchTerm) ||
                             cu.Identificacion.Contains(searchTerm))
                .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al realizar la búsqueda de usuarios con el término {SearchTerm}", searchTerm);
                throw new Exception($"Error en SearhUsuarioAsync con términos {searchTerm}", ex);
            }

        }

        // Implementación para obtener los usuarios activos
        public async Task<IEnumerable<CuentaUsuario>> GetUsuariosActivosAsync()
        {
            try
            {
                return await _context.Set<CuentaUsuario>()
                .Where(cu => cu.EsActivo)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al obtener los usuarios activos {SearchTerm}");
                throw new Exception("Ocurrió un error al buscar usuarios activos", ex);
            }

        }

        // Implementación para obtener usuarios por perfil
        public async Task<IEnumerable<CuentaUsuario>> GetUsuarioByPerfilAsync(Guid perfilId)
        {
            try
            {
                return await _context.Set<CuentaUsuario>()
                .Where(cu => cu.Perfiles.Any(p => p.Id == perfilId))
                .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al intentar obteneer usuarios por perfil");
                throw new Exception("Ocurrión un error inesperado al buscar usuarios por perfil", ex);
            }

        }

        // Implementación para obtener usuarios con perfiles y roles
        public async Task<IEnumerable<CuentaUsuario>> GetUsuarioByRolesAsync()
        {
            try
            {
                return await _context.Set<CuentaUsuario>()
                    .Include(cu => cu.Perfiles) // Carga los perfiles asociados al usuario
                        .ThenInclude(perfil => perfil.Roles) // Carga los roles asociados a los perfiles
                    .Where(cu => cu.Perfiles.Any(p => p.Roles.Any())) // Filtra solo usuarios con roles
                    .ToListAsync(); // Convierte el resultado a lista
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al obtener usuarios con perfiles y roles");
                throw new Exception("Error al obtener usuarios con perfiles y roles", ex);
            }
        }
        // Implementación para obtener usuarios por tipo de contrato
        public async Task<IEnumerable<CuentaUsuario>> GetUsuariosByTipoContratoAsync(TipoContrato tipoContrato)
        {
            try
            {
                return await _context.Set<CuentaUsuario>()
                .Where(cu => cu.TipoContrato == tipoContrato)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error("obtener usuarios por tipo de contrato {cuentaUsuairo.TipoContrato}");
                throw new Exception("Ocurrió un error inesperado al buscar usuarios por tipo de contrato", ex);
            }

        }

        // Implementación para verificar si el nombre de usuario ya existe
        public async Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario)
        {
            if (string.IsNullOrEmpty(nombreUsuario))
                throw new ArgumentException("El nombre de usuario no puede ser nulo o vacío");
            try
            {
                return await _context.Set<CuentaUsuario>()
                .AnyAsync(cu => cu.NombreUsuario == nombreUsuario);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al validar si el nombre de usuario existe: {NombreUsuario}");
                throw new Exception("Ocurrió un error al validar la existencia del nombre de usuario {NombreUsuario}", ex);
            }

        }

        // Obtener usuarios por última fecha de inicio de sesión
        public async Task<IEnumerable<CuentaUsuario>> GetUsuariosByUltimoLoginAsync(DateTime date)
        {
            try
            {
                return await _context.Set<CuentaUsuario>()
                .Where(cu => cu.FechaUltimoLogin >= date)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al intentar obtener usuarios po su última fecha de inicio de sesión");
                throw new Exception("Ocurrió un error inesperado al buscar la última fecha de inicio de sesión de un usuario");
            }

        }

        // Obtener usuarios con afiliaciones pendientes
        public async Task<IEnumerable<CuentaUsuario>> GetUsuariosAfiliacionPendienteAsync()
        {
            try
            {
                return await _context.Set<CuentaUsuario>()
                .Where(cu => cu.Afiliacion == Afiliacion.Parcial || cu.DiasPendientes.HasValue && cu.DiasPendientes > 0) // Ajustar según propiedad correcta
                .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al obtener usuarios con afiliaciones pendientes");
                throw new Exception("Ocurrió un error inesperado al buscar usuarios con afiliaciones pendientes.", ex);
            }

        }
        // Método para inactivar a un usuario.
        public async Task InactivarUsuarioAsync(Guid usuarioId, DateTime fechaInactivacion)
        {
            try
            {
                var usuario = await _context.Set<CuentaUsuario>()
                    .Include(cu => cu.Perfiles) // Incluir relaciones necesarias
                    .ThenInclude(p => p.Roles)
                    .FirstOrDefaultAsync(cu => cu.Id == usuarioId);

                if (usuario == null)
                {
                    Log.Warning("Intento de inactivación fallido: Usuario con ID {UsuarioId} no encontrado.", usuarioId);
                    throw new KeyNotFoundException($"No se encontró el usuario con ID {usuarioId}");
                }

                // Actualizar propiedades del usuario
                usuario.EsActivo = false;
                usuario.FechaInactivacion = fechaInactivacion;

                // Revocar perfiles y permisos
                foreach (var perfil in usuario.Perfiles)
                {
                    perfil.Roles.Clear(); // Eliminar roles asociados al perfil
                }
                usuario.Perfiles.Clear(); // Eliminar perfiles asociados

                // Guardar cambios en la base de datos
                _context.Set<CuentaUsuario>().Update(usuario);
                await _context.SaveChangesAsync();

                // Registrar la acción en el log
                Log.Information("Usuario {UsuarioId} inactivado exitosamente el {FechaInactivacion}", usuarioId, fechaInactivacion);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al intentar inactivar el usuario con ID {UsuarioId}", usuarioId);
                throw new Exception("Ocurrió un error al intentar inactivar el usuario.", ex);
            }
        }
        // Búsqueda de usuarios inactivos
        public async Task<IEnumerable<UsuarioInactivoDto>> GetUsuariosInactivosAsync()
        {
            return await _context.Set<CuentaUsuario>()
                .Where(cu => !cu.EsActivo) // Usuarios inactivos
                .Select(cu => new UsuarioInactivoDto
                {
                    NombresCompletos = cu.NombresCompletos,
                    ApellidosCompletos = cu.ApellidosCompletos,
                    Identificacion = cu.Identificacion,
                    FechaInactivacion = cu.FechaInactivacion ?? DateTime.MinValue, // Default para evitar nulos
                    MotivoInactivacion = cu.FechaInactivacion.HasValue
                        ? "Automática por afiliación" // Personaliza esto según tu lógica
                        : "Inactivación manual",
                    NombreUsuario = cu.NombreUsuario
                })
                .ToListAsync();
        }
        public async Task<bool> ExisteIdentificacionAsync(string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion))
                throw new ArgumentException("La identificación no puede ser nula o vacia", nameof(identificacion));

            try
            {

                return await _context.CuentasUsuarios.AnyAsync(cu => cu.Identificacion == identificacion);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al validar si la identificación ya existe: {identificacion}", identificacion);
                throw new Exception($"Error al validar la existencia de la identificación {identificacion}", ex);
            }
        }
    }
}



