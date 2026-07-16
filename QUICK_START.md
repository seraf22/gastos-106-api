# ⚡ Resumen Rápido: Despliegue Netlify + Render

Tu proyecto ya está listo para producción. Aquí está el flujo:

## 1️⃣ Frontend (React) → Netlify

```bash
# Solo debes hacer push a GitHub en rama main
git push origin main
```

**En Netlify:**
1. Ve a https://app.netlify.com/new
2. Conecta tu repo GitHub
3. Netlify automáticamente detecta `netlify.toml` y configura:
   - Base directory: `src/Casa106.Web`
   - Build Command: `npm run build`
   - Output Directory: `dist`
4. En "Environment Variables" añade:
   ```
   VITE_API_URL="https://tu-api.onrender.com"
   ```
5. Deploy automático✨

**Tu frontend estará en:** `https://tu-proyecto.netlify.app`

> **Ventaja Netlify**: Mejor conectividad GitHub que Vercel, más estable

---

## 2️⃣ Backend API (.NET 8) → Render.com

```bash
# Push a GitHub (ya lo haces arriba)
git push origin main
```

**En Render.com:**
1. Ve a Dashboard → "New" → "Web Service"
2. Conecta tu repo GitHub
3. Configure:
   - Name: `casa106-api`
   - Region: `Oregon` (u otro que prefieras)
   - Branch: `main`
   - Environment: `Docker`
   - Plan: `Free`

4. En la pestaña **"Environment"**, añade tus variables:

```
ASPNETCORE_ENVIRONMENT = Production
ASPNETCORE_URLS = http://+:8080
DB_HOST = casa106-db-casa106.i.aivencloud.com
DB_PORT = 16228
DB_NAME = defaultdb
DB_USER = avnadmin
DB_PASSWORD = AVNS_AT_6_Z_t7ClGxyibTFB
```

> ⚠️ **Mejor**: Usa "Secret Files" para las credenciales

5. Click en **"Create Web Service"**
6. Render automáticamente:
   - Lee tu `Dockerfile`
   - Hace build
   - Despliega
   - Le da una URL

**Tu API estará en:** `https://tu-api.onrender.com`

---

## 3️⃣ Base de Datos (PostgreSQL) → Ya está en Aiven ✅

No hace falta hacer nada. Ya es visible desde internet.

**Solo asegúrate que el firewall de Aiven permite:**
1. Ve a https://console.aiven.io
2. Tu servicio PostgreSQL → "Networking" → "IP Filter"
3. Añade IP de Render (0.0.0.0/0 para testing)

---

## 📝 Variables de Entorno Necesarias

### En Render (Backend API):
```env
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:8080
DB_HOST=casa106-db-casa106.i.aivencloud.com
DB_PORT=16228
DB_NAME=defaultdb
DB_USER=avnadmin
DB_PASSWORD=<tu-password>
CLOUDINARY_CLOUD_NAME=<si tienes>
CLOUDINARY_API_KEY=<si tienes>
CLOUDINARY_API_SECRET=<si tienes>
```

### En Vercel (Frontend):
```env
VITE_API_URL=https://tu-api.onrender.com
```

---

## ✅ Checklist Final

- [ ] Proyecto compilado localmente: ✅ (ya pasó)
- [ ] Dockerfile preparado: ✅ (ya existe)
- [ ] appsettings.Production.json creado: ✅
- [ ] vercel.json creado: ✅
- [ ] Push a GitHub rama `main`
- [ ] Crear Web Service en Render → deploy automático
- [ ] Crear Proyecto en Vercel → deploy automático
- [ ] Verificar CORS en Aiven Firewall
- [ ] Testar: Frontend llama a API → API llama a BD

---

## 🚀 Estimación de Tiempos

| Tarea | Tiempo |
|-------|--------|
| Push a GitHub | ~1 min |
| Setup Render | ~5 min |
| First build Render | ~10 min (cold start) |
| Setup Vercel | ~3 min |
| First build Vercel | ~2 min |
| **Total** | ~20 min |

---

## 🔗 URLs de Configuración

- [Render Dashboard](https://dashboard.render.com)
- [Vercel Dashboard](https://vercel.com/dashboard)
- [Aiven Console](https://console.aiven.io)
- [GitHub](https://github.com/seraf22/gestorGastos106)

---

## 💬 Pruebas Rápidas

Después del deploy:

```bash
# Testar API
curl https://tu-api.onrender.com/swagger

# Ver logs Render
Render Dashboard → tu-app → Logs

# Ver logs Vercel
Vercel Dashboard → tu-proyecto → Deployments → Logs
```

---

## ⚠️ Notas Importantes

1. **Free tier en Render**: La app se pausa tras 15 minutos de inactividad
   - Primer request después de pausa: ~30 seg (cold start)
   - Es normal

2. **El Dockerfile ya existe**: VS tal vez muestre error de caché, pero `dotnet build` compila sin problemas

3. **Secretos en GitHub**: Nunca hagas commit de `.env` con credenciales reales
   - Usa archivos `.local` (gitignored)
   - Usa Secrets en Render/Vercel

4. **CORS**: Ya está configurado para permitir cualquier origen

---

## 📞 Si Algo Falla

### Frontend no ve API
- Verifica `VITE_API_URL` en Vercel Environment Variables
- Revisa console del navegador (F12)
- Mira logs en Vercel

### API no conecta a BD
- Verifica credenciales en Render Environment Variables
- Verifica firewall en Aiven Console
- Mira logs en Render

### Cold start lento
- Es normal en free tier de Render
- Puedes hacer upgrade a plan pagado para mejorar

---

## 🎯 ¡Listo para producción!

Todo está configurado. Solo necesitas hacer push y los servicios se desplegarán solos. 🚀

Cualquier duda, revisa `DEPLOYMENT.md` para más detalles.
