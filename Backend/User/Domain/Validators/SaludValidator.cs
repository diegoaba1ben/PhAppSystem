using System.Globalization;
using FluentValidation;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Domain.Validators
{
    /// <summary>
    /// Clase de validación para la entidad salud utilizando Fluent Validations
    /// </summary>
    public class SaludValidator : AbstractValidator<Salud>
    {
        public SaludValidator()
        {
            RuleFor(s => s.Numero)
                .NotEmpty().WithMessage("El número de afiliación es requerido.")
                .Length(5,25).WithMessage("El campo debe tener entre 5 y 25 caracteres")
                .Matches(@"^\d+$").WithMessage("El campo solo debe contener números");
            RuleFor(s => s.RazonSocialSalud)
                .NotEmpty().WithMessage("El campo Razón social es requerido.")
                .MaximumLength(25).WithMessage("El campo  debe tener como máximo 25 caracteres")
                .Matches(@"^[a-zA-Z\s'-]+$").WithMessage("El campo solo debe contener letras y espacios");
        }
    }
}