using FluentValidation;
using PhAppUser.Application.DTOs;

namespace PhAppUser.Application.Validators
{
    public class AuditUsuarioDtoValidator : AbstractValidator<AuditoriaUsuarioDto>
    {
        public AuditUsuarioDtoValidator()
        {
            // Validaciones para fechas de auditoría
            RuleFor(x => x.FechaRegistro)
                .Must(IsValidDate)
                .WithMessage("La fecha de registro debe ser válida.");

            RuleFor(x => x.FechaInactivacion)
                .Must(IsValidDateNullable)
                .When(x => x.FechaInactivacion.HasValue)
                .WithMessage("La fecha de inactivación debe ser válida.");

            // Validación para historial de roles y permisos
            RuleForEach(x => x.HistorialRolesPermisos)
                .ChildRules(roles =>
                {
                    roles.RuleFor(r => r.FechaRegistro)
                        .NotEmpty()
                        .WithMessage("La fecha de registro de rol es obligatoria.")
                        .Must(IsValidDate)
                        .WithMessage("La fecha de registro del rol debe ser válida.");

                    roles.RuleFor(r => r.FechaRevocacion)
                        .Must(IsValidDateNullable)
                        .When(r => r.FechaRevocacion.HasValue)
                        .WithMessage("La fecha de revocación del rol debe ser válida.");
                });

            // Validación para eventos de auditoría
            RuleForEach(x => x.EventosAuditoria)
                .ChildRules(eventos =>
                {
                    eventos.RuleFor(e => e.FechaEvento)
                        .NotEmpty()
                        .WithMessage("La fecha del evento es obligatoria.")
                        .Must(IsValidDate)
                        .WithMessage("La fecha del evento debe ser válida.");
                });
        }

        /// <summary>
        /// Valida que una fecha de tipo DateTime sea válida.
        /// </summary>
        private bool IsValidDate(DateTime date)
        {
            return date != default;
        }

        /// <summary>
        /// Valida que una fecha de tipo DateTime? sea válida.
        /// </summary>
        private bool IsValidDateNullable(DateTime? date)
        {
            return !date.HasValue || IsValidDate(date.Value);
        }
    }
}
