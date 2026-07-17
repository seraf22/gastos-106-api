# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Copy entire source structure
COPY . .
# Restore and publish
RUN dotnet restore "src/Casa106.Api/Casa106.Api.csproj"
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