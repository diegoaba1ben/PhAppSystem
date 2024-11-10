using System.Threading.Tasks;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Infrastructure.Repositories.Interfaces

{
    public interface ISaludRepository : IGenericRepository<Salud>
    {
        // Métodos específicos de Salud
        Task<Salud?> GetByGuidAsync(Guid SaludId);
    }
}