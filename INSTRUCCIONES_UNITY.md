# 🎮 Instrucciones para montar "El Despertar del Vigía" en Unity

Esta guía es **paso a paso** y asume que NO eres experto en Unity. Sigue el orden de arriba a abajo. Cuando veas **[Arrastrar]** significa que tienes que coger un objeto de la ventana **Hierarchy** (la lista de la izquierda) y soltarlo en una casilla del **Inspector** (panel de la derecha).

> Los 14 scripts ya están creados en `Assets/Scripts/`. Aquí solo montas la escena y arrastras las referencias.

---

## 0) Antes de empezar (IMPORTANTE)

### 0.1 Input System (ya lo dejé configurado)
He cambiado por ti el ajuste **Active Input Handling** a **"Both"**, porque el código del profe usa `Input.GetKeyDown` y el proyecto venía configurado solo para el sistema nuevo (habría dado errores).

👉 **Comprueba que se aplicó:** menú `Edit > Project Settings… > Player > Other Settings > Active Input Handling` debe poner **Both**.
- Si pone otra cosa, cámbialo a **Both** a mano.
- Unity puede pedirte **reiniciar el Editor**. Acepta (Apply / Restart). Si tenías Unity abierto cuando se generaron los archivos, ciérralo y ábrelo otra vez.

### 0.2 Abre la escena
Abre `Assets/Scenes/SampleScene.unity` (doble clic). Trabajaremos sobre ella.

### 0.3 Crea las Tags que usaremos
Arriba del Inspector hay un desplegable **Tag**. Pulsa `Add Tag…` y crea estas dos si no existen:
- **Player**
- **Enemy** (opcional, no es obligatoria para que funcione)

---

## 1) El suelo 🟫

1. En la Hierarchy: clic derecho → `3D Object > Plane`.
2. Renómbralo a **Suelo**.
3. En el Inspector, pon **Transform > Scale** en `X=5, Y=1, Z=5` (para que sea grande).
4. Position `X=0, Y=0, Z=0`.

*(El Plane ya trae un Mesh Collider, así que el jugador no se cae.)*

---

## 2) El Jugador (primera persona) 🧍

1. Hierarchy → clic derecho → `3D Object > Capsule`. Renómbralo a **Jugador**.
2. Position `X=0, Y=1, Z=0`.
3. Arriba del Inspector, despliega **Tag** y elige **Player**.
4. **Quita el collider de cápsula** para que no choque con el del controlador: en el componente **Capsule Collider**, clic en los 3 puntitos `⋮` → `Remove Component`.
5. **Add Component** (botón abajo del Inspector) → escribe `FirstPersonController` → selecciónalo.
   - Al añadirlo, Unity añade **automáticamente** un **Character Controller** (el script lo exige). Déjalo como está.
6. La **cámara**: el script coge automáticamente la *Main Camera* de la escena, la hace hija del Jugador y la coloca a la altura de los ojos. **No tienes que arrastrar nada** en `Camera Settings > Player Camera` (déjalo vacío, usará la Main Camera).

**Controles del jugador:** WASD para moverte, ratón para mirar, **Espacio** para saltar, **Esc** libera el ratón, clic izquierdo lo vuelve a bloquear.

---

## 3) La linterna 🔦

1. Selecciona el **Jugador** en la Hierarchy.
2. Clic derecho sobre él → `Light > Spot Light`. Así la linterna es **hija** del jugador y le sigue.
3. Renómbrala a **Linterna**. Position (local) `X=0, Y=0, Z=0`, Rotation `X=0` (que apunte al frente).
4. Selecciona otra vez el **Jugador** → **Add Component** → `Linterna`.
5. En el componente **Linterna**, casilla `Linterna`: **[Arrastrar]** el Spot Light "Linterna" desde la Hierarchy.

**Controles:** tecla **4** enciende (con color aleatorio), tecla **5** apaga.

---

## 4) Luz general de la base 💡

