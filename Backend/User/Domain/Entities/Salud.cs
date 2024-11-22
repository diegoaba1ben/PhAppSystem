using System;
using System.ComponentModel.DataAnnotations;
using PhAppUser.Domain.Enums;

namespace PhAppUser.Domain.Entities
{
    /// <summary>
    /// Representa los atributos específicos de las Entidades de Salud
    /// </summary>
    public class Salud : CuentaUsuario
    {
        #region Propiedades específicas de salud       
        // Número de identificación tributaria
        public string Numero { get; set; } = string.Empty;

        // Nombre de la entidad prestadora de salud
        public string RazonSocialSalud { get; set; } = string.Empty;
        #endregion

        // Constructor privado para forzar el uso del builder
        internal Salud() {}

        // Método estático para obtener un builder de Salud
        public static SaludBuilder CrearBuilder()
        {
            return new SaludBuilder();
        }
    }

    // Clase separada para el builder de Salud
    public class SaludBuilder
    {
        private readonly Salud _salud = new Salud();

        public SaludBuilder ConNumero(string numero)
        {
            _salud.Numero = numero;
            return this;
        }

        public SaludBuilder ConRazonSocialSalud(string razonSocialSalud)
        {
            _salud.RazonSocialSalud = razonSocialSalud;
            return this;
        }

        // Método para construir la instancia final
        public Salud Build()
        {
            if(string.IsNullOrEmpty(_salud.Numero))
            {
                throw new InvalidOperationException("El número de afiliación es obligatorio.");
            }
            if(string.IsNullOrEmpty(_salud.RazonSocialSalud))
            {
                throw new InvalidOperationException("La razón social del prestador en salud es obligatorio");
            }
            return _salud;
        }
    }
}
