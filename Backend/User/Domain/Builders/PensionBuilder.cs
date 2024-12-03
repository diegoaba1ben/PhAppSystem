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
        public PensionBuilder ConRazonSocialPension(string razonSocialPension)
        {
            _pension.RazonSocialPension = razonSocialPension;
            return this;
        }

        // Constructor de la clase
        public Pension Build()
        {
            if(string.IsNullOrEmpty(_pension.Numero))
            {
                throw new InvalidOperationException("El número de afiliación a pensión es obligatorio.");
            }
            if(string.IsNullOrEmpty(_pension.RazonSocialPension))
            {
                throw new InvalidOperationException("La razón social de la entidad de Pensión es obligatoria.");
            }
            return _pension;
        }
    }
}