1. Hierarchy → clic derecho → `Light > Point Light` (o usa la *Directional Light* que ya existe). Renómbrala **LuzBase**. Súbela, p. ej. Position `Y=5`.
2. Crea un objeto vacío para el script: clic derecho → `Create Empty`, renómbralo **GestorLuces**.
3. Selecciona **GestorLuces** → **Add Component** → `ControlLuz`.
4. Casilla `Luz`: **[Arrastrar]** la luz **LuzBase**.

**Controles:** **1** sube intensidad, **2** baja, **3** apaga del todo.

---

## 5) Luz zonal que parpadea ✨

1. Hierarchy → `Light > Point Light`. Renómbrala **LuzZonal**. Colócala donde quieras (p. ej. encima de una zona).
2. Selecciónala y **Add Component** → `LuzZonalTitileo`.
3. Casilla `Luz Zonal`: **[Arrastrar]** la propia **LuzZonal**.
4. Marca `Encendida` si quieres que empiece encendida (o déjala apagada).

**Control:** tecla **7** la enciende/apaga. Cuando está encendida, parpadea sola.

---

## 6) Enemigo PATRULLERO 🤖 (patrulla y te persigue)

1. Crea el enemigo: `3D Object > Cube` (o Capsule). Renómbralo **Patrullero**. Súbelo a `Y=1`.
2. Crea los **puntos de patrulla**: clic derecho → `Create Empty` varias veces (p. ej. 3). Renómbralos **Punto1, Punto2, Punto3** y colócalos repartidos por el suelo.
3. Selecciona **Patrullero** → **Add Component** → `patrullero`.
4. En el componente:
   - `Puntos`: pon el tamaño (Size) en **3** y **[Arrastrar]** Punto1, Punto2, Punto3 a los huecos Element 0, 1, 2.
   - `Player`: **[Arrastrar]** el **Jugador**.
   - Los demás valores (velocidades, rangos) puedes dejarlos por defecto.

Si te detecta (rango 6) te persigue; si te alejas (rango 9) vuelve a patrullar.

---

## 7) Enemigo CERCANO 👻 (se activa al acercarte)

1. `3D Object > Cube`. Renómbralo **Cercano**. `Y=1`.
2. **Add Component** → `cercano`.
3. `Player`: **[Arrastrar]** el **Jugador**.

Se activa cuando te acercas (8 m) y se para si te alejas (12 m).

---

## 8) Enemigo SEGUIR 🕷️ (la "araña" con animaciones)

> ⚠️ Este script usa un **Animator con animaciones**. Si no tienes un modelo animado con un Animator Controller que tenga los triggers **`saltar`**, **`muere`** y **`ataca`**, el enemigo se moverá pero saldrán *warnings* al usar esos triggers (no rompe el juego). Para una entrega básica puedes usar un Cube y simplemente verás los `Debug.Log`.

1. `3D Object > Cube`. Renómbralo **Araña**. `Y=1`.
2. Asegúrate de que tiene un **Collider** (el Cube ya trae Box Collider) para que detecte choques.
3. **Add Component** → `seguir`.
4. (Opcional) **Add Component** → `Animator` y asígnale un **Animator Controller** con los triggers `saltar`, `muere`, `ataca`. Casilla `Animator` del script: **[Arrastrar]** ese Animator (o déjalo, el script intenta cogerlo solo en `Start`).
5. **No hay casilla `player`**: este script busca al jugador solo por su **Tag "Player"** (ya se la pusiste en el paso 2).

**Controles que usa:** **Espacio** dispara la animación "saltar", **clic izquierdo** la mata (`vive = 0`).

---

## 9) Torreta que DISPARA 🔫 + la BALA

