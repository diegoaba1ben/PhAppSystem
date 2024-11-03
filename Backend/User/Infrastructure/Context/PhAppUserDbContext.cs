using Microsoft.EntityFrameworkCore;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Infrastructure.Context
{
    public class PhAppUserDbContext : DbContext
    {
        public PhAppUserDbContext(DbContextOptions<PhAppUserDbContext> options) : base(options) { }

        // Definición de DbSets
        public DbSet<CuentaUsuario> CuentasUsuarios { get; set; }
        public DbSet<Salud> Saluds { get; set; }
        public DbSet<Pension> Pensiones { get; set; }
        public DbSet<RepLegal> RepLegals{ get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Area> Areas{ get; set; }
        public DbSet<Perfil> Perfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeo de tablas con nombres específicos
            modelBuilder.Entity<CuentaUsuario>().ToTable("CuentaUsuario");
            modelBuilder.Entity<Salud>().ToTable("Salud");
            modelBuilder.Entity<Pension>().ToTable("Pension");
            modelBuilder.Entity<RepLegal>().ToTable("RepLegal");
            modelBuilder.Entity<Permiso>().ToTable("Permiso");
            modelBuilder.Entity<Rol>().ToTable("Rol");
            modelBuilder.Entity<Area>().ToTable("Area");
            modelBuilder.Entity<Perfil>().ToTable("Perfil");

            // Configuraciones de relaciones personalizadas
            ConfigureRelationships(modelBuilder);
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // Relación muchos a muchos entre CuentaUsuario y Perfil
            modelBuilder.Entity<CuentaUsuario>()
                .HasMany(cu => cu.Perfiles)
                .WithMany(p => p.CuentaUsuarios)
                .UsingEntity<Dictionary<string, object>>(
                    "CuentaUsuarioPerfil",
                    j => j.HasOne<Perfil>().WithMany().HasForeignKey("PerfilId"),
                    j => j.HasOne<CuentaUsuario>().WithMany().HasForeignKey("CuentaUsuarioId")
                );

            // Relación muchos a muchos entre Perfil y Rol
            modelBuilder.Entity<Perfil>()
                .HasMany(p => p.Roles)
                .WithMany(r => r.Perfiles)
                .UsingEntity<Dictionary<string, object>>(
                    "PerfilRol",
                    j => j.HasOne<Rol>().WithMany().HasForeignKey("RolId"),
                    j => j.HasOne<Perfil>().WithMany().HasForeignKey("PerfilId")
                );

            // Relación uno a muchos entre Area y Rol
            modelBuilder.Entity<Area>()
                .HasMany(a => a.Roles)
                .WithOne(r => r.Area)
                .HasForeignKey(r => r.AreaId);

            // Relación uno a muchos entre Perfil y Area
            modelBuilder.Entity<Perfil>()
                .HasOne(p => p.Area)
                .WithMany()
                .HasForeignKey(p => p.AreaId);
        }        // Métodos adicionales (si es necesario)
    }
}

