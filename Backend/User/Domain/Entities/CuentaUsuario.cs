using System;
using System.ComponentModel.DataAnnotations;
using PhAppUser.Domain.Enums;

namespace PhAppUser.Domain.Entities
{
    /// <summary>
    /// Representa a un usuario en el sistema.
    /// </summary>
    public class CuentaUsuario
    {
        #region Atributos básicos para un usuario
        [Key]
        public Guid Id { get; internal set; }
        public string NombresCompletos { get; internal set; } = string.Empty;
        public string ApellidosCompletos { get; internal set; } = string.Empty;
        public TipoId TipoId { get; internal set; }
        public string Identificacion { get; internal set; } = string.Empty;
        #endregion

        #region Atributos de ubicación
        public string Direccion { get; internal set; } = string.Empty;
        public string Ciudad { get; internal set; } = string.Empty;
        public string Telefono { get; internal set; } = string.Empty;
        public string Email { get; internal set; } = string.Empty;
        #endregion

        #region Atributos del estado de un usuario
        public bool EsActivo { get; internal set; } = true;
        public DateTime FechaRegistro { get; internal set; } = DateTime.Now;
        public DateTime? FechaInactivacion { get; internal set; }
        #endregion

        #region Atributos de identificación de login
        public string NombreUsuario { get; internal set; } = string.Empty;
        public string Password { get; internal set; } = string.Empty;
        public DateTime? FechaUltimoLogin { get; internal set; }
        #endregion

        #region Atributos para determinar el tipo de usuario
        public TipoCuenta TipoCuenta { get; internal set; }
        public string TarjProf { get; internal set; } = string.Empty;
        #endregion

        #region Tipo de contrato y manejo tributario
        public TipoContrato TipoContrato { get; internal set; }
        public bool? SujetoRetencion { get; internal set; }
        public TipoIdTrib? TipoIdTrib { get; internal set; }
        public string RazonSocial { get; internal set; } = string.Empty;
        #endregion

        #region Atributos relacionados con la seguridad social
        public Afiliacion Afiliacion { get; internal set; }
        public int? DiasPendientes { get; internal set; }
        public DateTime FechaCreacion { get; internal set; } = DateTime.Now;
        #endregion

        // Navegación inversa a Perfiles (uno a muchos)
        public ICollection<Perfil> Perfiles { get; internal set; } = new List<Perfil>();

        

        // Constructor privado para forzar el uso del builder
        internal CuentaUsuario() { }

        // Builder interno para la clase
        public class Builder
        {
            private readonly CuentaUsuario _usuario = new CuentaUsuario();

            #region Métodos concatenados para los atributos básicos para un usuario

            public Builder ConId(Guid id)
            {
                _usuario.Id = id;
                return this;
            }
            public Builder ConNombresCompletos(string nombres)
            {
                _usuario.NombresCompletos = nombres;
                return this;
            }

            public Builder ConApellidosCompletos(string apellidos)
            {
                _usuario.ApellidosCompletos = apellidos;
                return this;
            }

            public Builder ConTipoId(TipoId tipoId)
            {
                _usuario.TipoId = tipoId;
                return this;
            }

            public Builder ConIdentificacion(string identificacion)
            {
                _usuario.Identificacion = identificacion;
                return this;
            }
            #endregion

            #region Métodos concatenados para los atributos de ubicación
            public Builder ConDireccion(string direccion)
            {
                _usuario.Direccion = direccion;
                return this;
            }

            public Builder ConCiudad(string ciudad)
            {
                _usuario.Ciudad = ciudad;
                return this;
            }
            public Builder ConTelefono(string telefono)
            {
                _usuario.Telefono = telefono;
                return this;
            }
            public Builder ConEmail(string email)
            {
                _usuario.Email = email;
                return this;
            }
            #endregion

            #region Métodos concatenados para el estado de un usuario
            public Builder ConEsActivo(bool esActivo)
            {
                _usuario.EsActivo = esActivo;
                return this;
            }
            public Builder ConFechaRegistro(DateTime fechaRegistro)
            {
                _usuario.FechaRegistro = fechaRegistro;
                return this;
            }
            public Builder ConFechaInactivacion(DateTime fechaInactivacion)
            {
                _usuario.FechaInactivacion = fechaInactivacion; 
                return this;
            }
            #endregion

            #region Métodos concatenados para la identificación de loggin
            public Builder ConNombreUsuario(string nombreUsuario)
            {
                _usuario.NombreUsuario = nombreUsuario;
                return this;
            }
            public Builder ConPassword(string password)
            {
                _usuario.Password = password;
                return this;
            }
            #endregion
            #region Métodos concatenados para determinar el tipo de usuario
            public Builder ConTipoCuenta(TipoCuenta tipoCuenta)
            {
                _usuario.TipoCuenta = tipoCuenta;
                return this;
            }
             public Builder ConTarjProf(string tarjProf)
            {
                _usuario.TarjProf = tarjProf;
                return this;
            }
            #endregion

            #region Métodos concatenados para el tipo usuario y manejo tributarios
            public Builder ConTipoContrato(TipoContrato tipoContrato)
            {
                _usuario.TipoContrato = tipoContrato;
                return this;
            }
            public Builder ConSujetoRetencion(bool? sujetoRetencion)
            {
                if(_usuario.TipoContrato == TipoContrato.Empleado)
                {
                    _usuario.SujetoRetencion = sujetoRetencion;
                }
                return this; 
            }
            public Builder ConTipoIdTrib(TipoIdTrib? tipoIdTrib)
            {
                if(_usuario.TipoContrato == TipoContrato.PrestadorDeServicios)
                {
                    _usuario.TipoIdTrib = tipoIdTrib;
                }
                return this;
            }
            public Builder ConRazonsocial(string  razonsocial)
            {
                if(_usuario.TipoContrato == TipoContrato.PrestadorDeServicios)
                {
                    _usuario.RazonSocial = razonsocial;
                } 
                return this;
            }
            #endregion
            
            #region Métodos concatenados para los atributos de seguridad social
            public Builder ConAfiliacion(Afiliacion afiliacion)
            {
                _usuario.Afiliacion = afiliacion;
                return this;
            }
            public Builder ConDiasPendientes(int? diasPendientes)
            {
                _usuario.DiasPendientes = diasPendientes;
                return this;
            }
            public Builder ConFechaCreacion(DateTime fechaCreacion)
            {
                _usuario.FechaCreacion = fechaCreacion;
                return this;
            }
            #endregion
            // Constructor final para construir la instancia
            public CuentaUsuario Build()
            {
                
                return _usuario;
            }
           
            
        }
    }
}

