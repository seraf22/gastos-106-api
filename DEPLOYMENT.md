# 🚀 Guía de Despliegue: Vercel + Render + Aiven

## Vista General

- **Frontend**: Vercel (React + Vite)
- **API Backend**: Render.com (.NET 8)
- **Base de Datos**: Aiven PostgreSQL (existente)

---

## 📋 Requisitos Previos

1. Cuenta en [GitHub](https://github.com) (ya tienes)
2. Cuenta en [Render.com](https://render.com)
3. Cuenta en [Vercel](https://vercel.com)
4. Your Aiven PostgreSQL connection string (ya tienes)
5. Push a `main` branch en GitHub

---

## 🔧 Paso 1: Desplegar API en Render.com

### 1.1 Crear un servicio Web en Render

1. Ve a [https://dashboard.render.com](https://dashboard.render.com)
2. Click en **"New"** → **"Web Service"**
3. Conecta tu repositorio GitHub
4. Configura:
   - **Name**: `casa106-api`
   - **Environment**: `Docker`
   - **Branch**: `main`
   - **Plan**: `Free`

### 1.2 Configurar Variables de Entorno

En Render, ve a **"Environment"** y añade:

```
ASPNETCORE_ENVIRONMENT = Production
ASPNETCORE_URLS = http://+:8080
DB_HOST = casa106-db-casa106.i.aivencloud.com
DB_PORT = 16228
DB_NAME = defaultdb
DB_USER = avnadmin
DB_PASSWORD = AVNS_AT_6_Z_t7ClGxyibTFB
```

⚠️ **Mejor aún**: Usa las variables secretas de Render:
1. Click en **"Secret Files"**
2. Sube un `.env` con las credenciales
3. Render las cargará automáticamente

### 1.3 Deploy

- Click en **"Create Web Service"**
- Render iniciará el build automáticamente
- Espera a que termine (5-10 minutos)
- Tu API estará en: `https://tu-app.onrender.com`

> **Nota**: El primer deploy en free tier tarda más. La app se pausará después de 15 minutos de inactividad.

---

## 🎨 Paso 2: Desplegar Frontend en Vercel

### 2.1 Crear proyecto en Vercel

1. Ve a [https://vercel.com/dashboard](https://vercel.com/dashboard)
2. Click en **"Add New"** → **"Project"**
3. Selecciona tu repositorio `gestorGastos106`
4. Configura:
   - **Framework**: `Vite`
   - **Root Directory**: `src/Casa106.Web`

### 2.2 Configurar Build

En **"Project Settings"** → **"Build & Development Settings"**:

```
Build Command: npm run build
Output Directory: dist
Install Command: npm install
Development Command: npm run dev
```

### 2.3 Variables de Entorno

En **"Settings"** → **"Environment Variables"**, añade:

```
VITE_API_URL = https://tu-app.onrender.com
```

(Reemplaza con la URL real de tu API en Render)

### 2.4 Deploy

- Click en **"Deploy"**
- Vercel hará build y deploy automáticamente
- Tu frontend estará en: `https://tu-proyecto.vercel.app`

---

## 🗄️ Paso 3: Configurar Base de Datos Aiven (Existente)

La BD ya está en Aiven, pero hay que asegurarse de que el Firewall permita conexiones:

### 3.1 Permitir IPs en Aiven

1. Ve a [https://console.aiven.io](https://console.aiven.io)
2. Selecciona tu servicio PostgreSQL
3. Ve a **"Networking"** → **"IP Filter"**
4. Añade:
   - IP de Render (puedes usar `0.0.0.0/0` para testing, no ideal para producción)
   - IP de Vercel: `76.76.19.0/24` (opcional, para API calls desde frontend)

---

## 🔗 Verificar la Integración

### ✅ La API debe responder:

```bash
curl https://tu-app.onrender.com/swagger/index.html
```

### ✅ El Frontend debe cargar:

```
https://tu-proyecto.vercel.app
```

### ✅ El Frontend debe conectar con API:

- Abre DevTools (F12) → Network
- Realiza una acción que haga llamada a la API
- Verifica que la request vaya a `https://tu-app.onrender.com/api/...`

---

## 📝 Variables de Entorno Necesarias

### En Render (para la API)

```env
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:8080
DB_HOST=casa106-db-casa106.i.aivencloud.com
DB_PORT=16228
DB_NAME=defaultdb
DB_USER=avnadmin
DB_PASSWORD=<tu-password-aiven>
CLOUDINARY_CLOUD_NAME=<opcional>
CLOUDINARY_API_KEY=<opcional>
CLOUDINARY_API_SECRET=<opcional>
```

### En Vercel (para el Frontend)

```env
VITE_API_URL=https://tu-app.onrender.com
```

---

## 🐛 Troubleshooting

### El Frontend no conecta con la API

1. Verifica que `VITE_API_URL` esté correcto en Vercel
2. Comprueba CORS en `Program.cs` (debe permitir origen de Vercel)
3. Verifica que la API en Render esté corriendo: `curl https://tu-app.onrender.com/health` (si existe ese endpoint)

### La API no conecta a la base de datos

1. Verifica credenciales Aiven en variables de entorno de Render
2. Verifica firewall IP en Aiven Console
3. Mira logs en Render: Dashboard → Logs

### Free tier lentitud

- Render pausa apps inactivas después de 15 minutos
- Primera solicitud después de pausa tarda ~30 segundos (cold start)
- Para mejorar: actualiza a plan pagado

---

## 📊 Monitoreo

### Render Logs

```
Dashboard → tu-app → Logs
```

### Vercel Logs

```
Dashboard → tu-proyecto → Deployments → Logs
```

### Aiven PostgreSQL

```
Console Aiven → tu-servicio → Metrics
```

---

## 💡 Tips y Optimizaciones

1. **Para desarrollo local**, usa:
   ```bash
   dotnet run --configuration Debug
   cd src/Casa106.Web && npm run dev
   ```

2. **Para testing de producción**, usa HTTPS:
   ```bash
   dotnet run --urls "https://localhost:7210"
   ```

3. **Actualizar sin downtime**:
   - Push a `main`
   - Render/Vercel harán redeploy automático
   - Cero downtime para frontend, segundos para API

4. **Monitorea costos**:
   - Render Free: $0
   - Vercel Free: $0
   - Aiven: $25/mes (ya lo tienes)
   - **Total**: $25/mes

---

## 🚨 Notas Importantes

⚠️ **No commits secrets directamente**:
- Usa `.env.local` (gitignored)
- Usa variables de entorno en Render y Vercel

⚠️ **Aiven Firewall**:
- Por defecto restrictivo
- Necesitas permitir IPs de Render

⚠️ **Cold starts en free tier**:
- Primer request después de pausa: ~30 segundos
- Es normal

---

## 🎯 Próximos Pasos

1. ✅ Push al repositorio `main`
2. ✅ Crear web service en Render
3. ✅ Crear project en Vercel
4. ✅ Configurar variables de entorno
5. ✅ Monitorear logs en primeros deploy
6. ✅ Testar flujo completo (frontend → API → BD)

¡Listo para producción! 🚀

---

## 📞 Soporte

- **Render Docs**: https://render.com/docs
- **Vercel Docs**: https://vercel.com/docs
- **Aiven Docs**: https://docs.aiven.io
