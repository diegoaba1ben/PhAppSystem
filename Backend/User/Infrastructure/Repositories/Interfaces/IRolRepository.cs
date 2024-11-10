using System;
using System.Threading.Tasks;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz para manejar operaciones específicas de Rol.
    /// </summary>
    public interface IRolRepository : IGenericRepository<Rol>
    {
        Task<Rol?> BuscarPorNombreAsync(string nombre);
        Task<bool> TienePermisoAsync(Guid rolId, Guid permisoId);
    }
}
