using System.Globalization;
using FluentValidation;
using PhAppUser.Application.Services.Validation;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Domain.Validators
{
    /// <summary>
    /// Clase de validación para la entidad Representación legal utilizando Fluent Validations
    /// </summary>
    public class RepLegalValidator : AbstractValidator<RepLegal>
    {
        public RepLegalValidator(DatabaseValidationService validationService)
        {
            // Validaciones heredadas de CuentaUsuario
            Include(new CuentaUsuarioValidator(validationService));

            // Validación para el número de radicación de la certificación legal
            RuleFor(r => r.CertLegal)
                .NotEmpty().WithMessage("El número de radicación de la certificación legal es requerido.")
                .Length(3, 12).WithMessage("El campo CertLegal debe tener entre 3 y 12 caracteres.")
                .Matches(@"^\d+$").WithMessage("El campo de radicación de la certificación legal debe ser numérico.")
                .MustAsync(async (certLegal, cancellation) =>
                {
                    return await validationService.CertLegalEsUnicoAsync(certLegal);
                })
                .WithMessage("El número de radicación ya está registrado.");

            // Validación para la fecha de inicio
            RuleFor(r => r.FechaInicio)
                .NotEmpty().WithMessage("La fecha de inicio de la representación legal es requerida.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de inicio no puede estar en el futuro.");

            // Validación para la fecha final
            RuleFor(r => r.FechaFinal)
                .NotEmpty().WithMessage("La fecha de vencimiento de la representación legal es requerida.")
                .GreaterThan(r => r.FechaInicio).WithMessage("La fecha final debe ser posterior a la fecha de inicio.");
        }

        /// <summary>
        /// Método para validar que las fechas estén en formato ISO 8601.
        /// </summary>
        private bool BeAValidISO8601Date(DateTime fecha)
        {
            string[] formats = { "yyyy-MM-dd", "yyyy-MM-ddTHH:mm", "yyyy-MM-ddTHH:mm:ss" };
            string fechaStr = fecha.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
            return DateTime.TryParseExact(fechaStr, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }
}

