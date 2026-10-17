# Búsqueda Medieval - Previo 2

**Serious Game 2D en Unity** | Historia medieval | Prototipo académico (octubre de 2026).

Integrantes: **Yezid Santiago Fontecha Pazos** y **Samuel David Gonzales Saavedra**.

## Requisitos
- Unity 6.3 LTS, versión `6000.3.24f1`.
- Abrir la carpeta del proyecto desde Unity Hub, no la escena individual.
- Abrir `Assets/Scenes/MainMenu.unity` para iniciar.

## Mecánicas
- **A/D / flechas:** caminar.
- **E junto al Sabio Medieval:** abrir y avanzar el diálogo educativo.
- **E cerca de un libro o cartel:** consultar contenido educativo.
- **Recoger cinco pergaminos:** colisión `Is Trigger`, un punto por objeto.
- **HUD:** contador `0/5` a `5/5`, objetivo actual y cinco casillas visuales.
- **Al completar la misión:** mensaje de logro y opciones de menú/reinicio.
- **Muros de piedra con colisión:** a la izquierda del sabio y a la derecha del quinto pergamino. Son barreras físicas visibles y no impiden recoger los cinco manuscritos (P1).
- **Ventanas exclusivas:** no se puede abrir un segundo diálogo ni una lectura mientras otro panel esté abierto (P5).

## Estructura
`Assets/Scenes`: menú y nivel jugable.
`Assets/Scripts/Previo2`: NPCDialogue, InteractableObject, CollectibleItem, GameHUDManager y MontajePrevio2.
`Assets/Sprites`, `Assets/Resources`: pixel art y tiles.

## Pruebas
Consultar `DOCUMENTACION/PLAN_PRUEBAS.md` para los casos P1-P10. La realización
de las pruebas y el build final debe hacerse desde el editor de Unity.

## Uso académico
Prototipo educativo sin fines comerciales. Los sprites de ampliación han
sido dibujados para este trabajo; no se incorporan recursos comerciales.

## Diálogos y sonido
La actividad solicita diálogo escrito e interacción por la tecla E; **no exige locuciones grabadas**. El video de sustentación sí debe llevar explicación oral de los estudiantes.

## Cómo comprobar las mejoras
1. Inicia desde `MainMenu` y entra a jugar.
2. En el lado izquierdo, camina más allá del Sabio Medieval: la torre de piedra debe bloquear al jugador (P1).
3. También hay otra barrera después del quinto pergamino, a la derecha.
4. Junto al sabio, pulsa E varias veces para recorrer las frases. Intenta abrir otras ventanas durante el diálogo: no deben solaparse (P5).

**Importante:** la comprobación estática de archivos no sustituye la ejecución real en Unity 6.3.
