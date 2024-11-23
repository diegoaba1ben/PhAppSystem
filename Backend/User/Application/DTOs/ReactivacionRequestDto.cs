using System;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Application.DTOs
{
   public class ReactivacionRequest
   {
    public string? NumeroSalud {get; set; } // campo opcional para la afiliación de salud
   public string ? NumeroPension {get; set; } // Campo opcional para la afiliación de pension
   }
}