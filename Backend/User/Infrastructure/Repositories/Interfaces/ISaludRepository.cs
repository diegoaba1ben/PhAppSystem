using System.Threading.Tasks;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz para manejar operaciones específicas de Salud.
    /// </summary>
    public interface ISaludRepository : IGenericRepository<Salud>
    {
        // Verifica si ya existe un registro con el mismo número de afiliación
        Task<bool> ExisteNumeroAsync(string numero);
    }
}
