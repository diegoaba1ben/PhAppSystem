using Microsoft.EntityFrameworkCore; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Repositories.Interfaces;
using PhAppUser.Infrastructure.Context;
using PhAppUser.Domain.Enums;

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
            return await _context.Set<CuentaUsuario>()
                .Where(cu => cu.NombresCompletos.Contains(searchTerm) ||
                             cu.ApellidosCompletos.Contains(searchTerm) ||
                             cu.Identificacion.Contains(searchTerm))
                .ToListAsync();
        }

        // Implementación para obtener los usuarios activos
        public async Task<IEnumerable<CuentaUsuario>> GetUsuariosActivosAsync()
        {
            return await _context.Set<CuentaUsuario>()
                .Where(cu => cu.EsActivo)
                .ToListAsync();
        }

        // Implementación para obtener usuarios por perfil
        public async Task<IEnumerable<CuentaUsuario>> GetUsuarioByPerfilAsync(Guid perfilId)
        {
            return await _context.Set<CuentaUsuario>()
                .Where(cu => cu.Perfiles.Any(p => p.Id == perfilId))
                .ToListAsync();
        }

        // Implementación para obtener usuarios con roles
        public async Task<IEnumerable<CuentaUsuario>> GetUsuarioByRolesAsync()
        {
            return await _context.Set<CuentaUsuario>()
                .Include(cu => cu.Perfiles) // Incluye los roles para cada usuario
                    .ThenInclude(perfil => perfil.Roles)
                .ToListAsync();
        }

        // Implementación para obtener usuarios por tipo de contrato
        public async Task<IEnumerable<CuentaUsuario>> GetUsuariosByTipoContratoAsync(TipoContrato tipoContrato)
        {
            return await _context.Set<CuentaUsuario>()
                .Where(cu => cu.TipoContrato == tipoContrato)
                .ToListAsync();
        }

        // Implementación para verificar si el nombre de usuario ya existe
        public async Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario)
        {
            return await _context.Set<CuentaUsuario>()
                .AnyAsync(cu => cu.NombreUsuario == nombreUsuario);
        }

        // Obtener usuarios por última fecha de inicio de sesión
        public async Task<IEnumerable<CuentaUsuario>> GetUsuariosByUltimoLoginAsync(DateTime date)
        {
            return await _context.Set<CuentaUsuario>()
                .Where(cu => cu.FechaUltimoLogin >= date)
                .ToListAsync();
        }

        // Obtener usuarios con afiliaciones pendientes
        public async Task<IEnumerable<CuentaUsuario>> GetUsuariosAfiliacionPendienteAsync()
        {
            return await _context.Set<CuentaUsuario>()
                .Where(cu => cu.Afiliacion == Afiliacion.Parcial || cu.DiasPendientes.HasValue && cu.DiasPendientes > 0) // Ajustar según propiedad correcta
                .ToListAsync();
        }
    }
}



