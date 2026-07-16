# 🚀 NETLIFY + RENDER - Guía de Despliegue (5 minutos)

## Arquitectura Final

```
┌─────────────────────────────────────────────┐
│          NETLIFY (Frontend React)           │
│  • Hosting estático (Vite dist folder)     │
│  • Auto-deploy desde GitHub                 │
│  • CDN global                               │
│  • Gratis para siempre                      │
└────────┬────────────────────────────────────┘
		 │ HTTPS
		 ▼
┌─────────────────────────────────────────────┐
│        RENDER (Backend .NET 8 API)         │
│  • Docker container                         │
│  • Auto-deploy desde GitHub                 │
│  • Conecta a PostgreSQL Aiven              │
│  • Gratis (con pausas)                      │
└────────┬────────────────────────────────────┘
		 │ HTTPS
		 ▼
┌─────────────────────────────────────────────┐
│      AIVEN (PostgreSQL Database)            │
│  • Ya tienes, ya está funcionando           │
│  • $25/mes                                  │
└─────────────────────────────────────────────┘
```

**Total: $25/mes (igual que ahora)**

---

## PASO 1️⃣: Push a GitHub (2 minutos)

```bash
git add .
git commit -m "feat: add Netlify config for production"
git push origin main
```

**Verifica que aparezca en:** https://github.com/seraf22/gestorGastos106

---

## PASO 2️⃣: Desplegar Frontend en Netlify (3 minutos)

### 2A. Ir a Netlify
1. Ve a https://app.netlify.com
2. Si es la primera vez, crea cuenta (gratis)
3. Click en **"Add new site"** → **"Import an existing project"**

### 2B. Conectar GitHub
1. Click en **"GitHub"**
2. Autoriza Netlify a acceder a GitHub
3. Selecciona tu repo: **`gestorGastos106`**
4. Netlify automáticamente detectará la config en `netlify.toml`

### 2C. Verificar Configuración
Netlify debería mostrar:
- **Base directory**: `src/Casa106.Web` ✅
- **Build command**: `npm run build` ✅
- **Publish directory**: `dist` ✅

Si duda, edita para que esté exacto ↑

### 2D. Configurar Variables de Entorno
1. Click en **"Build & deploy"** → **"Environment"**
2. Click en **"Edit variables"**
3. Añade:
   ```
   VITE_API_URL = https://tu-api.onrender.com
   ```
   (Usa URL de tu API en Render - podrás actualizarla después)

### 2E. Desplegar
1. Click en **"Deploy site"**
2. Espera ~2 minutos
3. ¡Listo! Tu frontend estará en: `https://tu-sitio.netlify.app`
4. Netlify te dará una URL (puedes personalizarla)

✅ **Frontend LIVE**

---

## PASO 3️⃣: Desplegar Backend en Render (10 minutos)

### 3A. Ir a Render
1. Ve a https://dashboard.render.com
2. Si es la primera vez, crea cuenta (gratis)
3. Click en **"New"** → **"Web Service"**

### 3B. Conectar GitHub
1. Click en **"GitHub"**
2. Autoriza Render a acceder a GitHub
3. Selecciona tu repo: **`gestorGastos106`**
4. Render automáticamente detectará el `Dockerfile`

### 3C. Configurar Service
- **Name**: `casa106-api`
- **Region**: Elige el más cercano a tu región
- **Branch**: `main`
- **Plan**: `Free` (está bien para comenzar)
- Click en **"Create Web Service"**

### 3D. Configurar Variables de Entorno
Mientras Render hace el build, ve a **"Environment"** y añade:

```
ASPNETCORE_ENVIRONMENT = Production
ASPNETCORE_URLS = http://+:8080
DB_HOST = casa106-db-casa106.i.aivencloud.com
DB_PORT = 16228
DB_NAME = defaultdb
DB_USER = avnadmin
DB_PASSWORD = (tu password de Aiven)
```

⚠️ **IMPORTANTE**: Usa "Secret Files" o marcar como "Secret" para el password

### 3E. Deploy
1. Render automáticamente hace build (5-10 minutos)
2. Si hay error, revisa logs en dashboard
3. Una vez LIVE, tu URL será: `https://casa106-api.onrender.com`
4. Guarda esta URL

✅ **Backend LIVE**

---

## PASO 4️⃣: Actualizar Netlify con URL Real (1 minuto)

Ahora que tienes tu URL de Render:

1. Ve a tu dashboard de Netlify
2. Click en **"Site settings"** → **"Build & deploy"** → **"Environment"**
3. Edita `VITE_API_URL` con tu URL real de Render:
   ```
   VITE_API_URL = https://casa106-api.onrender.com
   ```
