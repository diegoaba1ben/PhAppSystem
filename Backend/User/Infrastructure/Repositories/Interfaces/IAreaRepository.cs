using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz para manejar operaciones específicas de Area.
    /// </summary>
    public interface IAreaRepository : IGenericRepository<Area>
    {
        Task<Area?> BuscarPorNombreAsync(string nombre);
        Task<IEnumerable<Area>> ObtenerAreasConRolesAsync();
    }
}

