using PhAppUser.Application.DTOs;
using PhAppUser.Domain.Entities;
using PhAppUser.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhAppUser.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz específica para operaciones relacionadas con CuentaUsuario.
    /// </summary>
    public interface ICuentaUsuarioRepository : IGenericRepository<CuentaUsuario>
    {
        Task<IEnumerable<CuentaUsuario>> SearchUsuarioAsync(string searchTerm);
        Task<IEnumerable<CuentaUsuario>> GetUsuariosActivosAsync();
        Task<IEnumerable<CuentaUsuario>> GetUsuarioByPerfilAsync(Guid perfilId);
        Task<IEnumerable<CuentaUsuario>> GetUsuarioByRolesAsync();
        Task<IEnumerable<CuentaUsuario>> GetUsuariosByTipoContratoAsync(TipoContrato tipoContrato);
        Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario);
        Task<IEnumerable<CuentaUsuario>> GetUsuariosByUltimoLoginAsync(DateTime date);
        Task<IEnumerable<CuentaUsuario>> GetUsuariosAfiliacionPendienteAsync();
        Task InactivarUsuarioAsync(Guid usuarioId, DateTime fechaInactivacion);
        Task<IEnumerable<UsuarioInactivoDto>> GetUsuariosInactivosAsync();
        Task<bool> ExisteIdentificacionAsync(string identificacion);
    }
}

