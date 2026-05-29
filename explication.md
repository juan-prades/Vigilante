# Explicación técnica del juego

## Resumen general

El proyecto es un juego de supervivencia en primera persona hecho en Unity. El jugador debe moverse por una base, evitar enemigos, recoger monedas y llegar al panel final para ganar.

La misión actual es:

`Encontrar 3 monedas y salir por el panel de control final sin ser atrapado.`

El juego tiene estas partes principales:

- Menú principal con botones de jugar, instrucciones y opciones.
- Control del jugador en primera persona.
- Cambio de cámara entre primera persona, tercera persona y cenital.
- Sistema de luces, linterna y luces parpadeantes.
- Enemigos que patrullan, persiguen, atacan o disparan.
- Sistema de monedas recogibles.
- UI de misión, victoria, derrota, pausa, instrucciones y opciones.
- Música y efectos de sonido.

## Escenas

### `Assets/Scenes/Menu.unity`

Es la pantalla inicial del juego.

Contiene un `Canvas` con el menú principal. Desde aquí se puede:

- Pulsar `JUGAR` para cargar la escena de juego.
- Pulsar `INSTRUCCIONES` para ver controles y objetivo.
- Pulsar `OPCIONES` para modificar el volumen.

El script principal de esta escena es `MenuPrincipal.cs`.

### `Assets/Scenes/SampleScene.unity`

Es la escena principal jugable.

Contiene:

- El jugador.
- Enemigos.
- Monedas.
- Luces.
- Canvas de juego.
- Paneles de victoria y derrota.
- Panel de pausa con instrucciones y opciones.
- Panel final de meta.

El script principal de UI de esta escena es `UIManager.cs`.

## Scripts de juego

### `MenuPrincipal.cs`

Controla el menú principal.

Variables importantes:

- `escenaJuego`: nombre de la escena que se carga al jugar. Actualmente apunta a `SampleScene`.
- `sonidoMenu`: música o sonido que se reproduce en el menú.
- `panelInstrucciones`: panel que contiene las instrucciones.
- `panelOpciones`: panel de opciones del menú.
- `sliderVolumen`: slider que controla el volumen global.
- `audioMenu`: `AudioSource` creado por código para reproducir el sonido del menú.

Métodos:

- `Start()`:
  - Libera y muestra el ratón.
  - Restaura `Time.timeScale` a `1` por si venimos de una partida pausada.
  - Oculta instrucciones y opciones al comenzar.
  - Inicializa el slider con `AudioListener.volume`.
  - Llama a `PrepararOpciones()` para crear el botón/panel de opciones si faltan.
  - Crea un `AudioSource` y reproduce `sonidoMenu` en bucle.

- `Jugar()`:
  - Es llamado por el botón `JUGAR`.
  - Carga la escena indicada por `escenaJuego` usando `SceneManager.LoadScene`.

- `Salir()`:
  - Llama a `Application.Quit()` para cerrar el juego.
  - En el editor no cierra Unity, pero en build sí cerraría la aplicación.

- `MostrarInstrucciones()`:
  - Activa `panelInstrucciones`.
  - Usa `SetAsLastSibling()` para que el panel quede por encima de botones como `OPCIONES`.

- `OcultarInstrucciones()`:
  - Desactiva `panelInstrucciones`.

- `MostrarOpciones()`:
  - Activa `panelOpciones`.
  - Lo pone por encima del resto con `SetAsLastSibling()`.

- `OcultarOpciones()`:
  - Desactiva `panelOpciones`.

- `CambiarVolumen(float volumen)`:
  - Cambia `AudioListener.volume`.
  - Esto afecta al volumen global de todo el juego.

- `PrepararOpciones()`:
  - Busca el `Canvas`.
  - Si no existe `BotonOpciones`, lo crea.
  - Si no existe `panelOpciones`, crea el panel, el texto, el slider y el botón de volver.
  - Configura el slider para trabajar de `0` a `1`.

- `CrearPanel(...)`:
  - Crea un `GameObject` con `RectTransform` e `Image`.
  - Lo usa como fondo oscuro de panel.

- `CrearBoton(...)`:
  - Crea un botón UI con imagen roja y texto TMP.

- `CrearTexto(...)`:
  - Crea un texto `TextMeshProUGUI` para botones o paneles.

- `CrearSlider(...)`:
  - Crea un slider simple con fondo, relleno rojo y handle blanco.

### `UIManager.cs`

Controla toda la interfaz durante la partida.

Variables importantes:

