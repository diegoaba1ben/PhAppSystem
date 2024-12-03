using System;
using PhAppUser.Domain.Enums;
using PhAppUser.Domain.Entities;
using PhAppUser.Domain.Validators;

namespace PhAppUser.Domain.Builders
{
    /// <summary>
    /// Builder para la creación de instancias de CuentaUsuario, proporcionando flexibilidad adicional.
    /// </summary>
    public class CuentaUsuarioBuilder
    {
        private readonly CuentaUsuario _cuentaUsuario;

        public CuentaUsuarioBuilder()
        {
            _cuentaUsuario = new CuentaUsuario();
        }

        #region Métodos de Configuración Básica
        public CuentaUsuarioBuilder GenerarNuevoId()
        {
            _cuentaUsuario.Id = Guid.NewGuid();
            return this;
        }

        public CuentaUsuarioBuilder ConNombresCompletos(string nombres)
        {
            _cuentaUsuario.NombresCompletos = nombres;
            return this;
        }

        public CuentaUsuarioBuilder ConApellidosCompletos(string apellidos)
        {
            _cuentaUsuario.ApellidosCompletos = apellidos;
            return this;
        }

        public CuentaUsuarioBuilder ConTipoId(TipoId tipoId)
        {
            _cuentaUsuario.TipoId = tipoId;
            return this;
        }

        public CuentaUsuarioBuilder ConIdentificacion(string identificacion)
        {
            _cuentaUsuario.Identificacion = identificacion;
            return this;
        }

        public CuentaUsuarioBuilder ConDireccion(string direccion)
        {
            _cuentaUsuario.Direccion = direccion;
            return this;
        }

        public CuentaUsuarioBuilder ConCiudad(string ciudad)
        {
            _cuentaUsuario.Ciudad = ciudad;
            return this;
        }

        public CuentaUsuarioBuilder ConTelefono(string telefono)
        {
            _cuentaUsuario.Telefono = telefono;
            return this;
        }

        public CuentaUsuarioBuilder ConEmail(string email)
        {
            _cuentaUsuario.Email = email;
            return this;
        }
        #endregion

        #region Métodos de Configuración Avanzada
        public CuentaUsuarioBuilder ConEsActivo(bool esActivo)
        {
            _cuentaUsuario.EsActivo = esActivo;
            return this;
        }

        public CuentaUsuarioBuilder ConNombreUsuario(string nombreUsuario)
        {
            _cuentaUsuario.NombreUsuario = nombreUsuario;
            return this;
        }

        public CuentaUsuarioBuilder ConPassword(string password)
        {
            _cuentaUsuario.Password = password;
            return this;
        }

        public CuentaUsuarioBuilder ConTipoCuenta(TipoCuenta tipoCuenta)
        {
            _cuentaUsuario.TipoCuenta = tipoCuenta;
            return this;
        }

        public CuentaUsuarioBuilder ConTarjProf(string tarjProf)
        {
            _cuentaUsuario.TarjProf = tarjProf;
            return this;
        }

        public CuentaUsuarioBuilder ConTipoContrato(TipoContrato tipoContrato)
        {
            _cuentaUsuario.TipoContrato = tipoContrato;
            return this;
        }

        public CuentaUsuarioBuilder ConSujetoRetencion(bool? sujetoRetencion)
        {
            _cuentaUsuario.SujetoRetencion = sujetoRetencion;
            return this;
        }

        public CuentaUsuarioBuilder ConTipoIdTrib(TipoIdTrib? tipoIdTrib)
        {
            _cuentaUsuario.TipoIdTrib = tipoIdTrib;
            return this;
        }

        public CuentaUsuarioBuilder ConRazonSocial(string razonSocial)
        {
            _cuentaUsuario.RazonSocial = razonSocial;
            return this;
        }

        public CuentaUsuarioBuilder ConAfiliacion(Afiliacion afiliacion, int? diasPendientes = null)
        {
            _cuentaUsuario.Afiliacion = afiliacion;
            _cuentaUsuario.DiasPendientes = afiliacion == Afiliacion.Parcial ? diasPendientes : null;
            return this;
        }

        public CuentaUsuarioBuilder ConIntento(int intentos)
        {
            _cuentaUsuario.Intento = intentos;
            _cuentaUsuario.Bloqueado = intentos > 2;
            return this;
        }

        public CuentaUsuarioBuilder ConBloqueado(bool bloqueado)
        {
            _cuentaUsuario.Bloqueado = bloqueado;
            return this;
        }

        // Métodos para Configurar Salud y Pensión
        public CuentaUsuarioBuilder ConSalud(Salud salud)
        {
            _cuentaUsuario.Salud = salud;
            return this;
        }

        public CuentaUsuarioBuilder ConPension(Pension pension)
        {
            _cuentaUsuario.Pension = pension;
            return this;
        }
        #endregion

        public CuentaUsuario Build()
        {
            // Validaciones Centralizadas
            if (!CuentaUsuarioCustomValidations.ValidarAfiliacion(_cuentaUsuario))
            {
                throw new InvalidOperationException("La afiliación no es válida.");
            }

            if (!CuentaUsuarioCustomValidations.ValidarEstadoUsuario(_cuentaUsuario))
            {
                throw new InvalidOperationException("El estado del usuario no es consistente.");
            }

            if (_cuentaUsuario.Afiliacion == Afiliacion.Completa && (_cuentaUsuario.Salud == null || _cuentaUsuario.Pension == null))
            {
                throw new InvalidOperationException("La afiliación completa requiere tanto salud como pensión.");
            }

            // Retornar la cuenta construida
            return _cuentaUsuario;
        }
    }
}
