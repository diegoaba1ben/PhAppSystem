using System.Globalization;
using FluentValidation;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Domain.Validators
{
    /// <summary>
    /// Clase de validación para la entidad Pensión utilizando Fluent Validations
    /// </summary>
    public class PensionValidator : AbstractValidator <Pension>
    {
        public PensionValidator()
        {
            RuleFor(p => p.Numero)
                .NotEmpty().WithMessage("El campo número es requerido.")
                .Length(5,25).WithMessage("El campo debe tener entre 5 y 25 caracteres.")
                .Matches(@"^\d+$").WithMessage("El campo solo debe contener números.");
            RuleFor(p => p.RazonSocial)
                .NotEmpty().WithMessage("El campo Razón Social es requerido")
                .Length(5, 25).WithMessage("El campo debe tener entre 5 y  25 caracteres")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("El campo debe contener letras y espacios.");

        }
    }

}