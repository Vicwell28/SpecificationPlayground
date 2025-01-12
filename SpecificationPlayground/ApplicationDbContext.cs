using Microsoft.EntityFrameworkCore;

namespace SpecificationPlayground
{
    /// <summary>
    /// Contexto de la base de datos para la aplicación.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Producto> Productos { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Configuraciones adicionales si son necesarias
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones de entidades
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.Categoria)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(e => e.Precio)
                      .HasColumnType("decimal(18,2)");
                entity.Property(e => e.EnStock)
                      .IsRequired();
            });
        }
    }
}