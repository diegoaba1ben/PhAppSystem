using System;
using PhAppUser.Application.DTOs;

namespace PhAppUser.Application.DTOs
{
    public class PermisoDto
    {
        public Guid Id { get; set;}
        public string Codigo { get; set;} = string.Empty;
        public string Nombre { get; set;} = string.Empty;
        public string Descripcion { get; set;} = string.Empty;
        public DateTime FechaCreacion { get; set;} = DateTime.Now;
    }
}