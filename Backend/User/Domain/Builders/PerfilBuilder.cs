using System;
using System;
using System.Collections.Generic;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Domain.Builders
{
    /// <summary>
    /// Builder para crear instancias de Perfil utilizando el patrón Fluent Builder
    /// </summary>
    public class PerfilBuilder
    {
        private Guid _usuarioId;
        private Area? _area; // Permitir nulos temporalmente hasta que se configure
        private List<Rol> _roles = new List<Rol>();

        public PerfilBuilder ConUsuarioId(Guid usuarioId)
        {
            _usuarioId = usuarioId;
            return this;
        }

        public PerfilBuilder ConArea(Area area)
        {
            _area = area;
            return this;
        }

        public PerfilBuilder ConRol(Rol rol)
        {
            _roles.Add(rol);
            return this;
        }

        public PerfilBuilder ConRoles(IEnumerable<Rol> roles)
        {
            _roles.AddRange(roles);
            return this;
        }

        public Perfil Build()
        {
            if (_usuarioId == Guid.Empty)
                throw new InvalidOperationException("El perfil debe tener un usuario asignado.");

            if (_area == null)
                throw new InvalidOperationException("El perfil debe estar asociado a un área.");

            if (_roles.Count == 0)
                throw new InvalidOperationException("El perfil debe tener al menos un rol asignado.");

            return new Perfil(_usuarioId, _area, _roles);
        }
    }
}
