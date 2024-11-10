using System.Threading.Tasks;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Infrastructure.Repositories.Interfaces
{
    public interface IPermisoRepository : IGenericRepository<Permiso>
    {
        // Métodos específicos de Permiso
        Task<Permiso?> GetByGuidAsync(Guid PermisoId);
    }
}