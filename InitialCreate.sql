CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Categorias" (
    "Id" uuid NOT NULL,
    "Nombre" character varying(100) NOT NULL,
    "TipoMovimiento" integer NOT NULL,
    "Grupo" integer NOT NULL,
    "Activa" boolean NOT NULL,
    "Orden" integer NOT NULL,
    CONSTRAINT "PK_Categorias" PRIMARY KEY ("Id")
);

CREATE TABLE "Propiedades" (
    "Id" uuid NOT NULL,
    "Nombre" character varying(200) NOT NULL,
    "Direccion" text,
    "Unidad" text,
    "Activa" boolean NOT NULL,
    "FechaCreacion" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Propiedades" PRIMARY KEY ("Id")
);

CREATE TABLE "Documentos" (
    "Id" uuid NOT NULL,
    "PropiedadId" uuid NOT NULL,
    "NombreOriginal" character varying(256) NOT NULL,
    "NombreAlmacenado" character varying(256) NOT NULL,
    "RutaAlmacenamiento" text NOT NULL,
    "TipoMime" text NOT NULL,
    "Extension" text NOT NULL,
    "TamanoBytes" bigint NOT NULL,
    "HashArchivo" character varying(64) NOT NULL,
    "TextoExtraido" text,
    "RespuestaIaJson" text,
    "FechaCarga" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Documentos" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Documentos_Propiedades_PropiedadId" FOREIGN KEY ("PropiedadId") REFERENCES "Propiedades" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "Ocupaciones" (
    "Id" uuid NOT NULL,
    "PropiedadId" uuid NOT NULL,
    "Anio" integer NOT NULL,
    "Mes" integer NOT NULL,
    "NochesDisponibles" integer NOT NULL,
    "NochesOcupadas" integer NOT NULL,
    "IngresosAlojamiento" numeric(18,2) NOT NULL,
    "TarifaPromedio" numeric(18,2),
    "Observaciones" text,
    CONSTRAINT "PK_Ocupaciones" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Ocupaciones_Propiedades_PropiedadId" FOREIGN KEY ("PropiedadId") REFERENCES "Propiedades" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "Movimientos" (
    "Id" uuid NOT NULL,
    "PropiedadId" uuid NOT NULL,
    "CategoriaId" uuid NOT NULL,
    "Tipo" integer NOT NULL,
    "Estado" integer NOT NULL,
    "Origen" integer NOT NULL,
    "FechaMovimiento" timestamp with time zone NOT NULL,
    "PeriodoDesde" date,
    "PeriodoHasta" date,
    "Monto" numeric(18,2) NOT NULL,
    "Descripcion" character varying(500) NOT NULL,
    "Proveedor" text,
    "MetodoPago" text,
    "DocumentoId" uuid,
    "FechaCreacion" timestamp with time zone NOT NULL,
    "FechaActualizacion" timestamp with time zone,
    CONSTRAINT "PK_Movimientos" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Movimientos_Categorias_CategoriaId" FOREIGN KEY ("CategoriaId") REFERENCES "Categorias" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Movimientos_Documentos_DocumentoId" FOREIGN KEY ("DocumentoId") REFERENCES "Documentos" ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_Movimientos_Propiedades_PropiedadId" FOREIGN KEY ("PropiedadId") REFERENCES "Propiedades" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "ProcesaminetosIA" (
    "Id" uuid NOT NULL,
    "DocumentoId" uuid NOT NULL,
    "Proveedor" text NOT NULL,
    "Modelo" text NOT NULL,
    "Solicitud" text NOT NULL,
    "Respuesta" text NOT NULL,
    "Confianza" numeric NOT NULL,
    "Estado" integer NOT NULL,
    "Error" text,
    "FechaProcesamiento" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_ProcesaminetosIA" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ProcesaminetosIA_Documentos_DocumentoId" FOREIGN KEY ("DocumentoId") REFERENCES "Documentos" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Categorias_TipoMovimiento_Activa" ON "Categorias" ("TipoMovimiento", "Activa");

CREATE UNIQUE INDEX "IX_Documentos_HashArchivo" ON "Documentos" ("HashArchivo");

CREATE INDEX "IX_Documentos_PropiedadId" ON "Documentos" ("PropiedadId");

CREATE INDEX "IX_Movimientos_CategoriaId" ON "Movimientos" ("CategoriaId");

CREATE INDEX "IX_Movimientos_DocumentoId" ON "Movimientos" ("DocumentoId");

CREATE INDEX "IX_Movimientos_PropiedadId_Estado" ON "Movimientos" ("PropiedadId", "Estado");

CREATE INDEX "IX_Movimientos_PropiedadId_FechaMovimiento" ON "Movimientos" ("PropiedadId", "FechaMovimiento");

CREATE UNIQUE INDEX "IX_Ocupaciones_PropiedadId_Anio_Mes" ON "Ocupaciones" ("PropiedadId", "Anio", "Mes");

CREATE INDEX "IX_ProcesaminetosIA_DocumentoId" ON "ProcesaminetosIA" ("DocumentoId");

CREATE INDEX "IX_Propiedades_Activa" ON "Propiedades" ("Activa");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260716193358_InitialCreate', '8.0.4');

COMMIT;

