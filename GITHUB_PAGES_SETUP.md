# 🚀 GitHub Pages + Render - Setup (10 minutos)

## ¿Por qué GitHub Pages?

✅ Hosting GRATIS dentro de tu repo  
✅ Auto-deploy con cada push  
✅ Cero configuración externa  
✅ Ya tienes GitHub, solo activar  
✅ Dominio: `tu-user.github.io/gestorGastos106`  

---

## PASO 1: Configurar GitHub (2 minutos)

### 1.1 Ve a tu Repositorio
https://github.com/seraf22/gestorGastos106

### 1.2 Settings → Pages
1. Click en **"Settings"** (arriba a la derecha)
2. Scroll down a **"Pages"** (en el menú izquierdo)
3. **Source**: Selecciona **"GitHub Actions"**
4. Save

### 1.3 Agregar Secret para API URL
1. Settings → **"Secrets and variables"** → **"Actions"**
2. Click **"New repository secret"**
3. Name: `VITE_API_URL`
4. Value: `https://casa106-api.onrender.com` (actualizar después con URL real)
5. Click **"Add secret"**

### 1.4 Push a GitHub
```bash
git add .
git commit -m "production: add GitHub Actions workflow"
git push origin main
```

**Lo que sucede automáticamente:**
- GitHub detecta cambio
- Ejecuta workflow `.github/workflows/deploy.yml`
- Compila React con `npm run build`
- Deploya a GitHub Pages
- Tu frontend estará LIVE en: `https://seraf22.github.io/gestorGastos106`

---

## PASO 2: Desplegar Backend en Render (10 minutos)

### 2.1 Ir a Render
https://dashboard.render.com

### 2.2 Crear Web Service
1. Click en **"New"** → **"Web Service"**
2. Selecciona **GitHub**
3. Conecta tu repo: `gestorGastos106`
4. Autoriza Render a acceder a GitHub

### 2.3 Configurar
- **Name**: `casa106-api`
- **Branch**: `main`
- **Environment**: `Docker` (Render auto-detecta)
- **Plan**: `Free`

### 2.4 Variables de Entorno
Mientras Render hace el build, añade:

```
ASPNETCORE_ENVIRONMENT = Production
ASPNETCORE_URLS = http://+:8080
DB_HOST = casa106-db-casa106.i.aivencloud.com
DB_PORT = 16228
DB_NAME = defaultdb
DB_USER = avnadmin
DB_PASSWORD = (tu password Aiven)
```

⚠️ Marca el password como "Secret"

### 2.5 Deploy
- Click **"Create Web Service"**
- Espera ~10 minutos
- Tu API estará en: `https://casa106-api.onrender.com`

---

## PASO 3: Actualizar GitHub Secret (1 minuto)

Una vez que Render esté LIVE:

1. Ve a tu repo → **Settings** → **Secrets and variables** → **Actions**
2. Edit `VITE_API_URL`
3. Value: `https://casa106-api.onrender.com` (URL real)
4. Save

Automáticamente GitHub redeploya el frontend con la URL correcta.

---

## ✅ Verificación

### Test 1: Frontend Carga
```
https://seraf22.github.io/gestorGastos106
→ Debe cargar sin errores
```

### Test 2: API Responde
```
https://casa106-api.onrender.com/swagger
→ Debe mostrar Swagger UI
```

### Test 3: Full Flow
1. En frontend, abre DevTools (F12) → Network
2. Haz algo que llame API
3. Debe ver request a `casa106-api.onrender.com`
4. Respuesta con data de BD

Si todo ✅, ¡GANASTE! 🎉

---

## 📊 URLs Finales

```
Frontend:  https://seraf22.github.io/gestorGastos106
Backend:   https://casa106-api.onrender.com
API Docs:  https://casa106-api.onrender.com/swagger
```

---

## 🔄 Cómo Actualizar en Futuro

Cada vez que hagas cambios:

```bash
git add .
git commit -m "tu mensaje"
git push origin main
```

Automáticamente:
1. GitHub Actions detecta push
2. Compila frontend
3. Deploya a GitHub Pages (1-2 min)
4. ¡LIVE!

---

## ⚠️ Notas Importantes

### GitHub Pages
- Hosting de contenido **ESTÁTICO**
- Perfect para React SPA (Vite build)
- Free para siempre
- Sin serverless logic (backend en Render)

### Render Backend
- Free tier con pausas (15 min inactividad)
- Upgrade a $7/mo si necesitas mejor performance

### CORS
- Ya configurado en tu API
- Funciona perfectamente con GitHub Pages

### Secrets Seguros
- `VITE_API_URL` nunca aparece en GitHub (es secret)
- Credenciales BD solo en Render (marcadas secret)

---

## 🐛 Troubleshooting

### GitHub Actions fallo
```
1. Ve a repo → Actions
2. Busca build fallido
3. Click para ver logs
4. Usual: typo en package.json o configuración
```

### Frontend no ve API
```
1. Verifica VITE_API_URL en repo secrets
2. Deve ser: https://casa106-api.onrender.com
3. Espera deploy completado (el círculo verde en Actions)
4. Refresh pagina
```

### API no conecta BD
```
1. Render dashboard → Logs
2. Verifica variables de entorno exactas
3. Verifica firewall Aiven permite IP de Render
```

---

## 💡 Ventajas GitHub Pages

✅ Zero configuración externa  
✅ Dominio gratis (github.io)  
✅ SSL/TLS automático  
✅ CDN global incluido  
✅ Integración perfecta con GitHub  
✅ Gratis para siempre  
✅ Un comando para todo (git push)  

---

## 📊 Arquitectura Final

```
git push origin main
		 ↓
	GitHub recibe código
		 ↓
	GitHub Actions CI/CD
		 ├─→ npm install
		 ├─→ npm run build
		 ├─→ Upload dist/ folder
		 └─→ Deploy a GitHub Pages

   RESULTADO:
   Frontend LIVE: 
   https://seraf22.github.io/gestorGastos106 ✨

   Render monitorea main branch también:
   ├─→ Detecta Dockerfile
   ├─→ docker build
   ├─→ docker run
   └─→ Deploy

   RESULTADO:
   Backend LIVE:
   https://casa106-api.onrender.com ✨
```

---

## ✨ Timeline

| Paso | Tiempo | Acción |
|------|--------|--------|
| Configurar GitHub Pages | 2 min | Settings → Pages → GitHub Actions |
| Add API secret | 1 min | Secrets → VITE_API_URL |
| Push código | 1 min | git push |
| GitHub Actions build | 2 min | Auto-deploy frontend |
| Render deploy | 10 min | Auto-deploy backend |
| Verify everything | 2 min | Test endpoints |
| **TOTAL** | **~18 min** | **LIVE!** |

---

## 🎯 Resumen

1. **GitHub** → Settings → Pages → GitHub Actions ✅
2. **GitHub** → Secrets → Add VITE_API_URL ✅
3. **Git push main** (10 caracteres de workflow) ✅
4. **Render** → New Web Service → Conecta repo ✅
5. **Render** → Add DB environment variables ✅
6. **Esperar** → Ambos deploys completos ✅
7. **Disfrutar** → Backend en Render, Frontend en GitHub Pages ✅

---

**Siguiente:** Lee el archivo `00_READ_ME_FIRST.txt` o `START_HERE.md`

Estarás LIVE en internet en ~20 minutos. 🚀
