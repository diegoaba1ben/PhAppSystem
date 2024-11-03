using System;
using System.Globalization;
using FluentValidation;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Domain.Validators
{
    ///  <summary>
    ///  Clase que representa las validaciones para la entidad Permiso.
    /// </summary>
    public class PermisoValidator : AbstractValidator<Permiso>
    {
        public PermisoValidator()
        {
            RuleFor(p => p.Codigo)
                .NotEmpty().WithMessage("El campo código es requerido.")
                .MaximumLength(5).WithMessage("El campo no puede tener más de 5 caracteres")
                .Matches(@"^\d+$").WithMessage("El campo sólo acepta carecteres numéricos");
            RuleFor(p => p.Nombre)
                .NotEmpty().WithMessage("El campo nombre es requerido.")
                .Length(3,30).WithMessage("El campo nombre debe estar entre 3 y 30 caracteres")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("El campo sólo acepta letras y espacios en blanco.");
            RuleFor(p => p.Descripcion)
                .NotEmpty().WithMessage("El campo descripción es requerido.")
                .Length(150).WithMessage("El campo acepta  hasta 150 caracteres")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("El campo descripción solo acepta letras y espacios.");
            RuleFor(p => p.FechaCreacion)
                .NotEmpty().WithMessage("El campo fecha de creación es requerido.")
                .Must(BeAValidISO8601Date).WithMessage("La fecha final de la representación legal debe estar en formato ISO 8601 (yyyy-MM-dd, yyyy-MM-ddTHH:mm)");
        }
        /// <summary>
        /// Método para validar que las fechas estén en formato ISO 8601
        /// </summary>
        private bool BeAValidISO8601Date(DateTime fecha)
        {
            string[] formats = { "yyyy-MM-dd", "yyyy-MM-ddTHH:mm", "yyyy-MM-ddTHH:mm:ss" };
            return DateTime.TryParseExact(fecha.ToString("o"), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }
}