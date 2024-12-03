using PhAppUser.Domain.Entities;
using PhAppUser.Domain.Enums;

namespace PhAppUser.Domain.Validators
{
    public static class CuentaUsuarioCustomValidations
    {
        /// <summary>
        /// Valida que el campo SujetoRetencion sea requerido solo para empleados.
        /// </summary>
        public static bool ValidarSujetoRetencion(CuentaUsuario usuario)
        {
            return usuario.TipoContrato != TipoContrato.Empleado || usuario.SujetoRetencion.HasValue;
        }

        /// <summary>
        /// Valida que los prestadores de servicios tengan Razón Social y Tipo de Identificación Tributaria.
        /// </summary>
        public static bool ValidarRazonSocIdTrib(CuentaUsuario usuario)
        {
            return usuario.TipoContrato != TipoContrato.PrestadorDeServicios ||
                   (!string.IsNullOrEmpty(usuario.RazonSocial) && usuario.TipoIdTrib.HasValue);
        }

        /// <summary>
        /// Valida las reglas de afiliación para el usuario.
        /// </summary>
        public static bool ValidarAfiliacion(CuentaUsuario usuario)
        {
            if (usuario.Afiliacion == Afiliacion.Parcial)
            {
                return usuario.DiasPendientes.HasValue && usuario.DiasPendientes > 0;
            }

            if (usuario.Afiliacion == Afiliacion.Completa)
            {
                return usuario.Salud != null && usuario.Pension != null;
            }

            return true;
        }

        /// <summary>
        /// Valida que la FechaInactivacion no sea anterior a la FechaRegistro.
        /// </summary>
        public static bool ValidarFechaInactivacion(CuentaUsuario usuario)
        {
            if (usuario.FechaInactivacion.HasValue)
            {
                return usuario.FechaInactivacion.Value >= usuario.FechaRegistro;
            }
            return true;
        }

        /// <summary>
        /// Valida que el usuario no esté activo si está bloqueado.
        /// </summary>
        public static bool ValidarEstadoUsuario(CuentaUsuario usuario)
        {
            return !(usuario.EsActivo && usuario.Bloqueado);
        }

        /// <summary>
        /// Valida que los perfiles asociados al usuario tengan un área asignada.
        /// </summary>
        public static bool ValidarPerfilesArea(CuentaUsuario usuario)
        {
            return usuario.Perfiles.All(perfil => perfil.Area != null);
        }

        /// <summary>
        /// Valida que los perfiles asociados al usuario contengan roles.
        /// </summary>
        public static bool ValidarPerfilesRoles(CuentaUsuario usuario)
        {
            return usuario.Perfiles.All(perfil => perfil.Roles.Any());
        }

        /// <summary>
        /// Verifica que la combinación de TipoId e Identificación sea única.
        /// </summary>
        public static async Task<bool> ValidarUnicidadIdentificacionAsync(
            CuentaUsuario usuario, Func<string, TipoId, Task<bool>> existeIdentificacionAsync)
        {
            return !await existeIdentificacionAsync(usuario.Identificacion, usuario.TipoId);
        }

        /// <summary>
        /// Valida que el usuario tenga afiliación completa, ambos campos (Salud y Pensión) estén presentes.
        /// </summary>
        public static bool ValidarSaludYPension(CuentaUsuario usuario)
        {
            return usuario.Afiliacion != Afiliacion.Completa || (usuario.Salud != null && usuario.Pension != null);
        }
    }
}

