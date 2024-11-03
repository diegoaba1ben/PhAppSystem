using System;
using PhAppUser .Domain.Entities;

namespace PhAppUser .Domain.Builders
{
    /// <summary>
    /// Builder para crear instancias de Permiso utilizando el patrón Fluent Builder
    /// </summary>
    public class PermisoBuilder
    {
        private readonly Permiso _permiso;

        public PermisoBuilder()
        {
            _permiso = new Permiso(); // Llama al constructor privado
        }

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
            // Validaciones antes de construir el objeto
            if (string.IsNullOrWhiteSpace(_permiso.Codigo))
                throw new InvalidOperationException("El código no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(_permiso.Nombre))
                throw new InvalidOperationException("El nombre no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(_permiso.Descripcion))
                throw new InvalidOperationException("La descripción no puede estar vacía.");

            // Asignar valores a propiedades de solo lectura
            _permiso.Id = Guid.NewGuid(); // Asignar un nuevo ID
            _permiso.FechaCreacion = DateTime.UtcNow; // Asignar la fecha de creación

            return _permiso;
        }
    }
}