### 9.1 Crear el prefab de la bala
1. `3D Object > Sphere`. Renómbrala **Bala**. Escálala pequeña (`Scale 0.3, 0.3, 0.3`).
2. **Add Component** → `Rigidbody` (¡obligatorio!, el script mueve la bala con física).
3. **Add Component** → `bala`.
4. Crea una carpeta `Assets/Prefabs` (en la ventana Project: clic derecho → `Create > Folder`).
5. **[Arrastrar]** la **Bala** desde la Hierarchy hasta la carpeta `Assets/Prefabs`. Ahora es un **prefab** (se pondrá azul).
6. Borra la **Bala** que quedó en la Hierarchy (ya tienes el prefab guardado).

### 9.2 Crear la torreta
1. `3D Object > Cube`. Renómbralo **Torreta**. `Y=1`.
2. Crea el punto de disparo: selecciona **Torreta** → clic derecho → `Create Empty`, renómbralo **ShootPoint**. Colócalo justo delante de la torreta (p. ej. local `Z=1`).
3. Selecciona **Torreta** → **Add Component** → `dispara`.
4. En el componente:
   - `Player`: **[Arrastrar]** el **Jugador**.
   - `Bullet Prefab`: **[Arrastrar]** el prefab **Bala** desde `Assets/Prefabs`.
   - `Shoot Point`: **[Arrastrar]** el **ShootPoint**.

Cuando te acercas (8 m) la torreta te mira y dispara balas.

---

## 10) Sonido 🔊

### 10.1 SoundManager
1. Crea la carpeta `Assets/Audio` (Project → clic derecho → `Create > Folder`) y mete ahí los sonidos que descargues (ver sección 13).
2. Hierarchy → `Create Empty`, renómbralo **SoundManager**.
3. **Add Component** → `SoundManager`.
4. En el componente:
   - `Background Music`: **[Arrastrar]** tu clip de música de fondo.
   - `Hit Sound`: clip para efecto (tecla E).
   - `Jump Sound`: clip de salto.

### 10.2 Zona con sonido ambiente (ZoneTrigger)
1. `3D Object > Cube`. Renómbralo **ZonaSonido**. Hazlo grande para cubrir un área.
2. En su **Box Collider**, marca la casilla **`Is Trigger`** ✔️ (importante).
3. (Opcional) Quita el componente **Mesh Renderer** para que la zona sea invisible.
4. **Add Component** → `ZoneTrigger`.
5. Casilla `Zone Sound`: **[Arrastrar]** un clip de ambiente.

Cuando el Jugador entra en la zona, suena en bucle; al salir, se para.

---

## 11) Cambio de cámara (1ª / 3ª / cenital) 🎥

> ⚠️ **Nota:** el `FirstPersonController` del Jugador ya controla la cámara en primera persona. El `CameraSwitcher` también mueve la *Main Camera*. Si activas los dos, pueden "pelearse" por la cámara. Para una demo de las 3 vistas, lo más limpio es probar el `CameraSwitcher` **desactivando temporalmente** el `FirstPersonController` (quitando el check de su componente). Móntalo así y decides tú:

1. Hierarchy → `Create Empty`, renómbralo **GestorCamara**.
2. **Add Component** → `CameraSwitcher`.
3. En el componente:
   - `Player`: **[Arrastrar]** el **Jugador**.
   - `Main Camera`: **[Arrastrar]** la **Main Camera** (si lo dejas vacío, coge `Camera.main` sola).

**Controles:** **T** = primera persona, **Y** = tercera persona, **U** = cenital.

---

## 12) La META para ganar 🏁 + los carteles de UI

### 12.1 El cubo de meta
1. `3D Object > Cube`. Renómbralo **Meta**. Colócalo al final del recorrido. Puedes ponerle un material de color llamativo.
2. En su **Box Collider**, marca **`Is Trigger`** ✔️.
3. **Add Component** → `MetaJuego`. (No necesita arrastrar nada.)

### 12.2 Los carteles GANASTE / PERDISTE
1. Hierarchy → clic derecho → `UI > Canvas`. (Se crea también un *EventSystem*, déjalo.)
2. Dentro del Canvas: clic derecho → `UI > Panel`. Renómbralo **PanelGanaste**.
   - Dentro del panel: clic derecho → `UI > Text - TextMeshPro` (si te pide importar TMP Essentials, acepta) o `UI > Legacy > Text`. Escribe **"GANASTE"** y ponlo grande y centrado.
