using Microsoft.EntityFrameworkCore;
using PdfProcessingApi.CQRS.Queries;
using PdfProcessingApi.Models;

namespace PdfProcessingApi
{
 

    public class EvaluacionDeClientesDbContext : DbContext
    {
        public EvaluacionDeClientesDbContext(DbContextOptions<EvaluacionDeClientesDbContext> options)
            : base(options)
        {
        }

        public DbSet<PdfExtracData> PdfExtracData { get; set; }

        // En tu DbContext (EvaluacionDeClientesDbContext.cs)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PdfExtracData>(entity =>
            {
                // Clave primaria compuesta
                entity.HasKey(e => new { e.UID, e.idFacultad });

                // Configuración de columnas
                entity.Property(e => e.UID)
                    .HasMaxLength(36)
                    .IsRequired();

                entity.Property(e => e.idFacultad)
                    .HasMaxLength(10)
                    .IsRequired();

                entity.Property(e => e.facultad)
                    .HasMaxLength(255)
                    .IsRequired();
            });
        }



      

        // ... (el resto del código existente se mantiene igual)
    }
}


