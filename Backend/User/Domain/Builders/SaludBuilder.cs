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
            return _salud;
        }
    }

}


