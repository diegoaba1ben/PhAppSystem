using System;
using System.ComponentModel.DataAnnotations;
using PhAppUser.Domain.Enums;

namespace PhAppUser.Domain.Entities
{
    /// <summary>
    /// Representa la afiliación a una entidad de salud.
    /// </summary>
    public class Salud
    {
        [Key]
        public Guid Id { get; internal set; }

        public string Numero { get; internal set; } = string.Empty; // Número de afiliación

        public string RazonSocialSalud { get; internal set; } = string.Empty; // Nombre de la entidad de salud

        public TipoIdTrib TipoIdTrib { get; internal set; } // Identificación tributaria de la entidad

        public string IdentificacionTributaria { get; internal set; } = string.Empty; // NIT o RUT de la entidad

        public bool EsActivo { get; internal set; } = true; // Estado inicial predeterminado

        /// <summary>
        /// Actualiza el estado de afiliación a salud.
        /// </summary>
        public void ActualizarEstado(bool estado)
        {
            EsActivo = estado;
        }

        // Relación con CuentaUsuario
        public Guid CuentaUsuarioId { get; internal set; } // FK hacia CuentaUsuario
        public CuentaUsuario CuentaUsuario { get; internal set; } = null!;

        // Constructor privado para uso exclusivo del Builder
        internal Salud() { }

        // Builder interno para crear instancias de Salud
        public class Builder
        {
            private readonly Salud _salud = new Salud();

            public Builder GenerarNuevoId()
            {
                _salud.Id = Guid.NewGuid();
                return this;
            }

            public Builder ConNumero(string numero)
            {
                if (string.IsNullOrWhiteSpace(numero))
                    throw new ArgumentException("El número de afiliación no puede estar vacío.");
                _salud.Numero = numero;
                return this;
            }

            public Builder ConRazonSocialSalud(string razonSocialSalud)
            {
                if (string.IsNullOrWhiteSpace(razonSocialSalud))
                    throw new ArgumentException("La razón social no puede estar vacía.");
                _salud.RazonSocialSalud = razonSocialSalud;
                return this;
            }

            public Builder ConTipoIdTrib(TipoIdTrib tipoIdTrib)
            {
                _salud.TipoIdTrib = tipoIdTrib;
                return this;
            }

            public Builder ConIdentificacionTributaria(string identificacion)
            {
                if (string.IsNullOrWhiteSpace(identificacion))
                    throw new ArgumentException("La identificación tributaria no puede estar vacía.");
                _salud.IdentificacionTributaria = identificacion;
                return this;
            }

            public Builder ConCuentaUsuarioId(Guid cuentaUsuarioId)
            {
                _salud.CuentaUsuarioId = cuentaUsuarioId;
                return this;
            }

            /// <summary>
            /// Inicializa el estado de afiliación (activo/inactivo).
            /// </summary>
            /// <param name="estado">Estado de la afiliación, predeterminado a activo (true).</param>
            /// <returns>El builder con el estado actualizado.</returns>
            public Builder InicializarEsActivo(bool estado = true)
            {
                _salud.EsActivo = estado;
                return this;
            }

            public Salud Build()
            {
                if (string.IsNullOrWhiteSpace(_salud.Numero) || string.IsNullOrWhiteSpace(_salud.RazonSocialSalud))
                    throw new InvalidOperationException("Los datos básicos de la afiliación a salud deben completarse.");

                return _salud;
            }
        }
    }
}
