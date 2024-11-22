using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PhAppUser.Infrastructure.Context;
using PhAppUser.Application.DTOs;
using PhAppUser.Domain.Enums;

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
        public async Task<List<AdvancedDto.AdvancedUserDto>> ObtUsuariosAvanzadosAsync(
            string? nombre = null,
            string? apellido = null,
            string? identificacion = null)
        {
            if (string.IsNullOrWhiteSpace(nombre) && string.IsNullOrWhiteSpace(apellido) && string.IsNullOrWhiteSpace(identificacion))
            {
                throw new ArgumentException("Debe proporcionar al menos uno de los datos requeridos.");
            }

            var query = _context.CuentasUsuarios.AsQueryable();

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

            query = query.Include(cu => cu.Perfiles)
                         .ThenInclude(p => p.Roles)
                         .ThenInclude(r => r.Permisos);

            return await _mapper.ProjectTo<AdvancedDto.AdvancedUserDto>(query).ToListAsync();
        }

        /// <summary>
        /// Obtiene usuarios activos con sus perfiles y permisos.
        /// </summary>
        public async Task<List<AdvancedDto.AdvancedUserDto>> ObtUsuariosActivosAsync()
        {
            var query = _context.CuentasUsuarios
                .Where(cu => cu.EsActivo)
                .Include(cu => cu.Perfiles)
                    .ThenInclude(p => p.Roles)
                        .ThenInclude(r => r.Permisos);

            return await _mapper.ProjectTo<AdvancedDto.AdvancedUserDto>(query).ToListAsync();
        }

        /// <summary>
        /// Obtiene todos los datos de un usuario específico por su identificación.
        /// </summary>
        /// <param name="identificacion">Número de identificación del usuario.</param>
        /// <returns>Información completa del usuario.</returns>
        public async Task<AdvancedDto.AdvancedUserDto?> ObtUsuarioPorIdentificacionAsync(string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                throw new ArgumentException("Debe proporcionar un número de identificación válido.");
            }

            var query = _context.CuentasUsuarios
                .Where(cu => cu.Identificacion == identificacion)
                .Include(cu => cu.Perfiles)
                    .ThenInclude(p => p.Roles)
                        .ThenInclude(r => r.Permisos);

            return await _mapper.ProjectTo<AdvancedDto.AdvancedUserDto>(query).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Obteniene información adicional sobre los usuarios que tienen un perfil específico.
        /// </summary>
        public async Task<List<AdvancedDto.AdvancedUserDto>> ObtUsuariosEspecialesAsync()
        {
            // Filtrar usuarios con roles de Representante Legal o Contador
            var query = _context.CuentasUsuarios
                .Where(cu => cu.Perfiles.Any(p => p.Roles.Any(r => r.Nombre == "Representante Legal" || r.Nombre == "Contador")))
                .Include(cu => cu.Perfiles)
                    .ThenInclude(p => p.Roles)
                        .ThenInclude(r => r.Permisos);

            // Mapear las entidades hacia el DTO usando AutoMapper
            return await _mapper.ProjectTo<AdvancedDto.AdvancedUserDto>(query).ToListAsync();
        }
        /// <summary>
        /// Obtiene una lista de usuarios con información sobre su estado de afiliación a seguridad social
        /// </summary>
        public async Task<List<AfiliacionSSDto>> ObtAfiliacionesSegSUsuariosAsync()
        {
            // Inicializar la consulta base para usuarios
            var query = from usuario in _context.CuentasUsuarios
                        join salud in _context.Saluds on usuario.Id equals salud.Id into saludJoin
                        from saludData in saludJoin.DefaultIfEmpty() // Permitir nulos
                        join pension in _context.Pensiones on usuario.Id equals pension.Id into pensionJoin
                        from pensionData in pensionJoin.DefaultIfEmpty() // Permitir nulos
                        select new AfiliacionSSDto
                        {
                            UsuarioId = usuario.Id,
                            NombresCompletos = usuario.NombresCompletos,
                            ApellidosCompletos = usuario.ApellidosCompletos,
                            Identificacion = usuario.Identificacion,
                            Afiliacion = usuario.Afiliacion.ToString(),
                            DiasPendientes = usuario.Afiliacion == Afiliacion.Parcial ? usuario.DiasPendientes : null,
                            Salud = saludData != null ? new SaludDto
                            {
                                RazonSocialSalud = saludData.RazonSocialSalud,
                            } : null,
                            Pension = pensionData != null ? new PensionDto
                            {
                                RazonSocialPension = pensionData.RazonSocialPension,
                            } : null
                        };
            return await query.ToListAsync();
        }
        /// <summary>
        /// Obtiene usuarios inactivos con información detallada.
        /// </summary>
        /// <returns>Lista de usuarios inactivos.</returns>
        public async Task<List<AdvancedDto.AdvancedUserDto>> ObtUsuariosInactivosAsync()
        {
            // Consulta usuarios marcados como inactivos
            var query = _context.CuentasUsuarios
                .Where(cu => !cu.EsActivo) // Filtra usuarios inactivos
                .Include(cu => cu.Perfiles)
                    .ThenInclude(p => p.Roles)
                        .ThenInclude(r => r.Permisos); // Incluye perfiles, roles y permisos relacionados

            // Mapea las entidades hacia el DTO usando AutoMapper
            return await _mapper.ProjectTo<AdvancedDto.AdvancedUserDto>(query).ToListAsync();
        }
        /// <summary>
        /// Obtiene usuarios que no tienen afiliación completa (Afiliacion == Parcial).
        /// </summary>
        /// <param name="fechaCorte">Fecha límite para evaluar las afiliaciones.</param>
        /// <returns>Lista de usuarios con afiliación parcial.</returns>
        public async Task<List<AdvancedDto.AdvancedUserDto>> ObtUsuariosSinAfiliacionAsync(DateTime fechaCorte)
        {
            // Filtrar usuarios con Afiliacion Parcial y días pendientes
            var query = _context.CuentasUsuarios
                .Where(cu =>
                    cu.Afiliacion == Afiliacion.Parcial && // Estado parcial
                    cu.DiasPendientes > 0 && // Días pendientes
                    cu.FechaCreacion.AddDays(cu.DiasPendientes ?? 0) <= fechaCorte // Fecha límite no superada
                )
                .Include(cu => cu.Perfiles) // Incluir perfiles
                    .ThenInclude(p => p.Roles)
                        .ThenInclude(r => r.Permisos); // Incluir roles y permisos

            // Mapear a AdvancedUserDto usando AutoMapper
            return await _mapper.ProjectTo<AdvancedDto.AdvancedUserDto>(query).ToListAsync();
        }
    }
}


