using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PhAppUser.Infrastructure.Context;
using PhAppUser.Application.DTOs;

namespace PhAppUser.Application.Queries
{
    /// <summary>
    /// Clase para gestionar consultas avanzadas de usuarios.
    /// </summary>
    public class AdvancedQuery
    {
        private readonly PhAppUserDbContext _context;
        private readonly IMapper _mapper;

        public AdvancedQuery(PhAppUserDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene una lista de usuarios con perfiles y roles asociados.
        /// </summary>
        /// <returns>Lista de usuarios con perfiles y roles asociados.</returns>
        public async Task<List<AdvancedDto.AdvancedUserDto>> ObtUsuariosAvanzadosAsync(
            string? nombre = null,
            string? apellido = null,
            string? identificacion = null)
        {
            // Validar que al menos uno de los parámetros esté diligenciado
            if (string.IsNullOrWhiteSpace(nombre) && string.IsNullOrWhiteSpace(apellido) && string.IsNullOrWhiteSpace(identificacion))
            {
                throw new ArgumentException("Debe proporcionar al menos uno de los datos requeridos.");
            }

            // Inicializar la consulta base
            var query = _context.CuentasUsuarios.AsQueryable();

            // Aplicar filtros opcionales
            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(cu => cu.NombresCompletos.Contains(nombre));
            }
            if (!string.IsNullOrEmpty(apellido))
            {
                query = query.Where(cu => cu.ApellidosCompletos.Contains(apellido));
            }
            if (!string.IsNullOrEmpty(identificacion))
            {
                query = query.Where(cu => cu.Identificacion == identificacion);
            }

            // Incluir relaciones necesarias
            query = query.Include(cu => cu.Perfiles)
                         .ThenInclude(p => p.Roles)
                         .ThenInclude(r => r.Permisos);

            // Mapear las entidades hacia el DTO usando AutoMapper
            return await _mapper.ProjectTo<AdvancedDto.AdvancedUserDto>(query).ToListAsync();
        }

        /// <summary>
        /// Obtiene usuarios activos con sus perfiles y permisos.
        /// </summary>
        /// <returns>Lista de usuarios activos con sus perfiles y permisos.</returns>
        public async Task<List<AdvancedDto.AdvancedUserDto>> ObtUsuariosActivosAsync()
        {
            // Inicialización de la consulta base con el filtro para usuarios activos
            var query = _context.CuentasUsuarios
                .Where(cu => cu.EsActivo)
                .Include(cu => cu.Perfiles)
                    .ThenInclude(p => p.Roles)
                        .ThenInclude(r => r.Permisos);

            // Mapear las entidades hacia el DTO usando AutoMapper
            return await _mapper.ProjectTo<AdvancedDto.AdvancedUserDto>(query).ToListAsync();
        }
    }
}