- `Instance`: Singleton para acceder al UIManager desde otros scripts.
- `panelGanaste`: panel que aparece al ganar.
- `panelPerdiste`: panel que aparece al perder.
- `panelInstrucciones`: instrucciones ingame.
- `panelPausa`: panel que aparece al pulsar `Esc`.
- `panelOpciones`: panel de opciones ingame.
- `sliderVolumen`: slider de volumen ingame.
- `textoMision`: texto superior con objetivo y monedas.
- `monedasObjetivo`: cantidad de monedas necesarias para salir. Actualmente `3`.
- `musicaJuego`: música de fondo de partida.
- `sonidoVictoria`: sonido al ganar.
- `sonidoDerrota`: sonido al perder.
- `sonidoInstrucciones`: sonido al abrir instrucciones.
- `terminado`: evita que se gane o pierda más de una vez.
- `pausado`: indica si el menú de pausa está abierto.
- `instruccionesAbiertas`: indica si el panel de instrucciones está abierto.
- `opcionesAbiertas`: indica si el panel de opciones está abierto.
- `monedas`: contador interno de monedas recogidas.
- `audioMusica`, `audioEfectos`, `audioInstrucciones`: fuentes de sonido creadas por código.

Métodos:

- `Awake()`:
  - Implementa el Singleton.
  - Si ya existe otro `UIManager`, destruye el duplicado.
  - Crea las fuentes de audio para música, efectos e instrucciones.

- `Start()`:
  - Oculta paneles de ganar, perder, instrucciones, pausa y opciones.
  - Inicializa el slider de volumen.
  - Llama a `PrepararMenuPausa()` para crear UI de pausa/opciones si falta.
  - Llama a `PrepararMision()` y `ActualizarMision()`.
  - Reproduce `musicaJuego` si está asignada.

- `Update()`:
  - Detecta `Escape`.
  - Si la partida no ha terminado, abre o cierra la pausa con `CambiarPausa()`.

- `MostrarGanaste()`:
  - Antes de ganar comprueba `TieneTodasLasMonedas()`.
  - Si faltan monedas, no deja ganar.
  - Si se puede ganar, marca la partida como terminada.
  - Detiene la música.
  - Reproduce sonido de victoria.
  - Activa `panelGanaste`.
  - Llama a `PararJuego()`.

- `MostrarPerdiste()`:
  - Marca la partida como terminada.
  - Detiene la música.
  - Reproduce sonido de derrota.
  - Activa `panelPerdiste`.
  - Llama a `PararJuego()`.

- `PararJuego()`:
  - Libera el cursor.
  - Desactiva el `FirstPersonController` del jugador.
  - Desactiva todos los enemigos `seguir`, `patrullero`, `cercano` y `dispara`.
  - Congela el tiempo con `Time.timeScale = 0`.

- `CambiarInstrucciones()`:
  - Alterna entre mostrar y ocultar instrucciones.

- `CambiarPausa()`:
  - Alterna entre mostrar y ocultar el panel de pausa.

- `MostrarPausa()`:
  - Activa el panel de pausa.
  - Lo pone por delante con `SetAsLastSibling()`.
  - Pausa el juego.
  - Libera y muestra el cursor.

- `OcultarPausa()`:
  - Oculta pausa, instrucciones y opciones.
  - Detiene el sonido de instrucciones.
  - Reactiva el tiempo.
  - Bloquea y oculta el cursor.

- `MostrarInstrucciones()`:
  - Muestra las instrucciones ingame.
  - Oculta el panel de pausa.
  - Reproduce sonido de instrucciones.
  - Pausa el juego.

- `OcultarInstrucciones()`:
  - Oculta instrucciones.
  - Vuelve a mostrar el panel de pausa.
  - Detiene el sonido de instrucciones.

- `MostrarOpciones()`:
  - Muestra opciones ingame.
  - Oculta el panel de pausa.
  - Pausa el juego.

- `OcultarOpciones()`:
  - Oculta opciones.
  - Vuelve al panel de pausa.

- `CambiarVolumen(float volumen)`:
  - Cambia `AudioListener.volume`.

- `InstruccionesAbiertas()`:
  - Devuelve `true` si pausa, instrucciones u opciones están abiertas.
  - El jugador lo usa para no moverse mientras hay UI abierta.

- `RecogerMoneda()`:
  - Suma una moneda si no se ha llegado al objetivo.
  - Actualiza el texto de misión.

- `TieneTodasLasMonedas()`:
  - Devuelve `true` cuando `monedas >= monedasObjetivo`.

- `ActualizarMision()`:
  - Escribe el texto: `Misión: encuentra las 3 monedas y sal` y `Monedas: X/3`.

