using System;
using PhAppUser.Domain.Enums;

namespace PhAppUser.Domain.Entities
{
    /// <summary>
    /// Representa los atributos específicos de las Entidades de Pension
    /// </summary>
    public class Pension : CuentaUsuario
    {
        #region Propiedades específicas de Pension       
        // Número de identificación tributaria
        public string Numero { get; set; } = string.Empty;

        // Nombre de la entidad prestadora de pensiones
        public string RazonSocialPension { get; set; } = string.Empty;
        #endregion

        // Constructor privado para forzar el uso del Builder
        internal Pension(){}

        // Método estático para obtener un builder de pensión
        public static PensionBuilder CrearBuilder()
        {
            return new PensionBuilder();
        }
    }

    // Clase separada para el builder de pensión
    public class PensionBuilder
    {
        private readonly Pension _pension = new Pension();

        public PensionBuilder ConNumero(string numero)
        {
            _pension.Numero = numero;
            return this;
        }

        public PensionBuilder ConRazonSocialPension(string razonSocialPension)
        {
            _pension.RazonSocial = razonSocialPension;
            return this;
        }

        // Método para construir la instancia final
        public Pension Build()
        {
            return _pension;
        }
    }
}