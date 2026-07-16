# ✅ DEPLOYMENT CHECKLIST

## Pre-Deployment (Do Now)

### Local Testing
- [ ] Run `dotnet build Casa106.sln` - should show "Build succeeded"
- [ ] Run `cd src/Casa106.Web && npm run build` - should succeed
- [ ] Test locally: `dotnet run` and `npm run dev`
- [ ] Verify API responds: http://localhost:7210/swagger
- [ ] Verify frontend loads: http://localhost:5173
- [ ] Verify frontend calls API and gets data

### Code Review
- [ ] No hardcoded secrets in appsettings.json
- [ ] `.env` is in `.gitignore`
- [ ] CORS is configured (already done ✅)
- [ ] Dockerfile is present (already done ✅)

### GitHub
- [ ] `git add .`
- [ ] `git commit -m "chore: prepare for production deployment"`
- [ ] `git push origin main`
- [ ] Verify code is on GitHub main branch

---

## Render Deployment (Backend API)

### Create Service
- [ ] Go to https://dashboard.render.com
- [ ] Click "New" → "Web Service"
- [ ] Select your GitHub repository
- [ ] Choose `main` branch
- [ ] Name: `casa106-api`
- [ ] Environment: `Docker`
- [ ] Plan: `Free` (free tier is ok to start)
- [ ] Region: Choose closest to you

### Configure Environment Variables
- [ ] Add variable: `ASPNETCORE_ENVIRONMENT` = `Production`
- [ ] Add variable: `ASPNETCORE_URLS` = `http://+:8080`
- [ ] Add variable: `DB_HOST` = `casa106-db-casa106.i.aivencloud.com`
- [ ] Add variable: `DB_PORT` = `16228`
- [ ] Add variable: `DB_NAME` = `defaultdb`
- [ ] Add variable: `DB_USER` = `avnadmin`
- [ ] Add variable: `DB_PASSWORD` = (your Aiven password)

### Deploy
- [ ] Click "Create Web Service"
- [ ] Wait for build to complete (5-10 minutes)
- [ ] Get your API URL from dashboard (will be `https://something.onrender.com`)
- [ ] Save this URL for Vercel configuration

### Verify
- [ ] Check Render logs for errors
- [ ] Try: `curl https://your-api.onrender.com/swagger`
- [ ] Should see Swagger UI

---

## Vercel Deployment (Frontend)

### Create Project  
- [ ] Go to https://vercel.com/new
- [ ] Click "Import Git Repository"
- [ ] Select `gestorGastos106`
- [ ] Vercel auto-detects it's a Vite project

### Configure Build Settings
- [ ] Root Directory: `src/Casa106.Web`
- [ ] Build Command: `npm run build` (usually auto-detected)
- [ ] Output Directory: `dist`
- [ ] Install Command: `npm ci` (usually auto-detected)

### Add Environment Variables
- [ ] Click "Environment Variables"
- [ ] Name: `VITE_API_URL`
- [ ] Value: (your Render API URL from previous step)
  - E.g., `https://casa106-api.onrender.com`
- [ ] Click "Add"

### Deploy
- [ ] Click "Deploy"
- [ ] Wait for build (1-2 minutes)
- [ ] Get your frontend URL (will be `https://something.vercel.app`)

### Verify
- [ ] Frontend loads: https://your-frontend.vercel.app
- [ ] Check browser console (F12) for errors
- [ ] Try clicking something that calls the API
- [ ] Should see API requests in DevTools Network tab

---

## Aiven Configuration (Database Firewall)

- [ ] Go to https://console.aiven.io
- [ ] Select your PostgreSQL service
- [ ] Go to "Networking" → "IP Allowlist"
- [ ] Need to add Render's IP
  - [ ] Option A (easy): Allow `0.0.0.0/0` for testing (not recommended for production)
  - [ ] Option B (better): Get Render's IP and add it specifically
- [ ] Save changes
- [ ] Test from Render logs that DB connection works

---

## Final Verification (Full Flow)

### From Frontend to Backend to Database
- [ ] Frontend loads at https://your-frontend.vercel.app
- [ ] Open DevTools (F12) → Network tab
- [ ] Click a button that calls your API
- [ ] Verify request goes to your Render URL
- [ ] Verify response contains database data
- [ ] Check Render logs - should show successful DB query

### Performance Check
- [ ] Frontend loads in < 5 seconds
- [ ] API responds in < 2 seconds (first request might be slower due to cold start)
- [ ] No console errors in browser
- [ ] No errors in Render logs

---

## After First Successful Deploy

### Keep Monitoring
- [ ] Check Vercel dashboard daily first week
- [ ] Check Render logs if users report issues
- [ ] Monitor API response times
- [ ] Watch for DB connection TimeOuts

### Optional Improvements  
- [ ] Upgrade Render plan from Free to Starter ($7/mo) to avoid cold starts
- [ ] Set up error monitoring/logging service
- [ ] Configure automatic backups for database
- [ ] Set up CDN for better frontend performance

---

## Troubleshooting During Setup

### Frontend loads but no API data
```
1. Check VITE_API_URL environment variable in Vercel
2. Verify it matches your Render URL exactly
3. Check browser console for CORS errors
4. Check Render logs for errors
```

### API won't start
```
1. Check Render logs for specific error
2. Verify DB credentials in environment variables
3. Check Aiven firewall allows Render IP
4. Verify PostgreSQL is running in Aiven
```

### Database connection fails
```
1. Test connection string locally: use same credentials
2. Verify DB_HOST, DB_PORT, DB_USER, DB_PASSWORD in Render
3. Check Aiven IP allowlist includes Render
4. Aiven console → service → "Networking" → verify IP allowed
```

### Slow first load (Cold start)
```
1. This is NORMAL on Render free tier
2. First request after inactivity takes ~30 sec
3. Can't be avoided on free tier
4. Upgrade to paid tier if unacceptable
```

---

## Success Checklist

When ALL of these are true, you're done! ✅

- [ ] Frontend loads without console errors
- [ ] API endpoint responds to HTTP requests  
- [ ] Frontend successfully calls API and displays data
- [ ] Database queries execute successfully
- [ ] Aiven firewall allows connections from Render
- [ ] No hardcoded secrets in GitHub repo
- [ ] Environment variables properly configured in Render & Vercel
- [ ] HTTPS works on both frontend and backend
- [ ] Logs are clean (no recurring errors)

---

## Quick Reference URLs

| Service | URL |
|---------|-----|
| Github Repo | https://github.com/seraf22/gestorGastos106 |
| Render Dashboard | https://dashboard.render.com |
| Vercel Dashboard | https://vercel.com/dashboard |
| Aiven Console | https://console.aiven.io |
| Your Frontend | https://your-project.vercel.app |
| Your API | https://your-api.onrender.com |
| API Docs | https://your-api.onrender.com/swagger |

---

## Contact Support

| Issue | Where to Get Help |
|-------|-------------------|
| Render problems | https://render.com/docs (also check dashboard logs) |
| Vercel problems | https://vercel.com/docs (also check deployment logs) |
| Aiven problems | https://docs.aiven.io + support@aiven.io |
| .NET problems | https://docs.microsoft.com/dotnet |
| React problems | https://react.dev |

---

**Last Updated**: 2025  
**For**: Casa106 - Gestor de Gastos  
**Status**: 🟢 Ready for Production

When you complete this checklist, you'll be live! 🚀
