using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhAppUser.Application.DTOs;
using PhAppUser.Domain.Enums;
using PhAppUser.Infrastructure.Context;

namespace PhAppUser.Application.Queries
{
    /// <summary>
    /// Clase para gestionar auditorías relacionadas con los usuarios.
    /// </summary>
    public class AuditoriaQuery
    {
        private readonly PhAppUserDbContext _context;
        private readonly IMapper _mapper;

        public AuditoriaQuery(PhAppUserDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene usuarios con intentos de aplazamiento excedidos o bloqueados.
        /// </summary>
        /// <returns>Lista de usuarios con problemas en la afiliación.</returns>
        public async Task<List<AdvancedDto.AdvancedUserDto>> ObtUsuariosConProblemasDeAfiliacionAsync()
        {
            var query = _context.CuentasUsuarios
                .Where(cu =>
                    cu.Intento >= 2 || // Intentos excedidos
                    cu.Bloqueado // Usuarios bloqueados
                )
                .Include(cu => cu.Perfiles) // Incluir perfiles
                    .ThenInclude(p => p.Roles)
                        .ThenInclude(r => r.Permisos); // Incluir roles y permisos

            return await _mapper.ProjectTo<AdvancedDto.AdvancedUserDto>(query).ToListAsync();
        }

        /// <summary>
        /// Obtiene usuarios que no han cumplido los requisitos de afiliación.
        /// </summary>
        /// <param name="fechaCorte">Fecha límite para evaluar las afiliaciones.</param>
        /// <returns>Lista de usuarios con afiliación parcial y días vencidos.</returns>
        public async Task<List<AdvancedDto.AdvancedUserDto>> ObtUsuariosConAfiliacionParcialAsync(DateTime fechaCorte)
        {
            var query = _context.CuentasUsuarios
                .Where(cu =>
                    cu.Afiliacion == Afiliacion.Parcial && // Estado parcial
                    cu.DiasPendientes > 0 && // Días pendientes
                    cu.FechaCreacion.AddDays(cu.DiasPendientes ?? 0) <= fechaCorte // Fecha límite superada
                )
                .Include(cu => cu.Perfiles) // Incluir perfiles
                    .ThenInclude(p => p.Roles)
                        .ThenInclude(r => r.Permisos); // Incluir roles y permisos

            return await _mapper.ProjectTo<AdvancedDto.AdvancedUserDto>(query).ToListAsync();
        }
    }
}