4. Netlify automáticamente triggerea un redeploy
5. ¡Listo!

---

## PASO 5️⃣: Verificar Firewall Aiven (2 minutos)

Tu Render debe poder conectarse a Aiven:

1. Ve a https://console.aiven.io
2. Selecciona tu PostgreSQL
3. Ve a **"Networking"** → **"IP Allowlist"**
4. Añade IP de Render:
   - **Opción fácil**: `0.0.0.0/0` (permite todo, ok para testing)
   - **Opción segura**: Averigua IP de tu Render y permite solo esa
5. Save

---

## ✅ VERIFICACIÓN FINAL

### Test 1: Frontend Carga
```
https://tu-sitio.netlify.app
→ Debe cargar sin errores
→ Abre DevTools (F12) → Console
→ No debe haber errores rojos
```

### Test 2: API Responde
```
https://casa106-api.onrender.com/swagger
→ Debe mostrar Swagger UI
```

### Test 3: Frontend Llama API
```
1. En https://tu-sitio.netlify.app
2. Abre DevTools (F12) → Network
3. Haz algo que llame API (click botón, enviar formulario)
4. Deberías ver request a https://casa106-api.onrender.com/api/...
5. Respuesta debe tener data de la BD
```

Si esto funciona, ¡GANASTE! 🎉

---

## 📊 URLs de tu Aplicación LIVE

Una vez todo deploy:

```
Frontend:  https://tu-sitio.netlify.app
API:       https://casa106-api.onrender.com
API Docs:  https://casa106-api.onrender.com/swagger
GitHub:    https://github.com/seraf22/gestorGastos106
```

---

## 🔄 Cómo Actualizar en Producción

Desde este momento, cada vez que hagas push a `main`:

```bash
git add .
git commit -m "update features"
git push origin main
```

**Automáticamente:**
1. Netlify detecta cambio → rebuilds frontend → live en 1-2 min
2. Render detecta cambio → builds Docker → live en 5-10 min

**Sin hacer nada más.** 🤖

---

## ⚠️ Notas Importantes

### Free Tier Render
- App se pausa después de 15 min inactividad
- Primer request tras pausa: ~30 seg (cold start)
- Es normal, no te preocupes
- Upgrade a $7/mes si quieres evitarlo

### Limites Netlify Free
- 300 minutos de build/mes (ampliamente suficiente)
- Hosting ilimitado
- CDN global incluido

### Secretos Seguros
- NO hagas commit de `.env` con credenciales reales
- Usa Environment Variables en Netlify y Render (marcadas como Secret)
- `.env` ya está en `.gitignore`

---

## 🐛 Troubleshooting Rápido

### Frontend no ve API
```
1. Verifica VITE_API_URL en Netlify
2. Verifica que sea HTTPS URL de Render
3. Check browser console (F12) para CORS errors
```

### API no conecta BD
```
1. Verifica credenciales en Render Environment
2. Verifica Aiven IP allowlist
3. Mira logs en Render: Dashboard → Logs
```

### Build falla en Netlify
```
1. Mira logs: Site → Deploys → click en deployment fallido
2. Usual: falta dependencia, typo en config
3. Fix local: cd src/Casa106.Web && npm run build
```

### Build falla en Render
```
1. Mira logs: Web Service → Logs
2. Verifica variables de entorno (nombres exactos)
3. Verifica Dockerfile existe y no tiene typos
```

---

## 💡 Proximos Pasos (Futuro)

Una vez todo funcione:

1. **Dominio personalizado** (opcional):
   - Netlify: Easy custom domain
   - Render: También soporta

2. **Mejorar performance**:
   - Render: Upgrade a plan pagado ($7+)
   - Netlify: Ya está optimizado

3. **Monitoring**:
   - Ambos tienen dashboards con métricas

4. **Backups BD**:
   - Aiven: Automáticos incluidos

---

## URLs de Referencia

| Servicio | Dashboard |
|----------|-----------|
| Netlify | https://app.netlify.com/sites |
| Render | https://dashboard.render.com |
| Aiven | https://console.aiven.io |
| GitHub | https://github.com/seraf22/gestorGastos106 |

---

## 🎯 Resumen en 30 segundos

1. ✅ Push a GitHub
2. ✅ Netlify auto-deploy frontend
3. ✅ Render auto-deploy backend
4. ✅ Aiven ya está configurado
5. ✅ Everything LIVE in ~15 minutes

**¡Eso es todo!** 🚀

Questions? Revisa logs en Netlify/Render o este documento nuevamente.
