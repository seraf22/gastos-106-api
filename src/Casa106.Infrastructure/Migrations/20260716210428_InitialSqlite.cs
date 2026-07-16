using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Casa106.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    TipoMovimiento = table.Column<int>(type: "INTEGER", nullable: false),
                    Grupo = table.Column<int>(type: "INTEGER", nullable: false),
                    Activa = table.Column<bool>(type: "INTEGER", nullable: false),
                    Orden = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Propiedades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Direccion = table.Column<string>(type: "TEXT", nullable: true),
                    Unidad = table.Column<string>(type: "TEXT", nullable: true),
                    Activa = table.Column<bool>(type: "INTEGER", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Propiedades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PropiedadId = table.Column<Guid>(type: "TEXT", nullable: false),
                    NombreOriginal = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    NombreAlmacenado = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    RutaAlmacenamiento = table.Column<string>(type: "TEXT", nullable: false),
                    TipoMime = table.Column<string>(type: "TEXT", nullable: false),
                    Extension = table.Column<string>(type: "TEXT", nullable: false),
                    TamanoBytes = table.Column<long>(type: "INTEGER", nullable: false),
                    HashArchivo = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    TextoExtraido = table.Column<string>(type: "TEXT", nullable: true),
                    RespuestaIaJson = table.Column<string>(type: "TEXT", nullable: true),
                    FechaCarga = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documentos_Propiedades_PropiedadId",
                        column: x => x.PropiedadId,
                        principalTable: "Propiedades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ocupaciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PropiedadId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Anio = table.Column<int>(type: "INTEGER", nullable: false),
                    Mes = table.Column<int>(type: "INTEGER", nullable: false),
                    NochesDisponibles = table.Column<int>(type: "INTEGER", nullable: false),
                    NochesOcupadas = table.Column<int>(type: "INTEGER", nullable: false),
                    IngresosAlojamiento = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    TarifaPromedio = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true),
                    Observaciones = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ocupaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ocupaciones_Propiedades_PropiedadId",
                        column: x => x.PropiedadId,
                        principalTable: "Propiedades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Movimientos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PropiedadId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CategoriaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false),
                    Origen = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaMovimiento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PeriodoDesde = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    PeriodoHasta = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    Monto = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Proveedor = table.Column<string>(type: "TEXT", nullable: true),
                    MetodoPago = table.Column<string>(type: "TEXT", nullable: true),
                    DocumentoId = table.Column<Guid>(type: "TEXT", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movimientos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movimientos_Documentos_DocumentoId",
                        column: x => x.DocumentoId,
                        principalTable: "Documentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Movimientos_Propiedades_PropiedadId",
                        column: x => x.PropiedadId,
                        principalTable: "Propiedades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcesaminetosIA",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DocumentoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Proveedor = table.Column<string>(type: "TEXT", nullable: false),
                    Modelo = table.Column<string>(type: "TEXT", nullable: false),
                    Solicitud = table.Column<string>(type: "TEXT", nullable: false),
                    Respuesta = table.Column<string>(type: "TEXT", nullable: false),
                    Confianza = table.Column<decimal>(type: "TEXT", nullable: false),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false),
                    Error = table.Column<string>(type: "TEXT", nullable: true),
                    FechaProcesamiento = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcesaminetosIA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcesaminetosIA_Documentos_DocumentoId",
                        column: x => x.DocumentoId,
                        principalTable: "Documentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_TipoMovimiento_Activa",
                table: "Categorias",
                columns: new[] { "TipoMovimiento", "Activa" });

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_HashArchivo",
                table: "Documentos",
                column: "HashArchivo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_PropiedadId",
                table: "Documentos",
                column: "PropiedadId");

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_CategoriaId",
                table: "Movimientos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_DocumentoId",
                table: "Movimientos",
                column: "DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_PropiedadId_Estado",
                table: "Movimientos",
                columns: new[] { "PropiedadId", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_PropiedadId_FechaMovimiento",
                table: "Movimientos",
                columns: new[] { "PropiedadId", "FechaMovimiento" });

            migrationBuilder.CreateIndex(
                name: "IX_Ocupaciones_PropiedadId_Anio_Mes",
                table: "Ocupaciones",
                columns: new[] { "PropiedadId", "Anio", "Mes" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcesaminetosIA_DocumentoId",
                table: "ProcesaminetosIA",
                column: "DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Propiedades_Activa",
                table: "Propiedades",
                column: "Activa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movimientos");

            migrationBuilder.DropTable(
                name: "Ocupaciones");

            migrationBuilder.DropTable(
                name: "ProcesaminetosIA");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Documentos");

            migrationBuilder.DropTable(
                name: "Propiedades");
        }
    }
}
