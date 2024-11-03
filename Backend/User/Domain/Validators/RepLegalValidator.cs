using System;
using System.Globalization;
using FluentValidation;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Domain.Validators
{
    /// <summary>
    /// Clase de validación para la entidad Representación legal utilizando Fluent Validations
    /// </summary>
    public class RepLegalValidator : AbstractValidator<RepLegal>
    {
        public RepLegalValidator()
        {
            RuleFor(r => r.CertLegal)
                .NotEmpty().WithMessage("El número de radicación de la certificación legal es requerido")
                .Length(3, 12).WithMessage("El campo CertLegal debe tener entre 3 y 12 caracteres")
                .Matches(@"^\d+$").WithMessage("El campo de radicación de la certificación legal debe ser numérico");

            RuleFor(r => r.FechaInicio)
                .NotEmpty().WithMessage("La fecha de inicio de la representación legal es requerida")
                .Must(BeAValidISO8601Date).WithMessage("La fecha de inicio de la representación legal debe estar en formato ISO 8601 (yyyy-MM-dd, yyyy-MM-ddTHH:mm, yyyy-MM-ddTHH:mm:ss)");

            RuleFor(r => r.FechaFinal)
                .NotEmpty().WithMessage("La fecha de vencimiento de la representación legal es requerida")
                .Must(BeAValidISO8601Date).WithMessage("La fecha final de la representación legal debe estar en formato ISO 8601 (yyyy-MM-dd, yyyy-MM-ddTHH:mm)");
        }

        /// <summary>
        /// Método para validar que las fechas estén en formato ISO 8601
        /// </summary>
        private bool BeAValidISO8601Date(DateTime fecha)
        {
            string[] formats = { "yyyy-MM-dd", "yyyy-MM-ddTHH:mm", "yyyy-MM-ddTHH:mm:ss" };
            string fechaStr = fecha.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
            return DateTime.TryParseExact(fechaStr, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }
}
