using Microsoft.EntityFrameworkCore;

namespace Reservas.Infrastructure.Persistencia
{
    public sealed class ReservasDbContext(DbContextOptions<ReservasDbContext> options)
        : DbContext(options)
    {

        public DbSet<ReservaEntity> Reservas => Set<ReservaEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReservaEntity>(entidad =>
            {
                entidad.ToTable("Reservas");

                entidad.HasKey(r => r.Id);

                entidad.Property(r => r.SalaId)
                       .IsRequired();

                entidad.Property(r => r.Inicio)
                       .IsRequired();

                entidad.Property(r => r.Fin)
                       .IsRequired();

                entidad.HasIndex(r => new { r.SalaId, r.Inicio })
                       .HasDatabaseName("IX_Reservas_SalaId_Inicio");
            });
        }
    }
}
