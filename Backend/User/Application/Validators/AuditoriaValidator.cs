using FluentValidation;
using PhAppUser.Application.DTOs;

namespace PhAppUser.Application.Validators
{
    /// <summary>
    /// Validador para el DTO de auditoría de usuarios.
    /// </summary>
    public class AuditUsuarioDtoValidator : AbstractValidator<AuditoriaUsuarioDto>
    {
        public AuditUsuarioDtoValidator()
        {
            // Validaciones para fechas de auditoría
            RuleFor(x => x.FechaRegistro)
                .NotEmpty()
                .WithMessage("La fecha de registro es obligatoria.")
                .Must(date => BeFutureOrPastDate(date))
                .WithMessage("La fecha de registro debe ser válida.");

            RuleFor(x => x.FechaInactivacion)
                .Must(date => BeFutureOrPastDate(date))
                .When(x => x.FechaInactivacion.HasValue)
                .WithMessage("La fecha de inactivación debe ser válida.");

            // Validación para el historial de roles y permisos
            RuleForEach(x => x.HistorialRolesPermisos)
                .ChildRules(roles =>
                {
                    roles.RuleFor(r => r.FechaRegistro)
                        .NotEmpty()
                        .WithMessage("La fecha de registro de rol es obligatoria.")
                        .Must(date => BeFutureOrPastDate(date))
                        .WithMessage("La fecha de registro del rol debe ser válida.");

                    roles.RuleFor(r => r.FechaRevocacion)
                        .Must(date => BeFutureOrPastDate(date))
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
                        .Must(date => BeFutureOrPastDate(date))
                        .WithMessage("La fecha del evento debe ser válida.");
                });
        }

        /// <summary>
        /// Valida que la fecha sea válida (pasada o futura).
        /// </summary>
        /// <param name="date">Fecha a validar.</param>
        /// <returns>True si la fecha es válida o si es nula.</returns>
        private bool BeFutureOrPastDate(DateTime? date)
        {
            // Cualquier fecha válida o nula es aceptable
            return !date.HasValue || date.Value != default;
        }
    }
}
