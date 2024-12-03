using PhAppUser.Domain.Entities;
using PhAppUser.Domain.Enums;

namespace PhAppUser.Domain.Validators
{
    /// <summary>
    /// Validador para manejar la lógica específica del dominio de usuarios para la entidad CuentaUsuarios
    /// </summary>
    public static class userLogicValidator
    {
        /// <summary>
        /// Valida que la afiliación parcial tenga un plzao definido en días.
        /// </summary>
        public static bool ValidarAfiliacionParcial(CuentaUsuario cuentaUsuario)
        {
            return cuentaUsuario.Afiliacion != Afiliacion.Parcial || (cuentaUsuario.DiasPendientes.HasValue && cuentaUsuario.DiasPendientes > 0);

        }
        /// <summary>
        /// Valida que un usuario no puede estar activo y bloqueado a la vez.
        /// </summary>
        public static bool ValidarActivoBloqueado(CuentaUsuario cuentaUsuario)
        {
            return !(cuentaUsuario.EsActivo && cuentaUsuario.Bloqueado);
        }
        /// <summary>
        /// Valida que el usuario esté bloqueado si excede el número máximo de intentos.
        /// </summary>
        public static bool ValidarIntentosBloqueo(CuentaUsuario cuentaUsuario)
        {
            return cuentaUsuario.Intento <= 2 || cuentaUsuario.Bloqueado;
        }
        /// <summary>
        /// Valida que los perfiles tengan áreas asociadas.
        /// </summary>
        public static bool ValidarPerfilesAreas(CuentaUsuario cuentaUsuario)
        {
            return cuentaUsuario.Perfiles.All(perfil => perfil.Area != null && !string.IsNullOrEmpty(perfil.Area.Nombre));
        }
        /// <summary>
        /// Valida que los perfiles tengan roles asociandos antes de avctivar al usuario
        /// </summary>
        public static bool ValidarPerfilesRoles(CuentaUsuario cuentaUsuario)
        {
            return cuentaUsuario.Perfiles.All(perfil => perfil.Roles.Any());
        }
        /// <summary>
        /// Valida la unicidad  de la combinación TipoId e identificación en la base de datos
        /// </summary>
        /// <param name="TipoId">Tipo de identificación.</param>
        /// <param name="identificación">Número de identificación.</param>
        /// <param name="existeEnDb">Función para verificar la existencia en la base de datos.</param>
        public static async Task<bool> ValidarUnicidadIdentificacionAsync(
            string tipoId,
            string identificacion,
    Func<string, string, Task<bool>> existeEnDb)
        {
            if (string.IsNullOrEmpty(tipoId) || string.IsNullOrEmpty(identificacion))
            {
                throw new ArgumentException("TipoId e Identificación no pueden estar vacíos.");
            }

            // Verificar unicidad usando la función proporcionada
            return !await existeEnDb(tipoId, identificacion);
        }
    }
}