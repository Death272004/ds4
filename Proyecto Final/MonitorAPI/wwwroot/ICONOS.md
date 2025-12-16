# Iconos de Periféricos

Este proyecto utiliza **Font Awesome** para los iconos de periféricos, por lo que NO necesitas descargar imágenes PNG.

Los iconos se cargan automáticamente desde el CDN de Font Awesome en el archivo `index.html`.

## Iconos Utilizados

El sistema mapea automáticamente cada tipo de periférico a su icono correspondiente:

| Tipo de Periférico | Icono Font Awesome | Código              |
| ------------------ | ------------------ | ------------------- |
| Mouse              | 🖱️                 | `fa-computer-mouse` |
| Teclado            | ⌨️                 | `fa-keyboard`       |
| Monitor            | 🖥️                 | `fa-desktop`        |
| USB                | 🔌                 | `fa-usb`            |
| Impresora          | 🖨️                 | `fa-print`          |
| Auriculares        | 🎧                 | `fa-headphones`     |
| Cámara             | 📷                 | `fa-video`          |
| Micrófono          | 🎤                 | `fa-microphone`     |
| Disco              | 💾                 | `fa-hard-drive`     |
| Red                | 🌐                 | `fa-network-wired`  |

## Si Prefieres Usar Imágenes PNG Personalizadas

Si deseas usar tus propias imágenes PNG en lugar de iconos de Font Awesome:

### 1. Crea las Imágenes

Crea imágenes PNG de 128x128 píxeles con nombres como:

- `mouse.png`
- `keyboard.png`
- `monitor.png`
- `usb.png`
- `printer.png`
- `headphones.png`
- `webcam.png`
- `microphone.png`
- `harddrive.png`
- `network.png`

### 2. Coloca las Imágenes

Guarda las imágenes en:

```
MonitorAPI/wwwroot/images/devices/
```

### 3. Modifica el JavaScript

En `wwwroot/js/app.js`, cambia la función `renderPeripherals`:

```javascript
// En lugar de usar iconos de Font Awesome:
// <i class="fas ${icon}"></i>

// Usa imágenes:
<img src="/images/devices/${device.icono}" alt="${device.tipoPerifico}" style="width: 80px; height: 80px;">
```

## Recursos Gratuitos para Iconos

Si necesitas crear o descargar iconos, aquí hay recursos gratuitos:

### Sitios de Iconos Gratuitos

- **Flaticon**: https://www.flaticon.com/
- **Icons8**: https://icons8.com/
- **Font Awesome**: https://fontawesome.com/ (ya incluido)
- **Heroicons**: https://heroicons.com/
- **Feather Icons**: https://feathericons.com/

### Herramientas para Crear Iconos

- **Canva**: https://www.canva.com/ (gratis con opciones limitadas)
- **Figma**: https://www.figma.com/ (gratis para uso personal)
- **Inkscape**: https://inkscape.org/ (gratis y open source)

## Nota Importante

El sistema actual está configurado para usar **Font Awesome**, que:

- ✅ No requiere descargar archivos
- ✅ Se carga automáticamente desde CDN
- ✅ Ofrece más de 2000 iconos gratuitos
- ✅ Es escalable (vectorial)
- ✅ Se ve perfecto en cualquier resolución

Por lo tanto, **no necesitas hacer nada más** para que los iconos funcionen correctamente.
