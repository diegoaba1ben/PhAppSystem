using FluentValidation;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Domain.Validators
{
    /// <summary>
    /// Clase de validación para la entidad Perfil utilizando Fluent Validation
    /// </summary>
    public class PerfilValidator : AbstractValidator<Perfil>
    {
        public PerfilValidator()
        {
            RuleFor(p => p.Area)
                .NotNull().WithMessage("El área asignada es requerida.")
                .DependentRules(() =>
                {
                    RuleFor(p => p.AreaId)
                        .Equal(p => p.Area!.Id)
                        .WithMessage("El AreaId debe coincidir con el Id del área asignada.");
                });

            RuleFor(p => p.Roles)
                .NotEmpty().WithMessage("El perfil debe tener al menos un rol asignado.")
                .Must(roles => roles.Count == roles.Distinct().Count())
                .WithMessage("El perfil contiene roles duplicados.");

            RuleFor(p => p.CuentaUsuarios)
                .NotEmpty().WithMessage("El perfil debe estar asignado a al menos un usuario.")
                .Must(cuentaUsuarios => cuentaUsuarios.Count == cuentaUsuarios.Distinct().Count())
                .WithMessage("El perfil contiene usuarios duplicados.");
        }
    }
}
