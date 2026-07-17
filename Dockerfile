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