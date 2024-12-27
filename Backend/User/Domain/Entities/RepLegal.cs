namespace PhAppUser.Domain.Entities
{
    public class RepLegal : CuentaUsuario
    {
        #region Propiedades específicas de RepLegal
        public string CertLegal { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        #endregion

        #region Relación con CuentaUsuario
        public Guid CuentaUsuarioId { get; set; }
        public CuentaUsuario CuentaUsuario { get; set; } = null!;
        #endregion

        // Constructor interno para forzar el uso del builder de CuentaUsuario
        internal RepLegal() { }

        /// <summary>
        /// Builder específico para los atributos de RepLegal.
        /// Reutiliza el builder de CuentaUsuario para los atributos base.
        /// </summary>
        public new class Builder : CuentaUsuario.Builder
        {
            private readonly RepLegal _repLegal;

            public Builder()
            {
                _repLegal = new RepLegal();
            }

            #region Métodos de Configuración Específicos
            public Builder ConCertLegal(string certLegal)
            {
                _repLegal.CertLegal = certLegal;
                return this;
            }

            public Builder ConFechaInicio(DateTime fechaInicio)
            {
                _repLegal.FechaInicio = fechaInicio;
                return this;
            }

            public Builder ConFechaFinal(DateTime fechaFinal)
            {
                _repLegal.FechaFinal = fechaFinal;
                return this;
            }

            public Builder ConCuentaUsuario(CuentaUsuario cuentaUsuario)
            {
                _repLegal.CuentaUsuario = cuentaUsuario ?? throw new ArgumentNullException(nameof(cuentaUsuario));
                _repLegal.CuentaUsuarioId = cuentaUsuario.Id;
                return this;
            }
            #endregion

            #region Validaciones y Construcción
            public new RepLegal Build()
            {
                // Validaciones específicas de RepLegal
                if (string.IsNullOrEmpty(_repLegal.CertLegal))
                    throw new InvalidOperationException("El número de radicación (CertLegal) es obligatorio.");

                if (_repLegal.FechaInicio >= _repLegal.FechaFinal)
                    throw new InvalidOperationException("La fecha de inicio debe ser anterior a la fecha final.");

                // Llamada al builder base para validar atributos heredados
                base.Build();

                return _repLegal;
            }
            #endregion
        }
    }
}

