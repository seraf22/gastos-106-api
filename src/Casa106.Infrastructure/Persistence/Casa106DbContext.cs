namespace Casa106.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Casa106.Domain.Entities;

public class Casa106DbContext : DbContext
{
    public Casa106DbContext(DbContextOptions<Casa106DbContext> options) : base(options)
    {
    }

    public DbSet<Propiedad> Propiedades { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Movimiento> Movimientos { get; set; }
    public DbSet<Documento> Documentos { get; set; }
    public DbSet<ProcesamientoIA> ProcesaminetosIA { get; set; }
    public DbSet<OcupacionMensual> Ocupaciones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Propiedad
        modelBuilder.Entity<Propiedad>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<Propiedad>()
            .Property(p => p.Nombre)
            .IsRequired()
            .HasMaxLength(200);
        modelBuilder.Entity<Propiedad>()
            .HasIndex(p => p.Activa);

        // Categoria
        modelBuilder.Entity<Categoria>()
            .HasKey(c => c.Id);
        modelBuilder.Entity<Categoria>()
            .Property(c => c.Nombre)
            .IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<Categoria>()
            .HasIndex(c => new { c.TipoMovimiento, c.Activa });

        // Movimiento
        modelBuilder.Entity<Movimiento>()
            .HasKey(m => m.Id);
        modelBuilder.Entity<Movimiento>()
            .Property(m => m.Monto)
            .HasPrecision(18, 2);
        modelBuilder.Entity<Movimiento>()
            .Property(m => m.Descripcion)
            .IsRequired()
            .HasMaxLength(500);
        modelBuilder.Entity<Movimiento>()
            .HasOne(m => m.Propiedad)
            .WithMany(p => p.Movimientos)
            .HasForeignKey(m => m.PropiedadId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Movimiento>()
            .HasOne(m => m.Categoria)
            .WithMany(c => c.Movimientos)
            .HasForeignKey(m => m.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Movimiento>()
            .HasOne(m => m.Documento)
            .WithMany(d => d.Movimientos)
            .HasForeignKey(m => m.DocumentoId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Movimiento>()
            .HasIndex(m => new { m.PropiedadId, m.FechaMovimiento });
        modelBuilder.Entity<Movimiento>()
            .HasIndex(m => new { m.PropiedadId, m.Estado });
        modelBuilder.Entity<Movimiento>()
            .HasIndex(m => m.CategoriaId);

        // Documento
        modelBuilder.Entity<Documento>()
            .HasKey(d => d.Id);
        modelBuilder.Entity<Documento>()
            .Property(d => d.NombreOriginal)
            .IsRequired()
            .HasMaxLength(256);
        modelBuilder.Entity<Documento>()
            .Property(d => d.NombreAlmacenado)
            .IsRequired()
            .HasMaxLength(256);
        modelBuilder.Entity<Documento>()
            .Property(d => d.HashArchivo)
            .IsRequired()
            .HasMaxLength(64);
        modelBuilder.Entity<Documento>()
            .HasOne(d => d.Propiedad)
            .WithMany(p => p.Documentos)
            .HasForeignKey(d => d.PropiedadId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Documento>()
            .HasIndex(d => d.HashArchivo)
            .IsUnique();
        modelBuilder.Entity<Documento>()
            .HasIndex(d => d.PropiedadId);

        // ProcesamientoIA
        modelBuilder.Entity<ProcesamientoIA>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<ProcesamientoIA>()
            .HasOne(p => p.Documento)
            .WithMany(d => d.Procesamientos)
            .HasForeignKey(p => p.DocumentoId)
            .OnDelete(DeleteBehavior.Cascade);

        // OcupacionMensual
        modelBuilder.Entity<OcupacionMensual>()
            .HasKey(o => o.Id);
        modelBuilder.Entity<OcupacionMensual>()
            .HasOne(o => o.Propiedad)
            .WithMany(p => p.Ocupaciones)
            .HasForeignKey(o => o.PropiedadId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<OcupacionMensual>()
            .HasIndex(o => new { o.PropiedadId, o.Anio, o.Mes })
            .IsUnique();
        modelBuilder.Entity<OcupacionMensual>()
            .Property(o => o.IngresosAlojamiento)
            .HasPrecision(18, 2);
        modelBuilder.Entity<OcupacionMensual>()
            .Property(o => o.TarifaPromedio)
            .HasPrecision(18, 2);
    }
}