3. Repite: otro `UI > Panel` llamado **PanelPerdiste** con un texto **"PERDISTE"**.
4. Hierarchy → `Create Empty`, renómbralo **UIManager**.
5. **Add Component** → `UIManager`.
6. En el componente:
   - `Panel Ganaste`: **[Arrastrar]** el **PanelGanaste**.
   - `Panel Perdiste`: **[Arrastrar]** el **PanelPerdiste**.

Los dos paneles se ocultan solos al empezar. Cuando el Jugador toca la **Meta**, aparece "GANASTE".

> 💡 Si quieres que algún enemigo muestre "PERDISTE", habría que llamar a `UIManager.Instance.MostrarPerdiste();` desde el script del enemigo al chocar contigo. El método ya está listo; eso sí, tocar el código del enemigo cambiaría el del profe, así que decídelo tú.

---

## 13) Sonidos gratis para descargar 🎵

Descárgalos y mételos en `Assets/Audio/`. Todos son de uso libre:

| Para qué | Dónde buscar (gratis, sin copyright) |
|----------|--------------------------------------|
| 🎼 **Música de fondo** (ambiente terror/tensión) | https://pixabay.com/music/search/horror/ |
| 👣 **Efecto / pasos / golpe** (hit sound) | https://pixabay.com/sound-effects/search/footsteps/ |
| 💥 **Disparo de la torreta** | https://pixabay.com/sound-effects/search/laser/ |
| 🌫️ **Ambiente de zona** (drone/viento) | https://freesound.org/search/?q=ambient%20drone/ |

> En **Pixabay** das a "Download" sin registrarte. En **Freesound** necesitas una cuenta gratis. Formatos `.mp3` o `.wav` valen; arrástralos a `Assets/Audio/` y luego asígnalos en los componentes (paso 10).

---

## 14) Probar el juego ▶️

1. Pulsa el botón **Play** (triángulo, arriba en el centro).
2. Si sale el error rojo *"You are trying to read Input using the UnityEngine.Input class…"* → vuelve al **paso 0.1** (Active Input Handling = Both + reiniciar).
3. Muévete con WASD, mira con el ratón, prueba las teclas 1-7, T/Y/U, llega a la **Meta** y comprueba que sale "GANASTE".

---

## 15) Assets recomendados (piezas sueltas de varios sitios) 🧩

> ⚠️ **Regla del profe:** NO usar un kit/pack entero. Coge **piezas sueltas** de **sitios distintos** y móntalas tú. Todo lo de abajo es gratis. Como el juego es en **primera persona**, NO necesitas modelo de jugador (ves por sus ojos, basta la cápsula).

### De dónde sacar cada cosa

