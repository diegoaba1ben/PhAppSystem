using FluentValidation;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Domain.Validators
{
    /// <summary>
    /// Clase de validación para la entidad Rol utilizando Fluent Validation
    /// </summary>
    public class RolValidator : AbstractValidator<Rol>
    {
        public RolValidator()
        {
            RuleFor(r => r.Nombre)
                .NotEmpty().WithMessage("El campo nombre es requerido.")
                .Length(3, 25).WithMessage("El nombre debe tener entre 3 y 25 caracteres.")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("El nombre solo debe contener letras y espacios.");

            RuleFor(r => r.Descripcion)
                .NotEmpty().WithMessage("El campo descripción es requerido.")
                .MaximumLength(150).WithMessage("La descripción no debe exceder los 150 caracteres.");

            RuleFor(r => r.Permisos)
                .NotEmpty().WithMessage("El rol debe tener al menos un permiso asignado.")
                .Must(p => p.Count > 0).WithMessage("El rol debe contener permisos asignados.");
        }
    }
}
