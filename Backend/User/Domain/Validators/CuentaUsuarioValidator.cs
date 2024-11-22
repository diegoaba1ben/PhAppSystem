using System;
using System.Globalization;
using FluentValidation;
using PhAppUser.Domain.Entities;
using PhAppUser.Domain.Enums;

namespace PhAppUser.Domain.Validators
{
    /// <summary>
    /// Clase de validación para la entidad CuentaUsuario utilizando Fluent Validations
    /// </summary>
    public class CuentaUsuarioValidator : AbstractValidator<CuentaUsuario>
    {
        public CuentaUsuarioValidator()
        {
            #region Validaciones aplicables a las propiedades básicas de un usuario
            RuleFor(cu => cu.NombresCompletos)
                .NotEmpty().WithMessage("El campo Nombres Completos es requerido.")
                .Length(3, 60).WithMessage("El campo Nombres completos debe tener entre 3 y 60 caracteres")
                .Matches(@"^[\p{L}''\-\s]+$").WithMessage("El campo nombres completos solo debe contener letras y espacios en blanco");
                
            RuleFor(cu => cu.ApellidosCompletos)
                .NotEmpty().WithMessage("El campo apellidos completos es requerido")
                .Length(3, 60).WithMessage("El campo apellidos completos debe tener entre 3 y 60 caracteres")
                .Matches(@"^[\p{L}''\-\s]+$").WithMessage("El campo Apellidos Completos solo debe contener letras y espacios en blanco. ");
                
            RuleFor(cu => cu.Identificacion)
                .NotEmpty().WithMessage("El campo Identificación es requerido")
                .Length(5, 20).WithMessage("La Identificación debe tener entre 5 y 20 caracteres")
                .Matches(@"^\d+$").WithMessage("el campo identificación solo debe contener números");
            #endregion

            #region Validaciones aplicables a los atributos de ubicación de un usuario
            RuleFor(cu => cu.Direccion)
                .NotEmpty().WithMessage("El campo dirección es requerido")
                .Length(5, 100).WithMessage("El campo dirección debe tener entre 5 y 100 caracteres.")
                .Matches(@"^[a-zA-Z0-9\s]+$").WithMessage("El campo dirección solo acepta números y letras");
            RuleFor(cu => cu.Ciudad)
                .NotEmpty().WithMessage("El campo Ciudad es requerido")
                .Length(3, 60).WithMessage("El campo Ciudad debe tener entre 3 y 60  caracteres")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("El campCiudad solo acepta letras");
            RuleFor(cu => cu.Telefono)
                .NotEmpty().WithMessage("El campo Teléfono  es requerido")
                .Length(7, 20).WithMessage("El campo Teléfono debe tener entre  7 y 20 caracteres")
                .Matches(@"^\d+$").WithMessage("El campo Teléfono solo acepta números");
            RuleFor(cu => cu.Email)
                .NotEmpty().WithMessage("El correo electrónico es requerido")
                .EmailAddress().WithMessage("El correo electrónico es no válido.");
            #endregion

            #region Validaciones aplicables a los atributos de estado de un usuario
            RuleFor(cu =>cu.EsActivo)
                .NotNull().WithMessage("El campo de activación es requerido");
                
            RuleFor(cu => cu.FechaRegistro)
                .NotEmpty().WithMessage("La fecha de registro es requerida");

            RuleFor(cu => cu.FechaInactivacion)
                .Must(date => date == null || date >= DateTime.UtcNow)
                .WithMessage("La fecha de inactivación no puede ser hoy o una fecha futura.")
                .NotNull().When(cu =>cu.Bloqueado)
                .WithMessage("La fecha de inactivación es requerida si el usuairo no está activo.") ;
            #endregion

            #region Validaciones para los atributos de loggin
            RuleFor(cu => cu.NombreUsuario)
                .NotEmpty().WithMessage("El campo Nombre de usuario es requerido");
                
            RuleFor(cu => cu.Password)
                .NotEmpty().WithMessage("El campo Password es requerido.")
                .Length(8, 20).WithMessage("La contraseña debe contener entre 8 y 20 caracteres")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$")
                .WithMessage("La contraseña debe contener al menos una letra minúscula, una mayúscula, un número y un carácter especial. Ejemplo: Passw0rd@.");;
            #endregion

            #region Validaciones para determinar el tipo de usuario
            RuleFor(cu => cu.TarjProf)
                .NotEmpty().When(cu => cu.TipoCuenta == TipoCuenta.Contador)
                .WithMessage("Debe ingresar la tarjeta profesional cuando el usuario es un Contador y debe tener entre 5 y 20 caracteres")
                .Length(5, 20).When (cu => cu.TipoCuenta == TipoCuenta.Contador)
                .Matches(@"^[a-zA-Z0-9]+$").WithMessage("El atributo solo puede contener letras y números");
            #endregion

            #region Validaciones para el manejar del tipo de contrato y obligaciones tributarias
            RuleFor(cu => cu.TipoContrato)
                .NotNull().WithMessage("El tipo de contrato es requerido")
                .IsInEnum().WithMessage("El tipo de contrato no es válido");
            When(cu => cu.TipoContrato == TipoContrato.Empleado,() =>
            {
                RuleFor(cu => cu.SujetoRetencion)
                    .NotNull().WithMessage("Debe indicar si el empleado es sujeto a retención");
            });
            When(cu => cu.TipoContrato == TipoContrato.PrestadorDeServicios,() =>
            {
                RuleFor(cu => cu.TipoIdTrib)
                    .NotNull().WithMessage("El tipo de identificación tributaria es requerida para prestadores de servicios")
                    .IsInEnum().WithMessage("El tipo de identificación tributaria no es válido");
                RuleFor(cu => cu.RazonSocial)
                    .NotNull().WithMessage("El nombre o razón social es requerido para prestadores de servicios");
            });
            #endregion

            #region Validaciones para los atributos relacionados con la seguridad social
            RuleFor(cu => cu.Afiliacion)
                .IsInEnum().WithMessage("El tipo de afiliación no es válido.");
            RuleFor(cu => cu.DiasPendientes)
                .GreaterThanOrEqualTo(0).When(cu => cu.Afiliacion == Enums.Afiliacion.Parcial)
                .WithMessage("Los días pendientes deben ser mayores o iguales a 0 si la afiliación es parcial")
                .Null().When(cu => cu.Bloqueado)
                .WithMessage("Un usuario Bloqueado no puede tener días pendientes de afiliación");
            // Validación de los intentos de aplazamiento
            RuleFor(cu => cu.Intento)
                .InclusiveBetween(0,2). WithMessage("El número de intentos de aplazamiento no debe ser mayor a 2.")
                .LessThanOrEqualTo(2).When(cu => !cu.Bloqueado)
                .WithMessage("El número de intentos no debe superar los 2 intentos si el usuario no está bloqueado");       
            // Validar Bloqueado
            RuleFor(cu => cu.Bloqueado)
                .Must((cu, bloqueado) => !bloqueado || (cu.Intento >= 2 && !cu.EsActivo))
                .WithMessage("El usuario solo puede ser bloqueado si ha superado los intentos permitidos y está inactivo.");                
            #endregion
        }
    }
}

