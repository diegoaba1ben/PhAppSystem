using System;
using PhAppUser.Domain.Enums;

namespace PhAppUser.Domain.Entities
{
    /// <summary>
    /// Representa los atributos específicos de las Entidades de RepLegal
    /// </summary>
    public class RepLegal : CuentaUsuario
    {
        #region Propiedades específicas de Pension       
        // Número de radicado de la solicitud de la certificación legal
        public string CertLegal { get; set; } = string.Empty;

        // Fecha desde donde se autorizó ejercer funciones del representante legal
        public DateTime FechaInicio { get; set; } 

        // Fecha que se reportó como finalización de funciones del representante legal
        public DateTime FechaFinal { get; set; }
        #endregion

        // Constructor interno para forzar el uso del builder
        internal RepLegal() {}

        // Método estático para obtener un builder de RepLegal
        public static RepLegalBuilder CrearBuilder()
        {
            return new RepLegalBuilder();
        }
    }

    // Clase separada para el builder de RepLegal
    public class RepLegalBuilder
    {
        private readonly RepLegal _repLegal = new RepLegal();

        public RepLegalBuilder ConCertLegal(string certLegal)
        {
            _repLegal.CertLegal = certLegal;
            return this;
        }

        public RepLegalBuilder ConFechaInicio(DateTime fechaInicio)
        {
            _repLegal.FechaInicio = fechaInicio;
            return this;
        }
        public RepLegalBuilder ConFechaFinal(DateTime fechaFinal)
        {
            _repLegal.FechaFinal = fechaFinal;
            return this;
        }

        // Método para construir la instancia final
        public RepLegal Build()
        {
            return _repLegal;
        }
    }
}