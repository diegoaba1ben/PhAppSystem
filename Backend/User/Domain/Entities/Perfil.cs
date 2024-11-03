using System;
using System.Collections.Generic;
using PhAppUser.Domain.Builders;

namespace PhAppUser.Domain.Entities
{
    public class Perfil
    {
        public Guid Id { get; set; }

        // Navegación a CuentaUsuarios (relación muchos a muchos)
        public ICollection<CuentaUsuario> CuentaUsuarios { get; set; } = new List<CuentaUsuario>();
        
        // Navegación a roles (relación muchos a muchos)
        public ICollection<Rol> Roles { get; set; } = new List<Rol>();

        // Relación uno a muchos con Area
        public Guid AreaId { get; set; }
        public Area? Area { get; set; }
        
        // Constructor con parámetros para el Builder
        public Perfil(Guid usuarioId, Area area, ICollection<Rol> roles)
        {
            Id = Guid.NewGuid();
            Area = area;
            AreaId = area.Id;
            Roles = new List<Rol>();
        }

        // Constructor privado para forzar el uso del builder
        internal Perfil()
        {
            Id = Guid.NewGuid();
        }

        // Método estático para iniciar el Builder
        public static PerfilBuilder CrearBuilder()
        {
            return new PerfilBuilder();
        }

        // Clase interna para el builder de Perfil
        public class PerfilBuilder
        {
            private readonly Perfil _perfil = new Perfil();

            public PerfilBuilder ConCuentaUsuarios(ICollection<CuentaUsuario> cuentaUsuarios)
            {
                _perfil.CuentaUsuarios = cuentaUsuarios;
                return this;
            }

            public PerfilBuilder ConRoles(ICollection<Rol> roles)
            {
                _perfil.Roles = roles;
                return this;
            }
            public PerfilBuilder ConArea(Area area)
            {
                _perfil.Area = area;
                _perfil.AreaId = area.Id;
                return this;
            }

            // Método para construir el objeto Perfil
            public Perfil Build()
            {
                return _perfil;
            }
        }
    }
}

