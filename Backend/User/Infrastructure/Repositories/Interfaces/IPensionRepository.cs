using System.Threading.Tasks;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Infrastructure.Repositories.Interfaces
{
    public interface IPensionRepository : IGenericRepository<Pension>
    {
        // Métodos específicos de Pension
        Task<Pension> GetByGuidAsync(Guid Pension);
    }
}