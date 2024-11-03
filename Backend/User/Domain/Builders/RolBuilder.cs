using System;
using System.Collections.Generic;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Domain.Builders
{
    /// <summary>
    /// Builder para crear instancias de Rol utilizando el patrón Fluent Builder
    /// </summary>
    public class RolBuilder
    {
        private readonly Rol _rol;

        public RolBuilder()
        {
            _rol = new Rol
            {
                Id = Guid.NewGuid(),
                FechaCreacion = DateTime.UtcNow
            };
        }

        public RolBuilder ConNombre(string nombre)
        {
            _rol.Nombre = nombre;
            return this;
        }

        public RolBuilder ConDescripcion(string descripcion)
        {
            _rol.Descripcion = descripcion;
            return this;
        }

        public RolBuilder ConPermiso(Permiso permiso)
        {
            _rol.Permisos.Add(permiso);
            return this;
        }

        public RolBuilder ConPermisos(IEnumerable<Permiso> permisos)
        {
            _rol.Permisos.AddRange(permisos);
            return this;
        }

        public Rol Build()
        {
            if (string.IsNullOrWhiteSpace(_rol.Nombre))
                throw new InvalidOperationException("El nombre del rol no puede estar vacío.");

            if (_rol.Permisos.Count == 0)
                throw new InvalidOperationException("El rol debe tener al menos un permiso asignado.");

            return _rol;
        }
    }
}
