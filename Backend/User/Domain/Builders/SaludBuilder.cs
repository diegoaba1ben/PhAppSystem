using System;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Domain.Builders
{
    /// <summary>
    /// Builder para la creación de instancias de Salud
    /// </summary>
    public class SaludBuilder
    {
        private readonly Salud _salud;

        public SaludBuilder()
        {
            _salud = new Salud();
        }
        // Métodos de cada atributo
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

        // Constructor de la clase
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


