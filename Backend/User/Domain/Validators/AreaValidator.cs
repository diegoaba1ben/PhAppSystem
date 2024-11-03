using FluentValidation;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Domain.Validators
{
    /// <summary>
    /// Clase de validación para la entidad Area utilizando Fluent Validation
    /// </summary>
    public class AreaValidator : AbstractValidator<Area>
    {
        public AreaValidator()
        {
            RuleFor(a => a.Nombre)
                .NotEmpty().WithMessage("El campo nombre es requerido.")
                .Length(3, 50).WithMessage("El nombre debe tener entre 3 y 50 caracteres.")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("El nombre solo debe contener letras y espacios.");

            RuleFor(a => a.Roles)
                .NotEmpty().WithMessage("El área debe tener al menos un rol asignado.")
                .Must(roles => roles.Count > 0).WithMessage("El área debe contener al menos un rol.");
        }
    }
}
