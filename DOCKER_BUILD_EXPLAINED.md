# 🔧 DOCKER BUILD FIX - Restore vs Build vs Publish

## El Problema Real

Los errores mostraban que `Casa106.Application.Abstractions` no se encontraba durante Docker build, pero compilaba perfectamente localmente.

**¿Por qué?**

Local ← Tiene cache de compilaciones previas
Docker ← Todo está fresco, nada compilado

---

## La Raíz del Problema

**Antes:**
```dockerfile
RUN dotnet restore "src/Casa106.Api/Casa106.Api.csproj"
RUN dotnet publish "src/Casa106.Api/Casa106.Api.csproj" -c Release -o /app/publish --no-restore
```

**¿Qué hace `restore`?**
- Descarga paquetes NuGet (desde la carpeta packages)
- Resuelve referencias externas
- **NO compila el proyecto**

**¿Qué pasa entonces?**
- `restore` descarga el paquete de `Casa106.Application` pero no lo compila
- `publish --no-restore` intenta publicar sin compilar las dependencias
- Infrastructure intenta usar `Casa106.Application.Abstractions` pero no está compilado
- **FALLA** ❌

---

## La Solución

**Ahora:**
```dockerfile
RUN dotnet restore "src/Casa106.Api/Casa106.Api.csproj"
RUN dotnet build "src/Casa106.Api/Casa106.Api.csproj" -c Release --no-restore
RUN dotnet publish "src/Casa106.Api/Casa106.Api.csproj" -c Release -o /app/publish --no-restore
```

**El flujo correcto es:**

1. **RESTORE** - Descargar dependencias
   ```
   dotnet restore
   ```

2. **BUILD** - Compilar TODO (incluyendo referencias de proyecto)
   ```
   dotnet build ... --no-restore
   ```
   **Esto compila:**
   - Casa106.Domain
   - Casa106.Application (incluyendo Abstractions)
   - Casa106.Infrastructure
   - Casa106.Api

3. **PUBLISH** - Empaquetar los binarios compilados
   ```
   dotnet publish ... --no-restore
   ```
   Ahora sí encontrará todos los tipos compilados

---

## Por Qué Funciona

| Paso | Qué Hace | Resultado |
|------|----------|-----------|
| restore | Obtiene paquetes + project refs | ✅ Referencias disponibles |
| build | Compila TODOS los proyectos | ✅ `Casa106.Application.Abstractions` compilado |
| publish | Empaqueta binarios compilados | ✅ Tipos disponibles, publish exitoso |

Localmente funcionaba porque:
- Tu máquina tiene cache de compilaciones previas
- Visual Studio ya había compilado `Application.Abstractions`
- El `publish` encontraba los binarios en el cache

En Docker:
- Todo está fresco (no hay cache)
- Ahora el `build` paso asegura que TODO se compila antes de publish

---

## El Dockerfile Actualizado

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /repo

# Copy entire source structure
COPY . .

# Restore packages
RUN dotnet restore "src/Casa106.Api/Casa106.Api.csproj"

# Build the entire solution to compile all dependencies (including Application.Abstractions)
RUN dotnet build "src/Casa106.Api/Casa106.Api.csproj" -c Release --no-restore

# Publish
RUN dotnet publish "src/Casa106.Api/Casa106.Api.csproj" -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Create upload directory
RUN mkdir -p /app/uploads

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

# Copy published app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Casa106.Api.dll"]
```

---

## Verificación Local

```bash
# En tu máquina, esto debería funcionar:
cd C:\Users\sebar\source\repos\gestorGastos106
dotnet restore src/Casa106.Api/Casa106.Api.csproj
dotnet build src/Casa106.Api/Casa106.Api.csproj -c Release --no-restore
dotnet publish src/Casa106.Api/Casa106.Api.csproj -c Release -o ./publish --no-restore
```

Si esto funciona localmente, Docker también debería funcionar.

---

## Próximo Paso

```bash
git add Dockerfile
git commit -m "Fix Docker: agregar dotnet build step para compilar dependencias"
git push origin master
```

Docker debería construir correctamente ahora. ✅

---

## Lecciones Aprendidas

1. **`restore` ≠ `build`**
   - Restore: descargar paquetes
   - Build: compilar código
   - Publish: empaquetar para producción

2. **Orden importa en Docker**
   - Sin cache local, cada paso construye sobre el anterior
   - Si skip el build, las dependencias no se compilan

3. **`--no-restore` es un optimization**
   - Dice "ya hiciste restore, no lo hagas de nuevo"
   - Pero requiere que el paso anterior realmente haya compilado
