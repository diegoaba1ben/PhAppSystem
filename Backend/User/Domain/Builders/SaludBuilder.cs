using System;
using PhAppUser.Domain.Entities;
using PhAppUser.Domain.Enums;
using PhAppUser.Domain.Validators;

namespace PhAppUser.Domain.Builders
{
    /// <summary>
    /// Builder para la creación de instancias de Salud.
    /// </summary>
    public class SaludBuilder
    {
        private readonly Salud _salud;

        public SaludBuilder()
        {
            _salud = new Salud();
        }

        #region Métodos de Configuración

        public SaludBuilder ConCuentaUsuarioId(Guid cuentaUsuarioId)
        {
            _salud.CuentaUsuarioId = cuentaUsuarioId;
            return this;
        }

        public SaludBuilder ConNumeroAfiliacion(string numeroAfiliacion)
        {
            if (string.IsNullOrWhiteSpace(numeroAfiliacion))
                throw new ArgumentException("El número de afiliación no puede ser nulo o vacío.");

            _salud.Numero = numeroAfiliacion;
            return this;
        }

        public SaludBuilder ConTipoIdTrib(TipoIdTrib tipoIdTrib)
        {
            _salud.TipoIdTrib = tipoIdTrib;
            return this;
        }

        public SaludBuilder ConRazonSocial(string razonSocial)
        {
            if (string.IsNullOrWhiteSpace(razonSocial))
                throw new ArgumentException("La razón social no puede ser nula o vacía.");

            _salud.RazonSocialSalud = razonSocial;
            return this;
        }


        #endregion

        #region Validaciones y Construcción

        public Salud Build()
        {

            if (_salud.TipoIdTrib == TipoIdTrib.NoAplica)
                throw new InvalidOperationException("El tipo de identificación tributaria debe ser válido para entidades de salud.");

            // Devuelve la instancia construida
            return _salud;
        }

        #endregion
    }
}



