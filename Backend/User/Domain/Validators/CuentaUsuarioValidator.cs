using FluentValidation;
using PhAppUser.Domain.Entities;
using PhAppUser.Domain.Enums;
using PhAppUser.Application.Services.Validation;

namespace PhAppUser.Domain.Validators
{
    /// <summary>
    /// Clase de validación para la entidad CuentaUsuario utilizando Fluent Validations
    /// </summary>
    public class CuentaUsuarioValidator : AbstractValidator<CuentaUsuario>
    {
        private readonly DatabaseValidationService _validationService;

        /// <summary>
        /// Constructor que inyecta el servicio de validación de base de datos.
        /// </summary>
        /// <param name="validationService">Servicio para validar datos contra la base de datos.</param>
        public CuentaUsuarioValidator(DatabaseValidationService validationService)
        {
            _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));

            #region Validaciones aplicables a las propiedades básicas de un usuario
            RuleFor(cu => cu.NombresCompletos)
                .NotEmpty().WithMessage("El campo Nombres Completos es requerido.")
                .Length(3, 60).WithMessage("El campo Nombres completos debe tener entre 3 y 60 caracteres")
                .Matches(@"^[\p{L}''\-\s]+$").WithMessage("El campo nombres completos solo debe contener letras y espacios en blanco")
                .Must(nombres => !string.IsNullOrWhiteSpace(nombres)).WithMessage("El campo nombres completos no puede contener solo espacios.");

            RuleFor(cu => cu.ApellidosCompletos)
                .NotEmpty().WithMessage("El campo apellidos completos es requerido")
                .Length(3, 60).WithMessage("El campo apellidos completos debe tener entre 3 y 60 caracteres")
                .Matches(@"^[\p{L}''\-\s]+$").WithMessage("El campo Apellidos Completos solo debe contener letras y espacios en blanco. ")
                .Must(apellidos => !string.IsNullOrEmpty(apellidos)).WithMessage("El campo apellidos completos no puede contener solo espacios.");

            RuleFor(cu => cu.Identificacion)
                .NotEmpty().WithMessage("El campo Identificación es requerido")
                .Length(5, 20).WithMessage("La Identificación debe tener entre 5 y 20 caracteres")
                .Matches(@"^\d+$").WithMessage("El campo identificación solo debe contener números")
                .MustAsync(async (identificacion, cancellation) =>
                {
                    return await _validationService.IdentificacionEsUnicaAsync(identificacion);
                })
                .WithMessage("La identificación ya está registrada.");
            #endregion

            #region Validaciones aplicables a los atributos de ubicación de un usuario
            RuleFor(cu => cu.Direccion)
                .NotEmpty().WithMessage("El campo dirección es requerido")
                .Length(5, 100).WithMessage("El campo dirección debe tener entre 5 y 100 caracteres.")
                .Matches(@"^[a-zA-Z0-9\s,.]+$").WithMessage("El campo dirección solo acepta números y letras")
                .Must(direccion => !string.IsNullOrWhiteSpace(direccion))
                .WithMessage("El campo dirección no puede contener solo espacios.");
            RuleFor(cu => cu.Ciudad)
                .NotEmpty().WithMessage("El campo Ciudad es requerido")
                .Length(3, 60).WithMessage("El campo Ciudad debe tener entre 3 y 60 caracteres")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("El campo Ciudad solo acepta letras")
                .Must(ciudad => !string.IsNullOrWhiteSpace(ciudad))
                .WithMessage("El campo ciudad no puede contener espacios en blanco");
            RuleFor(cu => cu.Telefono)
                .NotEmpty().WithMessage("El campo Teléfono es requerido")
                .Length(7, 20).WithMessage("El campo Teléfono debe tener entre 7 y 20 caracteres")
                .Matches(@"^\d+$").WithMessage("El campo Teléfono solo acepta números");
            RuleFor(cu => cu.Email)
                .NotEmpty().WithMessage("El correo electrónico es requerido")
                .EmailAddress().WithMessage("El correo electrónico no es válido.");
            #endregion

            #region Validaciones aplicables a los atributos de estado de un usuario
            RuleFor(cu => cu.EsActivo)
                .NotNull().WithMessage("El campo de activación es requerido");

            RuleFor(cu => cu.FechaRegistro)
                .NotEmpty().WithMessage("La fecha de registro es requerida")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La fecha no puede ser en el futuro");

            RuleFor(cu => cu.FechaInactivacion)
                .Must(fecha => fecha == null || fecha >= DateTime.UtcNow)
                .WithMessage("La fecha de inactivación no puede ser hoy o una fecha futura.")
                .NotNull().When(cu => !cu.EsActivo)
                .WithMessage("La fecha de inactivación es requerida si el usuario no está activo.");
            #endregion

            #region Validaciones para los atributos de loggin
            RuleFor(cu => cu.NombreUsuario)
                .NotEmpty().WithMessage("El nombre de usuario es requerido.")
                .MustAsync(async (nombreUsuario, cancellation) =>
                {
                    return await _validationService.NombreUsuarioEsUnicoAsync(nombreUsuario);
                })
                .WithMessage("El nombre de usuario ya está registrado.");

            RuleFor(cu => cu.Password)
                .NotEmpty().WithMessage("El campo Contraseña es requerido.")
                .Length(8, 20).WithMessage("La contraseña debe tener entre 8 y 20 caracteres.")
                .Matches(@"^(?=.*[a-z])").WithMessage("La contraseña debe contener al menos una letra minúscula.")
                .Matches(@"^(?=.*[A-Z])").WithMessage("La contraseña debe contener al menos una letra mayúscula.")
                .Matches(@"^(?=.*\d)").WithMessage("La contraseña debe contener al menos un número.")
                .Matches(@"^(?=.*[@$!%*?&])").WithMessage("La contraseña debe contener al menos un carácter especial (@$!%*?&).");
            #endregion

            #region Validaciones avanzadas de CuentaUsuariocustomValidator
            RuleFor(cu => cu)
                .Must(CuentaUsuarioCustomValidations.ValidarRazonSocIdTrib)
                .WithMessage("Los prestadores de servicios deben incluir Razón Social y Tipo de Identificación Tributaria.");

            RuleFor(cu => cu)
                .Must(CuentaUsuarioCustomValidations.ValidarAfiliacion)
                .WithMessage("Días pendientes es obligatorio para usuarios con afiliación parcial.");

            RuleFor(cu => cu)
                .Must(CuentaUsuarioCustomValidations.ValidarFechaInactivacion)
                .WithMessage("La fecha de inactivación no puede ser anterior a la fecha de registro.");

            RuleFor(cu => cu)
                .Must(CuentaUsuarioCustomValidations.ValidarEstadoUsuario)
                .WithMessage("Un usuario no puede estar activo y bloqueado al mismo tiempo.");

            RuleFor(cu => cu)
                .Must(CuentaUsuarioCustomValidations.ValidarPerfilesArea)
                .WithMessage("Todos los perfiles asociados al usuario debe conteneer un área administrativa.");

            RuleFor(cu => cu)
                .Must(CuentaUsuarioCustomValidations.ValidarPerfilesRoles)
                .WithMessage("Todos los perfiles asociados al usuario deben contener roles.");
            #endregion
        }
    }
}
