using System.Threading.Tasks;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Infrastructure.Repositories.Interfaces
{
    public interface IPerfilRepository : IGenericRepository<Perfil>
    {
        // Métodos específicos Perfil
        Task<Perfil?> GetByGuidAsync(Guid PerfilId);
    }
}