- `PrepararMision()`:
  - Busca el `Canvas`.
  - Si falta `textoMision`, lo crea arriba de la pantalla.

- `PrepararMenuPausa()`:
  - Crea el menú de pausa si no existe.
  - Crea botones de instrucciones, opciones y volver.
  - Crea panel de opciones y slider.

- `CrearPanel(...)`, `CrearBoton(...)`, `CrearTexto(...)`, `CrearSlider(...)`:
  - Métodos auxiliares para construir UI por código si falta en la escena.

### `MetaJuego.cs`

Va en el objeto final, el panel de control o cubo de meta.

Método:

- `OnTriggerEnter(Collider other)`:
  - Detecta si entra el jugador con `CompareTag("Player")`.
  - Si el jugador no tiene todas las monedas, muestra un `Debug.Log` y no deja ganar.
  - Si tiene todas las monedas, llama a `UIManager.Instance.MostrarGanaste()`.

### `Moneda.cs`

Va en el prefab `Coin`.

Variables:

- `recogida`: evita que la misma moneda cuente dos veces.

Método:

- `OnTriggerEnter(Collider other)`:
  - Comprueba si quien toca la moneda es el jugador.
  - Si no había sido recogida, marca `recogida = true`.
  - Llama a `UIManager.Instance.RecogerMoneda()`.
  - Desactiva la moneda con `gameObject.SetActive(false)`.

## Scripts del jugador

### `FirstPersonController.cs`

Controla movimiento, salto, ratón, cámara de primera persona y sonido de salto.

Clases internas:

- `MovementSettings`: velocidad, suavizado, salto y gravedad.
- `MouseSettings`: sensibilidad, suavizado y límites verticales de mirada.
- `CameraSettings`: cámara, altura y FOV.

Métodos:

- `Start()`:
  - Bloquea el cursor.
  - Obtiene el `CharacterController`.
  - Crea un `AudioSource` para el jugador.
  - Busca `Camera.main` si no hay cámara asignada.
  - Hace hija la cámara del jugador si hace falta.
  - Coloca la cámara a la altura configurada.
  - Ajusta el FOV inicial.

- `Update()`:
  - Si se pulsa `Escape` y no hay `UIManager`, libera cursor.
  - Si hay pausa/instrucciones/opciones, no permite mover ni girar.
  - Si se pulsa click izquierdo, vuelve a bloquear el cursor.
  - Llama a `HandleMouseLook()` y `HandleMovement()`.

- `HandleMouseLook()`:
  - Lee `Mouse X` y `Mouse Y`.
  - Aplica rotación horizontal al jugador.
  - Aplica rotación vertical a la cámara.
  - Limita el pitch entre `minPitch` y `maxPitch`.
  - Usa `SmoothDampAngle` para suavizar.

- `HandleMovement()`:
  - Lee ejes `Horizontal` y `Vertical`.
  - Calcula dirección con `transform.right` y `transform.forward`.
  - Suaviza la velocidad con `Vector3.SmoothDamp`.
  - Aplica gravedad.
  - Si está en suelo y se pulsa salto, aplica `jumpForce` y reproduce `sonidoSalto`.
  - Mueve con `CharacterController.Move`.

- `SetFOV(float fov)`:
  - Cambia el FOV de la cámara dentro de los límites configurados.

## Scripts de cámara

### `CameraSwitcher.cs`

Permite cambiar entre tres modos de cámara.

Variables:

- `player`: jugador a seguir.
- `mainCamera`: cámara principal.
- `thirdPersonOffset`: separación para tercera persona.
- `topDownOffset`: separación para cámara cenital.
- `currentView`: vista actual.

Métodos:

- `Start()`:
  - Busca al jugador por tag `Player` si no está asignado.
  - Usa `Camera.main` si no hay cámara asignada.
  - Empieza en tercera persona.

- `Update()`:
  - `T`: primera persona.
  - `Y`: tercera persona.
  - `U`: cenital.
  - Llama a `FollowPlayer()`.

- `FollowPlayer()`:
  - En primera persona coloca la cámara sobre el jugador.
  - En tercera persona interpola hacia `player.position + thirdPersonOffset` y mira al jugador.
  - En cenital interpola hacia `player.position + topDownOffset` y rota a 90 grados.

- `SetFirstPersonView()`, `SetThirdPersonView()`, `SetTopDownView()`:
  - Cambian el valor de `currentView`.

## Scripts de luces

### `ControlLuz.cs`

Controla la intensidad de una luz con teclado.

Método:

- `Update()`:
  - `1`: sube intensidad.
  - `2`: baja intensidad y no permite valores negativos.
  - `3`: apaga la luz poniendo intensidad a 0.

