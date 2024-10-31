using System;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Domain.Builders
{
    /// <summary>
    /// Builder para la creación de instancias de RepLegal, proporcionando flexibilidad adicional.
    /// </summary>
    public class RepLegalBuilder
    {
        private readonly RepLegal _repLegal;

        public RepLegalBuilder()
        {
            _repLegal = new RepLegal();
        }

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

        // Método para construir la instancia final de RepLegal
        public RepLegal Build()
        {
            return _repLegal;
        }
    }
}
