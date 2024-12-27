using System;
using FluentValidation;
using PhAppUser.Application.Services.Validation;

namespace PhAppUser.Application.DTOs
{
    public class RepLegalDto : CuentaUsuarioDto
    {
        public string CertLegal { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
    }

    public class RepLegalDtoValidator : AbstractValidator<RepLegalDto>
    {
        private readonly DtoValidationService _validationService;

        public RepLegalDtoValidator(DtoValidationService validationService)
        {
            _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));

            // Validaciones generales de CuentaUsuarioDto
            
            // Validación para CertLegal
            RuleFor(x => x.CertLegal)
                .NotEmpty().WithMessage("El número de radicación es obligatorio.")
                .Length(3, 12).WithMessage("El número de radicación debe tener entre 3 y 12 caracteres.")
                .Matches(@"^\d+$").WithMessage("El número de radicación debe ser numérico.")
                .MustAsync(async (certLegal, cancellation) =>
                    await _validationService.CertLegalEsUnicoAsync(certLegal))
                .WithMessage("El número de radicación ya está registrado.");

            // Validación para FechaInicio y FechaFinal
            RuleFor(x => x.FechaInicio)
                .NotEmpty().WithMessage("La fecha de inicio es obligatoria.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La fecha de inicio no puede ser en el futuro.");

            RuleFor(x => x.FechaFinal)
                .NotEmpty().WithMessage("La fecha de finalización es obligatoria.")
                .GreaterThan(x => x.FechaInicio).WithMessage("La fecha de finalización debe ser posterior a la fecha de inicio.");
        }
    }
}
