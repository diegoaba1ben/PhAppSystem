using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;
using PhAppUser.Application.DTOs;
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

        /// <summary>
        /// Inactiva al usuario y actualiza sus afiliaciones asociadas (Salud y Pensión).
        /// </summary>
        /// <param name="fechaInactivación">Fecha de inactivación del usuario.</param>
        public void InactivarUsuario(DateTime fechaInactivación)
        {
            if (!EsActivo)
                throw new InvalidOperationException("El usuario ya está inactivo.");

            FechaInactivacion = fechaInactivación;
            EsActivo = false;

            // Propagar el estado a las afiliaciones asociadas
            Salud?.ActualizarEstado(false);
            Pension?.ActualizarEstado(false);
        }

        /// <summary>
        /// Reactiva al usuario y actualiza sus afiliaciones asociadas (Salud y Pensión).
        /// </summary>
        public void ReactivarUsuario()
        {
            if (EsActivo)
                throw new InvalidOperationException("El usuario ya está activo.");

            FechaInactivacion = null; // Se elimina la fecha de inactivación
            EsActivo = true;

            // Propagar el estado a las afiliaciones asociadas
            Salud?.ActualizarEstado(true);
            Pension?.ActualizarEstado(true);
        }
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

        public int Intento { get; internal set; } = 0;
        public bool Bloqueado { get; internal set; } = false;
        public Salud? Salud { get; internal set; }
        public Pension? Pension { get; internal set; }
        #endregion

        #region Manejo de relaciones
        public ICollection<Perfil> Perfiles { get; internal set; } = new List<Perfil>();
        public RepLegal? RepLegal { get; internal set; }
        #endregion

        internal CuentaUsuario() { }

        public class Builder
        {
            private readonly CuentaUsuario _usuario = new CuentaUsuario();

            #region Métodos concatenados
            public Builder GenerarNuevoId() { _usuario.Id = Guid.NewGuid(); return this; }
            public Builder ConNombresCompletos(string nombres) { _usuario.NombresCompletos = nombres; return this; }
            public Builder ConAfiliacion(Afiliacion afiliacion, int? diasPendientes = null)
            {
                _usuario.Afiliacion = afiliacion;
                _usuario.DiasPendientes = afiliacion == Afiliacion.Parcial ? diasPendientes : null;
                return this;
            }
            public Builder ConSalud(Salud? salud)
            {
                _usuario.Salud = salud;
                return this;
            }
            public Builder ConPension(Pension? pension)
            {
                _usuario.Pension = pension;
                return this;
            }
            public Builder ConInactivarUsuario(DateTime fechaInactivacion)
            {
                _usuario.InactivarUsuario(fechaInactivacion);
                return this;
            }
            public Builder ConReactivarUsuario()
            {
                _usuario.ReactivarUsuario();
                return this;
            }

            #endregion

            public CuentaUsuario Build()
            {
                ValidarConsistenciaAfiliacion();
                ValidarEstado();
                return _usuario;
            }

            #region Métodos de validación privados
            private void ValidarConsistenciaAfiliacion()
            {
                if (_usuario.Afiliacion == Afiliacion.Completa)
                {
                    if (_usuario.Salud == null || _usuario.Pension == null)
                        throw new InvalidOperationException("Para afiliación completa, se requieren Salud y Pensión.");
                }

                if (_usuario.Afiliacion == Afiliacion.Parcial && (!_usuario.DiasPendientes.HasValue || _usuario.DiasPendientes <= 0))
                    throw new InvalidOperationException("Para afiliación parcial, se debe definir un plazo mayor a 0 días.");
            }

            private void ValidarEstado()
            {
                if (_usuario.Bloqueado && _usuario.EsActivo)
                    throw new InvalidOperationException("Un usuario bloqueado no puede estar activo.");
            }
            #endregion
        }
    }
}