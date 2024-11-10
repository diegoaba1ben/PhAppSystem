using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz para manejar operaciones específicas de Perfil.
    /// </summary>
    public interface IPerfilRepository : IGenericRepository<Perfil>
    {
        // Métodos básicos de búsqueda y validación
        Task<Perfil?> BuscarPorIdAsync(Guid perfilId);
        Task<bool> ExistePerfilAsync(Guid perfilId);

        // Métodos de consultas relacionales
        Task<IEnumerable<Perfil>> ObtenerPerfilesConUsuariosAsync();
        Task<IEnumerable<Perfil>> ObtenerPerfilesPorAreaAsync(Guid areaId);
        Task<IEnumerable<Perfil>> ObtenerPerfilesConRolesAsync();

        // Métodos de consultas avanzadas
        Task<bool> TieneRolAsync(Guid perfilId, Guid rolId);
        Task<IEnumerable<Rol>> ObtenerRolesYPermisosDePerfilAsync(Guid perfilId);

        // Métodos de gestión avanzada
        Task ActualizarRolesAsync(Guid perfilId, ICollection<Rol> nuevosRoles);
        Task EliminarPerfilConRelacionesAsync(Guid perfilId);
    }
}
