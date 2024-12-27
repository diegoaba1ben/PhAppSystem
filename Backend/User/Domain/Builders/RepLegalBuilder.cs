using System;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Domain.Builders
{
    /// <summary>
    /// Builder externo para construir instancias de RepLegal, extendiendo el builder base.
    /// </summary>
    public class RepLegalBuilder : CuentaUsuarioBuilder
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

        public RepLegalBuilder ConCuentaUsuario(CuentaUsuario cuentaUsuario)
        {
            _repLegal.CuentaUsuario = cuentaUsuario ?? throw new ArgumentNullException(nameof(cuentaUsuario));
            _repLegal.CuentaUsuarioId = cuentaUsuario.Id;
            return this;
        }

        /// <summary>
        /// Método para construir la instancia final de RepLegal.
        /// </summary>
        /// <returns>Una instancia de RepLegal completa.</returns>
        public new RepLegal Build()
        {
            // Validar la coherencia de los datos antes de construir.
            if (_repLegal.FechaInicio >= _repLegal.FechaFinal)
            {
                throw new InvalidOperationException("La fecha de inicio debe ser anterior a la fecha final.");
            }

            return _repLegal;
        }
    }
}
