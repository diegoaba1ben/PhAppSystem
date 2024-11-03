using System;

namespace PhAppUser.Domain.Entities
{
    /// <summary>
    /// Clase que representa los permisos asignables a roles en el sistema
    /// </summary>
    public class Permiso
    {
        public Guid Id { get; internal set; } // Identificador único del permiso
        public string Codigo { get; internal set; } = string.Empty; // Código corto del permiso para identificación visual
        public string Nombre { get; internal set; } = string.Empty; // Nombre del permiso
        public string Descripcion { get; internal set; } = string.Empty; // Descripción del permiso
        public DateTime FechaCreacion { get; internal set; } // Fecha de creación del permiso
        // Navegación a muchos roles (muchos a muchos)
        public ICollection<Rol> Roles { get; internal set; } = new List<Rol>();

        // Constructor privado para iniciar el Builder interno
        internal Permiso()
        {
            Id = Guid.NewGuid();
            FechaCreacion = DateTime.UtcNow;
        }

        // Método estático para iniciar el Builder
        public static PermisoBuilder CrearBuilder()
        {
            return new PermisoBuilder();
        }

        // Clase interna para el Builder de Permiso
        public class PermisoBuilder
        {
            private readonly Permiso _permiso = new Permiso();

            public PermisoBuilder ConCodigo(string codigo)
            {
                _permiso.Codigo = codigo;
                return this;
            }

            public PermisoBuilder ConNombre(string nombre)
            {
                _permiso.Nombre = nombre;
                return this;
            }

            public PermisoBuilder ConDescripcion(string descripcion)
            {
                _permiso.Descripcion = descripcion;
                return this;
            }

            public Permiso Build()
            {
                return _permiso;
            }
        }
    }
}
