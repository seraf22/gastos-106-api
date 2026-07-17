using Casa106.Infrastructure.Persistence;
using Casa106.Infrastructure.Storage;
using Casa106.Infrastructure.FinancialAI;
using Casa106.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins(
                "https://seraf22.github.io",
                "http://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Falta 'ConnectionStrings:DefaultConnection' para PostgreSQL.");
}

builder.Services.AddDbContext<Casa106DbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IMovimientoRepository, MovimientoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IPropiedadRepository, PropiedadRepository>();
builder.Services.AddScoped<IDocumentoRepository, DocumentoRepository>();

builder.Services.AddScoped<
    IFinancialDocumentAnalyzer,
    FakeFinancialDocumentAnalyzer>();

var cloudinaryConfig = builder.Configuration.GetSection("Cloudinary");

if (!string.IsNullOrWhiteSpace(cloudinaryConfig["CloudName"]) &&
    !string.IsNullOrWhiteSpace(cloudinaryConfig["ApiKey"]) &&
    !string.IsNullOrWhiteSpace(cloudinaryConfig["ApiSecret"]))
{
    builder.Services.AddScoped<
        IDocumentStorage,
        CloudinaryDocumentStorage>();
}
else
{
    builder.Services.AddScoped<IDocumentStorage>(sp =>
        new LocalDocumentStorage(
            Path.Combine(
                builder.Environment.ContentRootPath,
                "uploads")));
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseRouting();

app.UseCors("FrontendPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();