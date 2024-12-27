using System;
using FluentValidation;

namespace PhAppUser.Application.DTOs
{
    public class UsuarioDatosBasicosQDto
    {
        public Guid Id { get; set; }
        public string NombresCompletos { get; set; } = string.Empty;
        public string ApellidosCompletos { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TipoContrato { get; set; } = string.Empty;
    }
    public class UsuarioDatosBasicosQDtoValidator : AbstractValidator<UsuarioDatosBasicosQDto>
    {
        public UsuarioDatosBasicosQDtoValidator()
        {
            RuleFor(x => x.NombresCompletos)
                .MinimumLength(3).When(x => !string.IsNullOrEmpty(x.NombresCompletos))
                .WithMessage("El nombre debe tener al menos 3 caracteres.");

            RuleFor(x => x.ApellidosCompletos)
                .MinimumLength(3).When(x => !string.IsNullOrEmpty(x.ApellidosCompletos))
                .WithMessage("El apellido debe tener al menos 3 caracteres.");

            RuleFor(x => x.Identificacion)
                .MinimumLength(3).When(x => !string.IsNullOrEmpty(x.Identificacion))
                .WithMessage("La identificación debe tener al menos 3 caracteres.")
                .Matches(@"^\d+$").When(x => !string.IsNullOrEmpty(x.Identificacion))
                .WithMessage("La identificación debe contener solo dígitos.");
        }
    }

}