# XR Interaction Challenge — EC_XR_IngaDiego

| Campo | Valor |
|---|---|
| Estudiante | Inga Diego |
| Código del estudiante | `PENDIENTE` |
| Curso | Laboratorio de Realidad Extendida (XR) para Videojuegos |
| Docente | Victor Alejandro Arroyo Castro |

## Descripción

Sala de entrenamiento XR construida en Unity 6 con URP y XR Interaction Toolkit. El usuario puede recorrer la sala mediante teletransporte, tomar y lanzar objetos, interactuar a distancia con rayos y usar un panel de UI espacial que genera nuevos objetos y los contabiliza.

Escena principal: `Assets/Scenes/EC_XR_IngaDiego.unity`

## Funcionalidades implementadas

| Requerimiento | Implementación |
|---|---|
| Configuración XR | URP, XR Plug-in Management, OpenXR y XR Interaction Toolkit 3.6.1 (Starter Assets + XR Device Simulator) |
| Escenario | Piso, luz direccional, luz puntual de la sala, cuatro paredes que delimitan el espacio, mesa, pilar y objetos interactivos (más de cinco objetos 3D) |
| Objetos manipulables | `Grab Cube`, `Grab Sphere` y `Grab Tool` con `Rigidbody` y `XR Grab Interactable` |
| Interacción a distancia (Ray) | `Light Switch`: enciende y apaga la luz de la sala. `Color Changer`: cambia de color en cada selección |
| Reto libre | Teletransporte sobre el piso (`Teleportation Area`), lanzamiento de objetos (`Throw On Detach`) y UI espacial que hace aparecer objetos con contador |

## Controles

Con visor (OpenXR):

- **Grip**: tomar objetos cercanos o seleccionar con el rayo (interruptor de luz y objeto que cambia de color).
- **Soltar el grip en movimiento**: lanzar el objeto.
- **Gatillo sobre el panel**: presionar el botón *Spawn object*.
- **Joystick hacia adelante**: apuntar y soltar para teletransportarse.

En el editor con XR Device Simulator:

- **WASD + mouse (clic derecho)**: mover y rotar la vista.
- **Shift izquierdo / Espacio**: controlar el mando izquierdo o el derecho.
- **G**: grip (tomar o seleccionar). **T**: gatillo (UI).

## Capturas

1. Vista general del escenario

   ![Vista general](Docs/overview.png)

2. Configuración XR y componentes en el Inspector

   ![Inspector](Docs/inspector.png)

3. Interacción funcionando

   ![Interacción](Docs/interaction.png)

## Video demostrativo

[Ver video (máximo 1 minuto)](PENDIENTE)

## Cómo abrir el proyecto

1. Clonar el repositorio.
2. Abrir la carpeta con Unity **6000.3.14f1**.
3. Abrir `Assets/Scenes/EC_XR_IngaDiego.unity` y presionar Play.
4. Opcional: el menú **EC XR > Build Scene** reconstruye la escena desde cero.

## Tecnologías y paquetes

- Unity 6000.3.14f1
- Universal Render Pipeline 17.3.0
- XR Interaction Toolkit 3.6.1
- XR Plug-in Management 4.6.1
- OpenXR Plugin 1.18.0
- Input System 1.19.0
- C# (scripts propios en `Assets/Scripts` y `Assets/Editor`)
