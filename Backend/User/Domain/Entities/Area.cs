using System;
using System.Collections.Generic;

namespace PhAppUser.Domain.Entities
{
    /// <summary>
    /// Clase que representa las áreas funcionales de la organización, agrupando roles relacionados.
    /// </summary>
    public class Area
    {
        public Guid Id { get; internal set; } // Identificador único del área
        public string Nombre { get; internal set; } = string.Empty; // Nombre del área
        public List<Rol> Roles { get; internal set; } = new List<Rol>(); // Roles que pertenecen a un área específica

        // Constructor privado para forzar el uso del builder
        internal Area()
        {
            Id = Guid.NewGuid();
        }

        // Método estático para inicializar el builder
        public static AreaBuilder Crear()
        {
            return new AreaBuilder();
        }

        // Clase interna para el builder de Area
        public class AreaBuilder
        {
            private readonly Area _area = new Area();

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
}
