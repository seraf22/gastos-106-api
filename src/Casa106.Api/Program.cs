using Casa106.Infrastructure.Persistence;
using Casa106.Infrastructure.Storage;
using Casa106.Infrastructure.FinancialAI;
using Casa106.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=(localdb)\\mssqllocaldb;Database=Casa106;Trusted_Connection=true;";
builder.Services.AddDbContext<Casa106DbContext>(options =>
    options.UseSqlServer(connectionString));

// Repositories
builder.Services.AddScoped<IMovimientoRepository, MovimientoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IPropiedadRepository, PropiedadRepository>();
builder.Services.AddScoped<IDocumentoRepository, DocumentoRepository>();

// Infrastructure Services
builder.Services.AddScoped<IFinancialDocumentAnalyzer, FakeFinancialDocumentAnalyzer>();
builder.Services.AddScoped<IDocumentStorage>(sp => new LocalDocumentStorage(
    Path.Combine(builder.Environment.ContentRootPath, "uploads")));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Migrate database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Casa106DbContext>();
    db.Database.EnsureCreated(); // Crea la base de datos y tablas si no existen
    await SeedDatabase(db);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();

// Seed Database
static async Task SeedDatabase(Casa106DbContext context)
{
    if (context.Propiedades.Any())
        return; // Ya está seeded

    using var transaction = await context.Database.BeginTransactionAsync();
    try
    {
        // Crear propiedad
        var propiedad = new Casa106.Domain.Entities.Propiedad
        {
            Id = Guid.NewGuid(),
            Nombre = "Casa 106",
            Direccion = "Pucón",
            Unidad = "106",
            Activa = true,
            FechaCreacion = DateTime.UtcNow
        };
        context.Propiedades.Add(propiedad);

        // Crear categorías de ingreso
        var categoriasIngreso = new[]
        {
            new Casa106.Domain.Entities.Categoria { Nombre = "Arriendo Airbnb", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Ingreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Operacional, Orden = 1 },
            new Casa106.Domain.Entities.Categoria { Nombre = "Arriendo directo", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Ingreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Operacional, Orden = 2 },
            new Casa106.Domain.Entities.Categoria { Nombre = "Reembolso", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Ingreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Otro, Orden = 3 },
            new Casa106.Domain.Entities.Categoria { Nombre = "Devolución", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Ingreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Otro, Orden = 4 },
            new Casa106.Domain.Entities.Categoria { Nombre = "Otros ingresos", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Ingreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Otro, Orden = 5 },
        };

        // Crear categorías de egreso
        var categoriasEgreso = new[]
        {
            new Casa106.Domain.Entities.Categoria { Nombre = "Comisión administrador", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Egreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Operacional, Orden = 1 },
            new Casa106.Domain.Entities.Categoria { Nombre = "Comisión Airbnb", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Egreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Operacional, Orden = 2 },
            new Casa106.Domain.Entities.Categoria { Nombre = "Aseo", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Egreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Operacional, Orden = 3 },
            new Casa106.Domain.Entities.Categoria { Nombre = "Gastos comunes", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Egreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Operacional, Orden = 4 },
            new Casa106.Domain.Entities.Categoria { Nombre = "Electricidad", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Egreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Operacional, Orden = 5 },
            new Casa106.Domain.Entities.Categoria { Nombre = "Agua", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Egreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Operacional, Orden = 6 },
            new Casa106.Domain.Entities.Categoria { Nombre = "Internet", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Egreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Operacional, Orden = 7 },
            new Casa106.Domain.Entities.Categoria { Nombre = "Gas", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Egreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Operacional, Orden = 8 },
            new Casa106.Domain.Entities.Categoria { Nombre = "Pellet", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Egreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Operacional, Orden = 9 },
            new Casa106.Domain.Entities.Categoria { Nombre = "Mantenimiento", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Egreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Operacional, Orden = 10 },
            new Casa106.Domain.Entities.Categoria { Nombre = "Reparaciones", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Egreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Operacional, Orden = 11 },
            new Casa106.Domain.Entities.Categoria { Nombre = "Muebles y equipamiento", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Egreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Inversion, Orden = 12 },
            new Casa106.Domain.Entities.Categoria { Nombre = "Contribuciones", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Egreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Impuesto, Orden = 13 },
            new Casa106.Domain.Entities.Categoria { Nombre = "Seguros", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Egreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Financiero, Orden = 14 },
            new Casa106.Domain.Entities.Categoria { Nombre = "Dividendo", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Egreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Financiero, Orden = 15 },
            new Casa106.Domain.Entities.Categoria { Nombre = "Otros egresos", TipoMovimiento = Casa106.Domain.Enumerations.TipoMovimiento.Egreso, Grupo = Casa106.Domain.Enumerations.GrupoCategoria.Otro, Orden = 16 },
        };

        foreach (var cat in categoriasIngreso.Concat(categoriasEgreso))
        {
            cat.Id = Guid.NewGuid();
            cat.Activa = true;
            context.Categorias.Add(cat);
        }

        await context.SaveChangesAsync();

        // Crear datos de demostración
        var catAirbnbIngreso = context.Categorias.First(c => c.Nombre == "Arriendo Airbnb");
        var catComisionAirbnb = context.Categorias.First(c => c.Nombre == "Comisión Airbnb");
        var catComisionAdmin = context.Categorias.First(c => c.Nombre == "Comisión administrador");
        var catElectricidad = context.Categorias.First(c => c.Nombre == "Electricidad");
        var catGastosComunes = context.Categorias.First(c => c.Nombre == "Gastos comunes");
        var catInternet = context.Categorias.First(c => c.Nombre == "Internet");
        var catMuebles = context.Categorias.First(c => c.Nombre == "Muebles y equipamiento");
        var catDividendo = context.Categorias.First(c => c.Nombre == "Dividendo");

        // Movimientos de demo
        var movimientos = new[]
        {
            new Casa106.Domain.Entities.Movimiento
            {
                Id = Guid.NewGuid(),
                PropiedadId = propiedad.Id,
                CategoriaId = catAirbnbIngreso.Id,
                Tipo = Casa106.Domain.Enumerations.TipoMovimiento.Ingreso,
                Estado = Casa106.Domain.Enumerations.EstadoMovimiento.Confirmado,
                Origen = Casa106.Domain.Enumerations.OrigenMovimiento.Manual,
                FechaMovimiento = new DateTime(2026, 7, 15),
                PeriodoDesde = new DateOnly(2026, 7, 1),
                PeriodoHasta = new DateOnly(2026, 7, 31),
                Monto = 540000,
                Descripcion = "Ingresos Airbnb julio",
                Proveedor = "Airbnb",
                FechaCreacion = DateTime.UtcNow
            },
            new Casa106.Domain.Entities.Movimiento
            {
                Id = Guid.NewGuid(),
                PropiedadId = propiedad.Id,
                CategoriaId = catAirbnbIngreso.Id,
                Tipo = Casa106.Domain.Enumerations.TipoMovimiento.Ingreso,
                Estado = Casa106.Domain.Enumerations.EstadoMovimiento.Confirmado,
                Origen = Casa106.Domain.Enumerations.OrigenMovimiento.Manual,
                FechaMovimiento = new DateTime(2026, 8, 15),
                PeriodoDesde = new DateOnly(2026, 8, 1),
                PeriodoHasta = new DateOnly(2026, 8, 31),
                Monto = 720000,
                Descripcion = "Ingresos Airbnb agosto",
                Proveedor = "Airbnb",
                FechaCreacion = DateTime.UtcNow
            },
            new Casa106.Domain.Entities.Movimiento
            {
                Id = Guid.NewGuid(),
                PropiedadId = propiedad.Id,
                CategoriaId = catElectricidad.Id,
                Tipo = Casa106.Domain.Enumerations.TipoMovimiento.Egreso,
                Estado = Casa106.Domain.Enumerations.EstadoMovimiento.Confirmado,
                Origen = Casa106.Domain.Enumerations.OrigenMovimiento.Manual,
                FechaMovimiento = new DateTime(2026, 7, 20),
                PeriodoDesde = new DateOnly(2026, 7, 1),
                PeriodoHasta = new DateOnly(2026, 7, 31),
                Monto = 92300,
                Descripcion = "Cuenta de electricidad julio",
                FechaCreacion = DateTime.UtcNow
            },
            new Casa106.Domain.Entities.Movimiento
            {
                Id = Guid.NewGuid(),
                PropiedadId = propiedad.Id,
                CategoriaId = catGastosComunes.Id,
                Tipo = Casa106.Domain.Enumerations.TipoMovimiento.Egreso,
                Estado = Casa106.Domain.Enumerations.EstadoMovimiento.Confirmado,
                Origen = Casa106.Domain.Enumerations.OrigenMovimiento.Manual,
                FechaMovimiento = new DateTime(2026, 7, 5),
                PeriodoDesde = new DateOnly(2026, 7, 1),
                PeriodoHasta = new DateOnly(2026, 7, 31),
                Monto = 85000,
                Descripcion = "Gastos comunes julio",
                FechaCreacion = DateTime.UtcNow
            },
            new Casa106.Domain.Entities.Movimiento
            {
                Id = Guid.NewGuid(),
                PropiedadId = propiedad.Id,
                CategoriaId = catInternet.Id,
                Tipo = Casa106.Domain.Enumerations.TipoMovimiento.Egreso,
                Estado = Casa106.Domain.Enumerations.EstadoMovimiento.Confirmado,
                Origen = Casa106.Domain.Enumerations.OrigenMovimiento.Manual,
                FechaMovimiento = new DateTime(2026, 7, 1),
                PeriodoDesde = new DateOnly(2026, 7, 1),
                PeriodoHasta = new DateOnly(2026, 7, 31),
                Monto = 25000,
                Descripcion = "Internet julio",
                FechaCreacion = DateTime.UtcNow
            },
            new Casa106.Domain.Entities.Movimiento
            {
                Id = Guid.NewGuid(),
                PropiedadId = propiedad.Id,
                CategoriaId = catComisionAdmin.Id,
                Tipo = Casa106.Domain.Enumerations.TipoMovimiento.Egreso,
                Estado = Casa106.Domain.Enumerations.EstadoMovimiento.Confirmado,
                Origen = Casa106.Domain.Enumerations.OrigenMovimiento.Manual,
                FechaMovimiento = new DateTime(2026, 7, 10),
                PeriodoDesde = new DateOnly(2026, 7, 1),
                PeriodoHasta = new DateOnly(2026, 7, 31),
                Monto = 108000,
                Descripcion = "Comisión administrador julio",
                Proveedor = "Administrador",
                FechaCreacion = DateTime.UtcNow
            },
            new Casa106.Domain.Entities.Movimiento
            {
                Id = Guid.NewGuid(),
                PropiedadId = propiedad.Id,
                CategoriaId = catMuebles.Id,
                Tipo = Casa106.Domain.Enumerations.TipoMovimiento.Egreso,
                Estado = Casa106.Domain.Enumerations.EstadoMovimiento.Confirmado,
                Origen = Casa106.Domain.Enumerations.OrigenMovimiento.Manual,
                FechaMovimiento = new DateTime(2026, 8, 3),
                Monto = 180000,
                Descripcion = "Compra de equipamiento",
                FechaCreacion = DateTime.UtcNow
            },
            new Casa106.Domain.Entities.Movimiento
            {
                Id = Guid.NewGuid(),
                PropiedadId = propiedad.Id,
                CategoriaId = catDividendo.Id,
                Tipo = Casa106.Domain.Enumerations.TipoMovimiento.Egreso,
                Estado = Casa106.Domain.Enumerations.EstadoMovimiento.Confirmado,
                Origen = Casa106.Domain.Enumerations.OrigenMovimiento.Manual,
                FechaMovimiento = new DateTime(2026, 7, 31),
                PeriodoDesde = new DateOnly(2026, 7, 1),
                PeriodoHasta = new DateOnly(2026, 7, 31),
                Monto = 736362,
                Descripcion = "Dividendo julio",
                FechaCreacion = DateTime.UtcNow
            },
        };

        foreach (var mov in movimientos)
            context.Movimientos.Add(mov);

        await context.SaveChangesAsync();
        await transaction.CommitAsync();

        Log.Information("Base de datos seeded correctamente");
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
        Log.Error(ex, "Error al seedear la base de datos");
        throw;
    }
}

