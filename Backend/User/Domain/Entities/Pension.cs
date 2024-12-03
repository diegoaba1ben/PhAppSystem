using PhAppUser.Domain.Enums;

namespace PhAppUser.Domain.Entities
{
    /// <summary>
    /// Representa la afiliación a una entidad de pensión.
    /// </summary>
    public class Pension
    {

        public Guid Id { get; internal set; }
        public string Numero { get; internal set; } = string.Empty; // Número de afiliación
        public string RazonSocialPension { get; internal set; } = string.Empty; // Nombre de la entidad de pensión
        public TipoIdTrib TipoIdTrib { get; internal set; } // Identificación tributaria de la entidad

        public string IdentificacionTributaria { get; internal set; } = string.Empty; // NIT o RUT de la entidad
        public bool EsActivoTrib { get; internal set;} = true; // Estado predeterminado del atributo.

        // Relación con CuentaUsuario
        public Guid CuentaUsuarioId { get; internal set; } // FK hacia CuentaUsuario
        public CuentaUsuario CuentaUsuario { get; internal set; } = null!;
        public bool EsActivo { get; internal set; } = true; // Indica si la afiliación está activa

        /// <summary>
        /// Actualiza el estado de la afiliación a Pensión
        /// </summary>
        public void ActualizarEstado(bool estado) => EsActivo = estado;

        // Constructor privado para uso exclusivo del Builder
        internal Pension() { }

        // Builder interno para crear instancias de Pension
        public class Builder
        {
            private readonly Pension _pension = new Pension();

            public Builder GenerarNuevoId()
            {
                _pension.Id = Guid.NewGuid();
                return this;
            }

            public Builder ConNumero(string numero)
            {
                if (string.IsNullOrWhiteSpace(numero))
                    throw new ArgumentException("El número de afiliación no puede estar vacío.");
                _pension.Numero = numero;
                return this;
            }

            public Builder ConRazonSocialPension(string razonSocialPension)
            {
                if (string.IsNullOrWhiteSpace(razonSocialPension))
                    throw new ArgumentException("La razón social no puede estar vacía.");
                _pension.RazonSocialPension = razonSocialPension;
                return this;
            }

            public Builder ConTipoIdTrib(TipoIdTrib tipoIdTrib)
            {
                _pension.TipoIdTrib = tipoIdTrib;
                return this;
            }

            public Builder ConIdentificacionTributaria(string identificacion)
            {
                if (string.IsNullOrWhiteSpace(identificacion))
                    throw new ArgumentException("La identificación tributaria no puede estar vacía.");
                _pension.IdentificacionTributaria = identificacion;
                return this;
            }

            public Builder ConCuentaUsuarioId(Guid cuentaUsuarioId)
            {
                _pension.CuentaUsuarioId = cuentaUsuarioId;
                return this;
            }
            /// <summary>
            /// Inicializa el estado de afiliación (activo/inactivo).
            /// </summary>
            /// <param name="estado">Estado de la afiliación, predeterminado a activo (true).</param>
            /// <returns>El builder con el estado actualizado</returns>
            public Builder InicializarEsActivo(bool estado = true)
            {
                _pension.EsActivo = estado;
                return this;
            }

            public Pension Build()
            {
                if (string.IsNullOrWhiteSpace(_pension.Numero) || string.IsNullOrWhiteSpace(_pension.RazonSocialPension))
                    throw new InvalidOperationException("Los datos básicos de la afiliación a pensión deben completarse.");
                return _pension;
            }
        }
    }
}
