# 🏠 Casa106 - Gestor de Gastos

> Aplicación web para gestionar gastos compartidos de una casa. Backend ASP.NET Core 8 + React SPA + PostgreSQL.

---

## 🚀 DEPLOYMENT READY

**Status:** ✅ Production Ready  
**Frontend:** GitHub Pages (auto-deploy)  
**Backend:** Render (Docker)  
**Database:** Aiven (PostgreSQL)  

### Quick Start

```bash
# 1. Configure GitHub Pages
# Go to: https://github.com/seraf22/gestorGastos106/settings/pages
# Source: GitHub Actions

# 2. Add API Secret
# Go to: https://github.com/seraf22/gestorGastos106/settings/secrets/actions
# New secret: VITE_API_URL = https://casa106-api.onrender.com

# 3. Deploy
git add .
git commit -m "production ready"
git push origin main

# Frontend: https://seraf22.github.io/gestorGastos106 (2-3 min)
# Backend: https://casa106-api.onrender.com (10 min, manual setup)
```

📖 **Read for full details:**
1. `START_HERE.md` - Overview (5 min)
2. `GITHUB_PAGES_SETUP.md` - Step-by-step (15 min)
3. `DEPLOYMENT_SUCCESS.md` - Checklist & troubleshooting

---

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                     GitHub Pages                                │
│              (Frontend - React SPA Vite)                        │
│           https://seraf22.github.io/gestorGastos106            │
└──────────────────────────┬──────────────────────────────────────┘
                           │ VITE_API_URL
                           ↓
┌─────────────────────────────────────────────────────────────────┐
│                     Render Web Service                           │
│           (Backend - ASP.NET Core 8 in Docker)                 │
│                 https://casa106-api.onrender.com               │
└──────────────────────────┬──────────────────────────────────────┘
                           │ PostgreSQL Connection
                           ↓
┌─────────────────────────────────────────────────────────────────┐
│                    Aiven PostgreSQL                              │
│                   (Database)                                    │
└─────────────────────────────────────────────────────────────────┘
```

---

## 📁 Project Structure

```
Casa106.sln
├── src/
│   ├── Casa106.Api/                 # ASP.NET Core Web API
│   │   ├── Program.cs               # DI, config, middleware
│   │   ├── Controllers/
│   │   ├── appsettings.json
│   │   ├── appsettings.Production.json
│   │   └── Dockerfile
│   │
│   ├── Casa106.Web/                 # React + Vite SPA
│   │   ├── src/
│   │   ├── vite.config.ts
│   │   ├── tsconfig.json
│   │   ├── package.json
│   │   └── index.html
│   │
│   ├── Casa106.Infrastructure/      # EF Core, Repositories
│   │   ├── Persistence/Casa106DbContext.cs
│   │   ├── Repositories/
│   │   ├── Storage/
│   │   └── Casa106.Infrastructure.csproj
│   │
│   ├── Casa106.Application/         # Business Logic DTOs
│   │   └── Services/
│   │
│   └── Casa106.Domain/              # Domain Models
│       └── Entities/
│
├── .github/
│   └── workflows/
│       └── deploy.yml               # GitHub Actions CI/CD
│
├── .gitignore
├── Dockerfile                       # Backend container build
├── START_HERE.md                    # Quick overview
├── GITHUB_PAGES_SETUP.md           # Detailed guide
└── DEPLOYMENT_SUCCESS.md           # Checklist & debugging
```

---

## 💻 Local Development

### Prerequisites
- .NET 8 SDK
- Node 18+
- PostgreSQL connection string (Aiven)

### Backend
```bash
cd src/Casa106.Api
dotnet restore
dotnet build
dotnet run
# Swagger: https://localhost:7210/swagger
```

### Frontend
```bash
cd src/Casa106.Web
npm install
npm run dev
# Local: http://localhost:5173
```

### Database
Connection string in `src/Casa106.Api/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=...; Database=...; Username=...; Password=..."
  }
}
```

---

## 🔧 Build & Deploy

### Production Build
```bash
# Backend
dotnet build Casa106.sln -c Release

# Frontend
cd src/Casa106.Web
npm run build  # Output: dist/
```

### Docker

```dockerfile
# Build
docker build -t casa106-api:latest .

# Run
docker run -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ConnectionStrings__DefaultConnection="..." \
  casa106-api:latest
```

### Automated Deploy

GitHub Actions auto-deploys frontend on every push to `main`:
1. Checks out code
2. npm ci + npm run build
3. Uploads to GitHub Pages
4. ✨ Live in 2-3 minutes

Backend on Render (manual first setup, then auto on Dockerfile changes):
1. Connect repo to Render
2. Add environment variables
3. Render detects Dockerfile
4. Auto-builds and deploys
5. ✨ Live in 5-10 minutes

---

## 🔐 Security

### Environment Variables
- Secrets stored in GitHub Actions
- Database password in Render (marked secret)
- API URL in GitHub secret
- Never commit sensitive data

### CORS
Already configured in `Program.cs` for GitHub Pages:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
```

### Database
- Postgres on Aiven (managed service)
- Encrypted connection
- Regular backups
- Access controlled via connection string

---

## 📊 Features

- 📱 Responsive React UI
- 📈 Charts with Recharts
- 💰 Expense tracking
- 👥 Shared budget management
- 🔐 Secure authentication
- ⚡ Real-time updates
- 📊 Analytics dashboard

---

## 🐛 Troubleshooting

### GitHub Actions Failed
1. Go to: https://github.com/seraf22/gestorGastos106/actions
2. Click failed workflow
3. Check logs

### Frontend Not Loading
- Ensure GitHub Pages is enabled (Settings → Pages)
- Hard refresh: Ctrl+Shift+R
- Wait 2-3 minutes for build

### API Connection Issues
- Check Render dashboard for errors
- Verify environment variables
- Check Aiven database is online

### Build Errors
Backend:
```bash
dotnet clean Casa106.sln
dotnet build -c Release
```

Frontend:
```bash
rm -rf node_modules dist
npm install
npm run build
```

---

## 📚 Documentation

- `START_HERE.md` - 5-min quick start
- `GITHUB_PAGES_SETUP.md` - Detailed setup guide  
- `DEPLOYMENT_SUCCESS.md` - Full checklist
- `GITHUB_PAGES_READY.txt` - Build verification
- `DEPLOYMENT_CHECKLIST.md` - Pre-launch checklist

---

## 🤝 Contributing

1. Create feature branch: `git checkout -b feature/name`
2. Commit changes: `git commit -am "feature: description"`
3. Push: `git push origin feature/name`
4. Create Pull Request

---

## 📝 License

Private project for Casa 106

---

## ✨ Status

- ✅ Backend: Compiles & Production Ready
- ✅ Frontend: Builds & Production Ready
- ✅ CI/CD: GitHub Actions Configured
- ✅ Database: Aiven Connected
- ✅ Deployment: Ready (2 more steps)

---

**Next:** Read `START_HERE.md` for deployment instructions.

🚀 You're 15 minutes away from LIVE!

