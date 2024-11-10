using System.Threading.Tasks;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Infrastructure.Repositories.Interfaces
{
    public interface Irolrepository : IGenericRepository<Rol>
    {
        Task<Rol> GetByGuidAsync(Guid RolId);
    }
}