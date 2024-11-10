using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interface para manipular el repositorio de RepLegal.
    /// </summary>
    public interface IRepLegalRepository : IGenericRepository<RepLegal>
    {
        // Verifica si ya existe un representante legal con el mismo número de radicación.
        Task<bool> ExisteCertLegalAsync(string certLegal);

        // Obtiene una lista de representantes legales activos en el sistema.
        Task<IEnumerable<RepLegal>> ObtenerRepresentantesActivosAsync();

        // Verifica si las fechas ingresadas se superponen con otros períodos de representación legal.
        Task<bool> ExisteSuperposicionDeFechasAsync(DateTime fechaInicio, DateTime fechaFinal);

        // Obtiene un historial de todos los representantes legales ordenados por fecha de inicio.
        Task<IEnumerable<RepLegal>> ObtenerHistorialRepresentantesAsync();
    }
}
