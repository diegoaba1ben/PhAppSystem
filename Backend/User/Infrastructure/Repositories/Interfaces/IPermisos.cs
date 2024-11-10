using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz para manejar operaciones específicas de Permiso.
    /// </summary>
    public interface IPermisoRepository : IGenericRepository<Permiso>
    {
        Task<Permiso?> BuscarPorCodigoAsync(string codigo);
        Task<bool> ExisteCodigoAsync(string codigo);
        Task<IEnumerable<Permiso>> ObtenerPermisosPorRolAsync(Guid rolId);
    }
}
