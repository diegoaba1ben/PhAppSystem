using System.Collections.Generic;
using System.Threading.Tasks;
using PhAppUser.Domain.Entities;
using PhAppUser.Domain.Enums;
using PhAppUser.Application.DTOs;

namespace PhAppUser.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Métodos aplicables a la entidad  CuentaUsuario.
    /// </summary>
    public interface ICuentaUsuarioRepository : IGenericRepository<CuentaUsuario>
    {     
       // Método de búsqueda avanzada para CuentaUsuario.
       Task<IEnumerable<CuentaUsuario>> SearchUsuarioAsync(string searchTerm);
       Task<IEnumerable<CuentaUsuario>> GetUsuariosActivosAsync();
       Task<IEnumerable<CuentaUsuario>> GetUsuarioByPerfilAsync(Guid perfilId);
       Task<IEnumerable<CuentaUsuario>> GetUsuarioByRolesAsync();
       Task<IEnumerable<CuentaUsuario>> GetUsuariosByTipoContratoAsync(TipoContrato tipoContrato);
       Task<bool> ExisteNombreUsuarioAsync(string NombreUsuario);
       Task<IEnumerable<CuentaUsuario>> GetUsuariosByUltimoLoginAsync(DateTime date);
       Task<IEnumerable<CuentaUsuario>> GetUsuariosAfiliacionPendienteAsync();

       // Método para inactivar un usuario
       Task InactivarUsuarioAsync(Guid CuentaUsuarioId, DateTime fechaInactivacion);
       Task<IEnumerable<UsuarioInactivoDto>> GetUsuariosInactivosAsync();
        Task<bool> ExisteIdentificacionAsync(string identificacion); 
    }
}
