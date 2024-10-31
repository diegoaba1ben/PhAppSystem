using System;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Domain.Builders
{
    /// <summary>
    /// Builder para la creación de instancias de Pension
    /// </summary>
    public class PensionBuilder
    {
        private readonly Pension _pension;

        public PensionBuilder()
        {
            _pension = new Pension();
        }
        // Métodos concatenados para cada atributo
        public PensionBuilder ConNumero(string numero )
        {
            _pension.Numero = numero;
            return this;
        }
        public PensionBuilder ConRazonSocial(string razonSocial)
        {
            _pension.RazonSocial = razonSocial;
            return this;
        }

        // Constructor de la clase
        public Pension Build()
        {
            return _pension;
        }
    }
}