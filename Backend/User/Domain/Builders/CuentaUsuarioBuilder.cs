using System;
using PhAppUser.Domain.Enums;
using PhAppUser.Domain.Entities;

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

        public CuentaUsuarioBuilder ConId(int id)
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

        public CuentaUsuario Build()
        {
            return _cuentaUsuario;
        }
    }
}
