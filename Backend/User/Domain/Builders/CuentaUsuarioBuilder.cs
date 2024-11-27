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

        public CuentaUsuarioBuilder ConEsActivo(bool esActivo)
        {
            _cuentaUsuario.EsActivo = esActivo;
            return this;
        }

        public CuentaUsuarioBuilder ConFechaRegistro(DateTime fechaRegistro)
        {
            _cuentaUsuario.FechaRegistro = fechaRegistro;
            return this;
        }

        public CuentaUsuarioBuilder ConFechaInactivacion(DateTime? fechaInactivacion)
        {
            _cuentaUsuario.FechaInactivacion = fechaInactivacion;
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

        public CuentaUsuarioBuilder ConAfiliacion(Afiliacion afiliacion)
        {
            _cuentaUsuario.Afiliacion = afiliacion;
            return this;
        }

        public CuentaUsuarioBuilder ConDiasPendientes(int? diasPendientes)
        {
            _cuentaUsuario.DiasPendientes = diasPendientes;
            return this;
        }

        public CuentaUsuarioBuilder ConFechaCreacion(DateTime fechaCreacion)
        {
            _cuentaUsuario.FechaCreacion = fechaCreacion;
            return this;
        }
        // Métodos de auditoría a Afiliación.
        public CuentaUsuarioBuilder ConIntento(int intentos)
        {
            if(intentos > 2)
            {
                _cuentaUsuario.Bloqueado = true;
            }    
            return this;
        }
        public CuentaUsuarioBuilder ConBloqueado(bool bloqueado)
        {
            _cuentaUsuario.Bloqueado = bloqueado;
            return this;
        }

        public CuentaUsuario Build()
        {
            if (!CuentaUsuarioCustomValidations.ValidarSujetoRetencion(_cuentaUsuario))
            {
                throw new InvalidOperationException("El campo Sujeto Retención debe definirse para empleados.");
            }

            if (!CuentaUsuarioCustomValidations.ValidarRazonSocIdTrib(_cuentaUsuario))
            {
                throw new InvalidOperationException("Los prestadores de servicios deben incluir Razón Social y Tipo de Identificación Tributaria.");
            }

            if (!CuentaUsuarioCustomValidations.ValidarAfiliacion(_cuentaUsuario))
            {
                throw new InvalidOperationException("Días pendientes es obligatorio para usuarios con afiliación parcial.");
            }

            if (!CuentaUsuarioCustomValidations.ValidarFechaInactivacion(_cuentaUsuario))
            {
                throw new InvalidOperationException("La fecha de inactivación no puede ser anterior a la fecha de registro.");
            }

            if (!CuentaUsuarioCustomValidations.ValidarEstadoUsuario(_cuentaUsuario))
            {
                throw new InvalidOperationException("Un usuario no puede estar activo y bloqueado al mismo tiempo.");
            }

            if(!CuentaUsuarioCustomValidations.ValidarPerfilesArea(_cuentaUsuario))
            {
                throw new InvalidOperationException("Todos los perfiles asociados al usuario deben contener un área administrativa asociada");
            }

             if (!CuentaUsuarioCustomValidations.ValidarPerfilesRoles(_cuentaUsuario))
            {
                throw new InvalidOperationException("Todos los perfiles asociados al usuario deben contener roles asociados.");
            }

            if (_cuentaUsuario.Afiliacion == Afiliacion.Parcial && (!_cuentaUsuario.DiasPendientes.HasValue || _cuentaUsuario.DiasPendientes <= 0))
        {
            throw new InvalidOperationException("Para una afiliación parcial, debe definirse un plazo en días mayor a 0.");
        }
            // Retornar el objeto construido
            return _cuentaUsuario;   

        }

    }
}
