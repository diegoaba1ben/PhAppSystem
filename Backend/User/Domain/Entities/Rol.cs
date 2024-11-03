using System;
using System.Collections;
using System.Collections.Generic;

namespace PhAppUser.Domain.Entities
{
    /// <summary>
    /// Clase que representa los roles asignados a usuarios en el sistema.
    /// </summary>
    public class Rol
    {
        public Guid Id { get; internal set; } // Identificador único del rol
        public string Nombre { get; internal set; } = string.Empty; // Nombre del rol
        public string Descripcion { get; internal set; } = string.Empty; // Descripción del rol
        public DateTime FechaCreacion { get; internal set; } // Fecha de creación del rol

        // Navegación a Perfiles (Relación muchos a muchos)
        public ICollection<Perfil> Perfiles { get; set; } = new List<Perfil>();

        // Navegación a Permisos (Relación muchos a muchos)
        public List<Permiso> Permisos{ get; set; } = new List<Permiso>();

        // Clave foránea y navegación a Area (relación uno a muchos)
        public Guid AreaId { get; internal set; }
        public Area? Area { get; internal set; }

        // Constructor privado para forzar el uso del builder
        internal Rol()
        {
            Id = Guid.NewGuid();
            FechaCreacion = DateTime.UtcNow;
        }

        // Método estático para iniciar el builder
        public static RolBuilder CrearBuilder()
        {
            return new RolBuilder();
        }

        // Clase interna para el builder de Rol
        public class RolBuilder
        {
            private readonly Rol _rol = new Rol();

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

            public RolBuilder ConPermisos(IEnumerable<Permiso> permisos)
            {
                _rol.Permisos = new List<Permiso>(permisos);
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
}
