using System.Threading.Tasks;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// interfaz para manejar las operaciones específicas de Pensión
    /// </summary>
    public interface IPensionRepository : IGenericRepository<Pension>
    {
        // Verifica si ya existe un registro con el mismo número de afiliación en pensión
        Task<bool> ExisteNumeroAsync(string numero);

    }
}