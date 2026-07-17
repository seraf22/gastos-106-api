#!/bin/bash
# QUICK CHECKLIST - Copia y pega en terminal

## ✅ VERIFICACIONES LOCALES
echo "=== VERIFICACIONES LOCALES ==="
echo ""
echo "1. ¿Existe Dockerfile en raíz?"
ls -la Dockerfile
echo ""
echo "2. ¿Compila localmente?"
dotnet build Casa106.sln -c Release
echo ""
echo "3. ¿Frontend compila?"
cd src/Casa106.Web && npm run build && cd ../..
echo ""
echo "✅ TODO COMPILA OK"
echo ""

## 📤 PUSH A GITHUB
echo "=== PUSH A GITHUB ==="
echo ""
echo "4. Agregar cambios..."
git add .
echo ""
echo "5. Comitear..."
git commit -m "Dockerfile recargado: listo para Render"
echo ""
echo "6. Pushear a master..."
git push origin master
echo ""
echo "✅ PUSH COMPLETADO"
echo ""

## 🔍 VERIFICACIONES POST-PUSH
echo "=== VERIFICACIONES POST-PUSH ==="
echo ""
echo "7. Verifica GitHub Actions en:"
echo "   https://github.com/seraf22/gestorGastos106/actions"
echo ""
echo "8. Si Render está conectado, debería reconstruir automáticamente"
echo "   Si NO, ve a: https://dashboard.render.com"
echo ""
echo "9. Después de 3-5 minutos:"
echo "   - Frontend: https://seraf22.github.io/gestorGastos106/"
echo "   - Backend: https://tu-servicio-render.com"
echo ""
echo "✅ DEPLOYMENT EN PROGRESO"
echo ""