| Para qué | Dónde (varía las fuentes) | Licencia |
|----------|---------------------------|----------|
| 🧱 Texturas de paredes/suelo (metal, hormigón) | ambientCG (https://ambientcg.com) · Poly Haven (https://polyhaven.com/textures) | CC0 |
| 📦 Props sueltos (barril, caja, tubería) | Kenney —un modelo, NO el kit— (https://kenney.nl/assets) · Quaternius (https://quaternius.com) | CC0 |
| 🖥️ Consola/ordenador para la Meta | Sketchfab (https://sketchfab.com, filtro *Downloadable* + *CC*) | CC0 / CC-BY |
| 🔫 Torreta | Sketchfab (busca "turret") · Quaternius (sci-fi) | CC0 / CC-BY |
| 🤖 Enemigos (patrullero, cercano) | Quaternius (robots) · Sketchfab | CC0 / CC-BY |
| 🕷️ Araña animada (opcional, para `seguir.cs`) | Mixamo (https://www.mixamo.com) | Gratis |

### El escenario SIN descargar kits (lo más fácil)
1. **Paredes** = cubos estirados (`3D Object > Cube`, escala fina y larga) formando pasillos y salas.
2. Arrástrales encima una **textura de metal/hormigón** → ya parece una base abandonada.
3. Reparte 3-4 **props sueltos**, cada uno de un sitio distinto (un barril de aquí, una caja de allá).

### Al importar modelos
- Mételos en una carpeta `Assets/Models/`.
- Si un material sale **rosa** → selecciónalo → Shader → `Standard` (es por el Built-In RP).
- Los modelos suelen venir **sin Collider**: añade `Box Collider` a enemigos y props que lo necesiten.
- Suelen ser **pequeños**: súbeles la **Scale** si no los ves.

### Créditos (importante para la nota)
Apunta en **`CREDITOS.txt`** (en la raíz del proyecto) de dónde sacaste cada cosa. Los **CC-BY obligan a citar al autor**; los CC0 no, pero cítalos igual.

---

## 16) Montar el escenario con el Modular Dungeon Kit de Kenney 🏰

> Usar piezas modulares para construir TU propio nivel es diseño propio (no es "usar el kit entero"). Aun así, mete 1-2 props de otra fuente y apúntalo en `CREDITOS.txt`.

### A) Importar el kit
1. **Descomprime** el ZIP descargado.
2. Busca dentro la carpeta **`Models`** en formato **FBX** y la **textura** (tipo `colormap.png`).
3. En Unity → Project → `Create > Folder`: crea `Assets/Models/Dungeon`.
4. **Arrastra** los `.fbx` + la textura a esa carpeta. Unity los importa solos.

### B) Construir las salas y pasillos
5. Hierarchy → `Create Empty`, nómbralo **Escenario** (mete dentro todas las piezas, para tenerlo ordenado).
6. Arrastra una **pieza de suelo** a la escena (Position `0,0,0`).
7. **Activa el snapping** para que encajen perfectas:
   - Mantén **V** al mover una pieza → se pega por las esquinas (vertex snap).
   - O mantén **Ctrl** al arrastrar el gizmo → se mueve en saltos de rejilla.
8. Coloca suelos, paredes, esquinas y puertas hasta formar tus salas. Duplica rápido con **Ctrl+D**.

### C) Colliders (¡o te caes / atraviesas paredes!)
Las piezas vienen **sin collider**:
9. Selecciona suelo y paredes → **Add Component** → `Mesh Collider`.
   - **Atajo:** deja tu Plane **"Suelo"** debajo como colisión del suelo y pon solo `Box Collider` a las paredes.

### D) Ambiente nocturno 🌙
10. `Window > Rendering > Lighting` → pestaña **Environment** → baja la **Intensity** de la luz ambiental (casi a 0).
11. Pon la **Directional Light** muy tenue. Así la Linterna (tecla 4) y las luces (1-3) cobran sentido.

### E) Terminar
12. Coloca dentro del dungeon el **Jugador**, **enemigos**, **luces** y el cubo **Meta** (pasos 2-12 de esta guía).
13. Si una pieza sale **rosa** → material → Shader → `Standard`.

---

## 📌 Notas y avisos que NO son errores tuyos

- **`dispara.cs`** usa `rb.velocity`, que en Unity 6 aparece como **warning amarillo** (lo nuevo es `linearVelocity`). Es código del profe copiado tal cual; el warning **no impide jugar**.
- **`seguir.cs`** llamará a triggers de Animator (`saltar`, `muere`, `ataca`); si el objeto no tiene un Animator con esos parámetros, verás *warnings* al pulsar Espacio/clic, pero el enemigo sigue funcionando.
- **`seguir.cs`** requiere que exista un objeto con Tag **Player** en la escena, o dará error al buscarlo. Ya se la pusiste al Jugador.
- Cámara: recuerda el aviso del **paso 11** (FirstPersonController vs CameraSwitcher).