### `Linterna.cs`

Controla la linterna del jugador.

Métodos:

- `Start()`:
  - Crea un `AudioSource`.
  - Apaga la linterna al empezar.

- `Update()`:
  - `4`: enciende la linterna, fija color e intensidad, reproduce sonido.
  - `5`: apaga la linterna y reproduce sonido.

### `BateriaLinterna.cs`

Gestiona batería de la linterna.

Método:

- `Update()`:
  - Si la linterna está encendida, reduce batería con el tiempo.
  - Si llega a 0, apaga la linterna.
  - Actualiza una barra UI con `fillAmount`.

### `LuzZonalTitileo.cs`

Controla una luz zonal parpadeante.

Métodos:

- `Start()`:
  - Pone la luz encendida o apagada según `encendida`.

- `Update()`:
  - `7`: alterna encendido/apagado.
  - Si está encendida, usa `Mathf.PerlinNoise` para variar la intensidad suavemente.

## Scripts de enemigos

### `patrullero.cs`

Es la araña patrullera.

Variables:

- `patrolSpeed`: velocidad de patrulla.
- `radioDeambular`: radio donde busca puntos aleatorios.
- `player`: jugador a detectar.
- `chaseSpeed`: velocidad de persecución.
- `detectionRange`: distancia para empezar a perseguir.
- `loseRange`: distancia para dejar de perseguir.
- `rangoAtaque`: distancia a la que mata al jugador.
- `sonidoAviso`: sonido al detectar al jugador.
- `agente`: `NavMeshAgent`.
- `audioAviso`: fuente de audio.
- `persiguiendo`: estado interno.

Métodos:

- `Start()`:
  - Obtiene `NavMeshAgent`.
  - Crea `AudioSource`.
  - Busca primer destino con `IrAOtroSitio()`.

- `Update()`:
  - Calcula distancia al jugador.
  - Si entra en `detectionRange`, activa persecución y reproduce sonido.
  - Si supera `loseRange`, deja de perseguir.
  - Si persigue, llama a `Perseguir()`.
  - Si no persigue, llama a `Deambular()`.
  - Si está a `rangoAtaque`, llama a `UIManager.Instance.MostrarPerdiste()`.

- `Deambular()`:
  - Ajusta velocidad de patrulla.
  - Si llegó al destino, busca otro punto.

- `IrAOtroSitio()`:
  - Genera una posición aleatoria alrededor.
  - Usa `NavMesh.SamplePosition` para encontrar un punto válido.
  - Manda al agente allí.

- `Perseguir()`:
  - Ajusta velocidad de persecución.
  - Manda al agente a la posición del jugador.

### `seguir.cs`

Es el fantasma que persigue siempre al jugador.

Variables:

- `animator`: animador del enemigo.
- `MoveSpeed`: velocidad de persecución.
- `rangoAtaque`: distancia de ataque.
- `sonidoFantasma`: sonido al atacar.
- `agente`: `NavMeshAgent`.
- `audioFantasma`: fuente de audio.
- `atacando`: evita repetir sonido cada frame.

Métodos:

- `Start()`:
  - Obtiene `Animator` y `NavMeshAgent`.
  - Crea `AudioSource`.

- `Update()`:
  - Busca al jugador por tag.
  - Ajusta velocidad y destino hacia el jugador.
  - Si está a distancia de ataque, activa trigger `ataca`.
  - Reproduce sonido una vez por ataque.
  - Llama a `MostrarPerdiste()` si alcanza al jugador.
  - Si se aleja, permite que el sonido vuelva a sonar en el siguiente ataque.

### `cercano.cs`

Enemigo simple que se activa por proximidad.

Método:

- `Update()`:
  - Calcula distancia al jugador.
  - Si entra en `activationRange`, activa seguimiento.
  - Si sale de `stopRange`, se desactiva.
  - Si está activo, se mueve hacia el jugador usando `transform.position`.

### `dispara.cs`

Controla una torreta o enemigo que dispara.

Variables:

- `player`: jugador.
- `shootRange`: distancia máxima para disparar.
- `bulletPrefab`: prefab de bala.
- `shootPoint`: punto desde donde sale la bala.
- `fireRate`: disparos por segundo.
- `bulletSpeed`: velocidad de la bala.
- `fireCooldown`: tiempo restante hasta poder disparar otra vez.

Métodos:

- `Update()`:
  - Si no hay jugador, sale.
  - Calcula distancia.
  - Si está dentro de rango, gira hacia el jugador.
  - Si el cooldown llega a 0, llama a `Disparar()` y reinicia cooldown.

