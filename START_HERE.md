# 🎯 START HERE - Netlify + Render en 3 Pasos

## Archivo: `NETLIFY_RENDER_SETUP.md` ← LEE ESTO PRIMERO

Es simple como 1-2-3:

### Step 1: Push a GitHub
```bash
git add .
git commit -m "production ready"
git push origin main
```

### Step 2: Deploy Frontend en Netlify (3 min)
1. https://app.netlify.com/new
2. Conecta GitHub
3. Selecciona repo `gestorGastos106`
4. **Listo** - Netlify lee `netlify.toml` automáticamente
5. Obtienes: `https://tu-sitio.netlify.app`

### Step 3: Deploy Backend en Render (10 min)
1. https://dashboard.render.com
2. "New" → "Web Service"
3. Conecta GitHub, selecciona repo
4. Agrega variables de entorno (BD Aiven)
5. **Listo** - Render lee `Dockerfile` automáticamente
6. Obtienes: `https://tu-api.onrender.com`

### Step 4: Update Netlify con URL real de Render (1 min)
- Vuelve a Netlify
- Edit `VITE_API_URL` con URL de Render
- Auto-redeploy ✨

---

## ¿Por qué Netlify en lugar de Vercel?

✅ Mejor conectividad con GitHub (sin problemas)  
✅ Más estable  
✅ Mismo pricing gratis  
✅ Config más simple (`netlify.toml`)  
✅ Soporte superior  

---

## Total Time: ~20 minutes

| Step | Time |
|------|------|
| Push to GitHub | 1 min |
| Setup Netlify | 3 min |
| First Netlify build | 2 min |
| Setup Render | 5 min |
| First Render build | 10 min |
| Verify all working | 5 min |
| **TOTAL** | **~20 min** |

---

## Archivos Que Creamos Para Ti

✅ `netlify.toml` - Config automática para Netlify  
✅ `appsettings.Production.json` - Config para Render  
✅ `Dockerfile` - Ya existía, perfecto  
✅ `.env.example` - Template de variables  
✅ `NETLIFY_RENDER_SETUP.md` - Guía detallada (↑ léelo)

---

## URLs Post-Deploy

```
Frontend:  https://casa106.netlify.app
Backend:   https://casa106-api.onrender.com
API Docs:  https://casa106-api.onrender.com/swagger
```

---

## Esto es todo lo que necesitas saber

1. **Proyecto compilado**: ✅
2. **Config creada**: ✅
3. **Dockerfile listo**: ✅
4. **netlify.toml listo**: ✅

**Siguiente**: Lee `NETLIFY_RENDER_SETUP.md` para pasos exactos.

¡Vas a estar LIVE en 20 minutos! 🚀
