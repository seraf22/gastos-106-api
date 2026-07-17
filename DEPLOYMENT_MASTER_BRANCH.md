🎯 DEPLOYMENT LISTO - GitHub Pages + Render

Rama corregida: `master` ✅

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

3 PASOS SIMPLES (20 minutos total)

PASO 1: Verificar GitHub Pages (1 min)
──────────────────────────────────────
URL: https://github.com/seraf22/gestorGastos106/settings/pages
→ Source: GitHub Actions
→ Save

PASO 2: Git Push (1 min)
─────────────────────────
$ git add .
$ git commit -m "fix: workflow for master branch"
$ git push origin master

Automáticamente:
→ GitHub Actions detecta push
→ Compila React
→ Deploya a GitHub Pages
→ ¡LIVE en 2-3 minutos!

PASO 3: Monitorear Deploy (2-3 min)
───────────────────────────────────
URL: https://github.com/seraf22/gestorGastos106/actions
→ Espera a que aparezca el workflow "Deploy to GitHub Pages"
→ Debe cambiar a verde ✅
→ Cuando esté verde, tu frontend está LIVE

RESULTADO:
─────────
✨ Frontend LIVE en: https://seraf22.github.io/gestorGastos106

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

PRÓXIMO: BACKEND EN RENDER (10-15 minutos más)

1. Ve a: https://dashboard.render.com
2. Click: New → Web Service
3. Conecta GitHub
4. Selecciona: gestorGastos106
5. Config:
   Name: casa106-api
   Branch: master
   Environment: Docker
6. Agrega Environment Variables:
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://+:8080
   DB_HOST=casa106-db-casa106.i.aivencloud.com
   DB_PORT=16228
   DB_NAME=defaultdb
   DB_USER=avnadmin
   DB_PASSWORD=(tu password)
7. Click: Create Web Service
8. Espera ~10 minutos
9. URL: https://casa106-api.onrender.com

LUEGO: ACTUALIZAR SECRET EN GITHUB (1 min)

URL: https://github.com/seraf22/gestorGastos106/settings/secrets/actions
→ Edit VITE_API_URL
→ Value: https://casa106-api.onrender.com
→ Save

GitHub automáticamente redeploya el frontend.

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

URLs FINALES:

Frontend:  https://seraf22.github.io/gestorGastos106
Backend:   https://casa106-api.onrender.com
API Docs:  https://casa106-api.onrender.com/swagger

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Timeline:
┌─────────────────────────────────────────┐
│ GitHub Pages Setup      → 1 min          │
│ Git Push (master)       → 1 min          │
│ GitHub Actions Build    → 2-3 min        │
│ Frontend LIVE           → ✨ READY       │
│                                         │
│ Render Setup            → 5 min          │
│ Render Build            → 10 min         │
│ Backend LIVE            → ✨ READY       │
│                                         │
│ Update Secret           → 1 min          │
│ Redeploy Frontend       → 2-3 min        │
│                                         │
│ TOTAL:                  → ~25 min        │
└─────────────────────────────────────────┘

¡AHORA! 🚀 Sigue los 3 pasos de arriba.