- `Disparar()`:
  - Instancia una bala en `shootPoint`.
  - Si la bala tiene `Rigidbody`, le asigna velocidad hacia delante.

### `bala.cs`

Controla la vida de los proyectiles.

Métodos:

- `Start()`:
  - Destruye la bala automáticamente tras `lifeTime` segundos.

- `OnCollisionEnter(Collision other)`:
  - Destruye la bala al chocar con algo.

## Scripts de sonido

### `SoundManager.cs`

Gestor de sonido genérico con Singleton.

Métodos:

- `Awake()`:
  - Crea Singleton.
  - Usa `DontDestroyOnLoad` para mantenerse entre escenas.
  - Crea dos `AudioSource`: música y efectos.
  - Configura música en bucle.

- `Start()`:
  - Llama a `PlayBackgroundMusic()`.

- `Update()`:
  - Actualiza volúmenes de música y efectos.

- `PlayBackgroundMusic()`:
  - Si hay música asignada, la reproduce en bucle.

- `PlayEffectSound(AudioClip clip)`:
  - Reproduce un efecto puntual.

- `PlayZoneSound(AudioClip clip, float volume)`:
  - Reproduce un sonido de zona con volumen específico.

### `ZoneTrigger.cs`

Activa sonido ambiental al entrar en una zona.

Métodos:

- `Start()`:
  - Crea un `AudioSource`.
  - Asigna clip, volumen y loop.

- `OnTriggerEnter(Collider other)`:
  - Si entra el jugador, reproduce el sonido si no estaba sonando.

- `OnTriggerExit(Collider other)`:
  - Si sale el jugador, detiene el sonido.

## Asset de monedas

### `SimpleGemsAnim.cs`

Este script viene con el asset de monedas.

Está dentro de namespace `Benjathemaker`, por eso no sigue el estilo propio del proyecto.

Métodos:

- `Start()`:
  - Guarda escala y posición iniciales.
  - Ajusta escalado inicial/final.

- `Update()`:
  - Si `isRotating` está activo, rota la moneda.
  - Si `isFloating` está activo, hace que suba y baje.
  - Si `isScaling` está activo, cambia escala suavemente.

- `EaseInOutQuad(float t)`:
  - Función matemática para suavizar animaciones.

## Flujo de victoria

1. El jugador recoge monedas.
2. Cada moneda llama a `UIManager.RecogerMoneda()`.
3. El UI cambia de `0/3` a `1/3`, `2/3`, `3/3`.
4. El jugador llega al panel final.
5. `MetaJuego.OnTriggerEnter()` comprueba si tiene todas las monedas.
6. Si faltan monedas, no pasa nada salvo `Debug.Log`.
7. Si tiene todas, llama a `UIManager.MostrarGanaste()`.
8. Se reproduce sonido de victoria, aparece panel de ganar y se congela el juego.

## Flujo de derrota

1. Un enemigo alcanza al jugador.
2. El enemigo llama a `UIManager.MostrarPerdiste()`.
3. Se reproduce sonido de derrota.
4. Aparece panel de perder.
5. Se desactiva el control del jugador.
6. Se desactivan enemigos.
7. Se congela el juego.

## Flujo de pausa

1. El jugador pulsa `Esc`.
2. `UIManager.Update()` llama a `CambiarPausa()`.
3. Se muestra el panel de pausa.
4. `Time.timeScale` pasa a `0`.
5. El cursor se libera.
6. Desde pausa se puede abrir instrucciones u opciones.
7. Al volver se ocultan los paneles, `Time.timeScale` vuelve a `1` y se bloquea el cursor.

## Sonidos asignados

- `menú_e_instrucciones.mp3`: menú e instrucciones.
- `in_game.mp3`: música de partida.
- `epic-victory.mp3`: victoria.
- `gameover.mp3`: derrota.
- `araña.mp3`: araña al detectar al jugador.
- `fantasma.mp3`: fantasma al atacar.
- `jumping.mp3`: salto.
- `linterna_on_off.mp3`: encender/apagar linterna.

## Luces

El proyecto usa Built-In Render Pipeline.

Se subió `pixelLightCount` en calidad `Ultra` para que Unity renderice más luces por píxel. Esto ayuda a que varias `Point Light` no se vean tan raras cuando se solapan.

Si las luces siguen viéndose extrañas, las causas probables son:

- Muchas luces con rango alto solapándose.
- Sombras desactivadas en varias luces.
- Intensidades similares iluminando el mismo espacio.
- Luces dinámicas en Built-In con límite de calidad.
