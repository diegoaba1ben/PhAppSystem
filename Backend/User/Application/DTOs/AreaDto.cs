using System;
using PhAppUser.Application.DTOs;

namespace PhAppUser.Aplication.DTOs
{
    public class AreaDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}