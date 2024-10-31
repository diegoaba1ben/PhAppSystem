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
            #endregion

            #region Validaciones aplicables a los atributos de estado de un usuario
            RuleFor(cu =>cu.EsActivo)
                .NotNull().WithMessage("El campo de activación es requerido");
                
            RuleFor(cu => cu.FechaRegistro)
                .NotEmpty().WithMessage("La fecha de registro es requerida");

            RuleFor(cu => cu.FechaInactivacion)
                .Must(date => date == null || date >= DateTime.UtcNow)
                .WithMessage("La fecha de inactivación no puede ser menor a la fecha actual.");
            #endregion

            #region Validaciones para los atributos de login
            RuleFor(cu => cu.NombreUsuario)
                .NotEmpty().WithMessage("El campo Nombre de usuario es requerido");
                
            RuleFor(cu => cu.Password)
                .NotEmpty().WithMessage("El campo Password es requerido.")
                .Length(8, 20).WithMessage("La contraseña debe contener entre 8 y 20 caracteres")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$")
                .WithMessage("La contraseña debe tener entre 8 y 20 caracteres, con al menos una letra minúscula, una mayúscula, un número y un carácter especial, y no debe contener espacios en blanco.");
            #endregion

            #region Validaciones para determinar el tipo de usuario
            RuleFor(cu => cu.TarjProf)
                .NotEmpty().When(cu => cu.TipoCuenta == TipoCuenta.Contador)
                .WithMessage("Debe ingresar la tarjeta profesional cuando el usuario es un Contador y debe tener entre 5 y 20 caracteres")
                .Length(5, 20)
                .Matches(@"^[a-zA-Z0-9]+$").WithMessage("El atributo solo puede contener letras y números");
            #endregion

            #region Validaciones para el manejor del tipo de contrato y obligaciones tributarias
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
                .NotNull().WithMessage("El tipo de afiliación es requerido");
            #endregion
        }
    }
}

