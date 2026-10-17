# Validación final - Previo 2

Ninguna prueba se considera superada sin ejecutar Unity o el build.

## Casos P1 a P10

- [ ] **P1** — Caminar hasta los muros de piedra (uno antes del NPC a la izquierda y otro después del 5.º pergamino a la derecha). **Esperado:** El jugador se detiene y no lo atraviesa.
- [ ] **P2** — Pulsar E lejos de cualquier objeto. **Esperado:** No se abre ningún panel.
- [ ] **P3** — Recoger un pergamino y volver a su lugar. **Esperado:** Desaparece y solo suma un punto.
- [ ] **P4** — Pulsar E varias veces frente al NPC. **Esperado:** Avanza cada frase y luego cierra.
- [ ] **P5** — Frente al Sabio Medieval, abrir el diálogo con E y volver a pulsar E rápidamente; intentar abrir una lectura cuando un panel siga activo. **Esperado:** No se solapan paneles.
- [ ] **P6** — Leer los dos objetos informativos. **Esperado:** Se muestra texto educativo legible.
- [ ] **P7** — Reiniciar la escena. **Esperado:** HUD e ítems vuelven a su estado inicial.
- [ ] **P8** — Ejecutar el build de Windows. **Esperado:** Corre sin errores de consola.
- [ ] **P9** — Recoger un pergamino. **Esperado:** Contador e inventario se actualizan.
- [ ] **P10** — Recoger los cinco pergaminos. **Esperado:** Aparece misión cumplida.

## Capturas obligatorias (10)

1. MainMenu con los tres botones.
2. Créditos abiertos.
3. HUD inicial 0/5.
4. Indicador E frente al NPC.
5. Diálogo abierto.
6. Lectura del libro.
7. Lectura del cartel.
8. HUD 1/5 al recoger primer pergamino.
9. Colisiones y seguimiento de cámara.
10. Panel final HUD 5/5.

## Build en Unity 6

File > Build Profiles > Windows > Switch Platform (si hace falta) > Build.
Guardar fuera de Assets, por ejemplo `Builds/Windows/`.
Comprobar que el ZIP contenga el ejecutable y la carpeta `_Data`.
Ejecutar el EXE y repetir P1-P10 antes de comprimir.

## GitHub

Crear repositorio público `busqueda-medieval-previo2`, subir Assets, Packages,
ProjectSettings, README.md y .gitignore. No subir Library ni Temp.
Colocar la URL real en la plataforma; no inventar un enlace.

## Video

Grabar 5 a 10 minutos con voz, usando el guion adjunto.

**Nota sobre los nuevos muros:** Ambos tienen `BoxCollider2D` sólido y un sprite de piedra. El sistema de exclusividad de paneles también fue reforzado en `GameHUDManager`. Las casillas permanecen pendientes hasta la prueba en Unity.
