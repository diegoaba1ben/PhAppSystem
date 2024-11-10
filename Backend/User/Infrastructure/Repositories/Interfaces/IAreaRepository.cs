using System.Threading.Tasks;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Infrastructure.Repositories.Interfaces
{
    public interface IAreaRepository : IGenericRepository<Area>
    {
        // Agregar métodos específicos de perfil
        Task<Area?> GetAreaByGuidAsync(Guid areaGuid);
    }
}
