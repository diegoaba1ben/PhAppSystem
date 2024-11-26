using System;
using System.Threading.Tasks;
using PhAppUser.Domain.Entities;
using PhAppUser.Infrastructure.Repositories.Interfaces;

namespace PhAppUser.Application.Services.Validation
{
    /// <summary>
    /// Servicio para realizar validaciones que requieren acceso a la base de datos.
    /// </summary>
    public class DatabaseValidationService
    {
        private readonly ICuentaUsuarioRepository _cuentaUsuarioRepository;

        public DatabaseValidationService(ICuentaUsuarioRepository cuentaUsuarioRepository)
        {
            _cuentaUsuarioRepository = cuentaUsuarioRepository ?? throw new ArgumentNullException(nameof(cuentaUsuarioRepository));
        }

        /// <summary>
        /// Verifica si un usuario existe por su ID.
        /// </summary>
        public async Task<bool> ExisteUsuarioPorIdAsync(Guid id)
        {
            var usuario = await _cuentaUsuarioRepository.GetByIdAsync(id);
            return usuario != null;
        }

        /// <summary>
        /// Verifica si un nombre de usuario es único.
        /// </summary>
        public async Task<bool> NombreUsuarioEsUnicoAsync(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new ArgumentException("El nombre de usuario no puede estar vacío.", nameof(nombreUsuario));

            return !await _cuentaUsuarioRepository.ExisteNombreUsuarioAsync(nombreUsuario);
        }

        /// <summary>
        /// Verifica si un usuario está activo.
        /// </summary>
        public async Task<bool> EsUsuarioActivoAsync(Guid id)
        {
            var usuario = await _cuentaUsuarioRepository.GetByIdAsync(id);
            return usuario != null && usuario.EsActivo;
        }

        /// <summary>
        /// Verifica si una identificación es única
        /// </summary>
        public async Task<bool> IdentificacionEsUnicaAsync(string identificacion)
        {
            if (string.IsNullOrEmpty(identificacion))
                throw new ArgumentException("La identificación no puede estar vacía", nameof(identificacion));
            // Consulta al repositorio para verificar si la identificación existe.
            return !await _cuentaUsuarioRepository.ExisteIdentificacionAsync(identificacion);        
        }
    }
}

