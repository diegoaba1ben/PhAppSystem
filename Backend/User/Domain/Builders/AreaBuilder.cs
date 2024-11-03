using System;
using System.Collections.Generic;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Domain.Builders
{
    /// <summary>
    /// Builder para crear instancias de Area utilizando el patrón Fluent Builder
    /// </summary>
    public class AreaBuilder
    {
        private readonly Area _area;

        public AreaBuilder()
        {
            _area = new Area
            {
                Id = Guid.NewGuid()
            };
        }

        public AreaBuilder ConNombre(string nombre)
        {
            _area.Nombre = nombre;
            return this;
        }

        public AreaBuilder ConRoles(IEnumerable<Rol> roles)
        {
            _area.Roles.AddRange(roles);
            return this;
        }

        public Area Build()
        {
            if (string.IsNullOrWhiteSpace(_area.Nombre))
                throw new InvalidOperationException("El nombre del área no puede estar vacío.");

            return _area;
        }
    }
}
