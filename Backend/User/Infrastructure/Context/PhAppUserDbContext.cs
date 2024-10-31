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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeo de tablas con nombres específicos
            modelBuilder.Entity<CuentaUsuario>().ToTable("CuentaUsuario");
            modelBuilder.Entity<Salud>().ToTable("Salud");
            modelBuilder.Entity<Pension>().ToTable("Pension");
            modelBuilder.Entity<RepLegal>().ToTable("RepLegal");

            // Configuraciones de relaciones personalizadas
            ConfigureRelationships(modelBuilder);
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // Definir relaciones entre entidades aquí (si las hay)
        }

        // Métodos adicionales (si es necesario)
    }
}

