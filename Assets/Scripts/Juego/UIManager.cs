using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;   // Singleton, igual que el SoundManager

    [Header("Paneles")]
    public GameObject panelGanaste;     // Arrastra aquí el panel de "GANASTE"
    public GameObject panelPerdiste;    // Arrastra aquí el panel de "PERDISTE"
    public GameObject panelInstrucciones; // Panel que muestra las instrucciones
    public GameObject panelPausa;       // Panel que muestra instrucciones y opciones
    public GameObject panelOpciones;    // Panel que muestra el volumen
    public Slider sliderVolumen;        // Slider para subir y bajar el volumen
    public TextMeshProUGUI textoMision; // Texto de la misión y las monedas

    [Header("Misión")]
    public int monedasObjetivo = 3;     // Monedas necesarias para poder salir

    [Header("Sonidos")]
    public AudioClip musicaJuego;       // Música de fondo de la partida
    public AudioClip sonidoVictoria;     // Sonido al ganar
    public AudioClip sonidoDerrota;      // Sonido al perder
    public AudioClip sonidoInstrucciones; // Sonido de instrucciones

    private bool terminado = false;     // Para que solo se muestre un cartel
    private bool pausado = false;       // Para saber si el panel de pausa está abierto
    private bool instruccionesAbiertas = false; // Para saber si el panel está abierto
    private bool opcionesAbiertas = false; // Para saber si las opciones están abiertas
    private int monedas = 0;            // Monedas recogidas por el jugador
    private AudioSource audioMusica;     // Fuente para la música de fondo
    private AudioSource audioEfectos;    // Fuente para victoria y derrota
    private AudioSource audioInstrucciones; // Fuente para las instrucciones

    void Awake()
    {
        // Patrón Singleton: solo puede haber un UIManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Preparar las fuentes de audio
        audioMusica = gameObject.AddComponent<AudioSource>();
        audioEfectos = gameObject.AddComponent<AudioSource>();
        audioInstrucciones = gameObject.AddComponent<AudioSource>();
        audioMusica.loop = true;
        audioMusica.volume = 0.4f;
        audioInstrucciones.loop = true;
        audioInstrucciones.volume = 0.7f;
    }

    void Start()
    {
        // Los dos paneles empiezan ocultos
        if (panelGanaste != null)
            panelGanaste.SetActive(false);

        if (panelPerdiste != null)
            panelPerdiste.SetActive(false);

        if (panelInstrucciones != null)
            panelInstrucciones.SetActive(false);

        if (panelPausa != null)
            panelPausa.SetActive(false);

        if (panelOpciones != null)
            panelOpciones.SetActive(false);

        if (sliderVolumen != null)
            sliderVolumen.value = AudioListener.volume;

        // Crear pausa y opciones si no están puestas en el Canvas
        PrepararMenuPausa();

        // Crear y actualizar el texto de la misión
        PrepararMision();
        ActualizarMision();

        // Reproducir la música de fondo de la partida
        if (musicaJuego != null)
        {
            audioMusica.clip = musicaJuego;
            audioMusica.Play();
        }
    }

    void Update()
    {
        // Abrir o cerrar pausa con Escape durante la partida
        if (!terminado && Input.GetKeyDown(KeyCode.Escape))
        {
            CambiarPausa();
        }
    }

    // Muestra el cartel de "GANASTE"
    public void MostrarGanaste()
    {
        // No se puede ganar sin recoger todas las monedas
        if (!TieneTodasLasMonedas())
        {
            Debug.Log("Faltan monedas para poder salir");
            return;
        }

        // Si ya se acabó la partida, no hacemos nada
        if (terminado) return;
        terminado = true;

        Debug.Log("¡GANASTE!");

        if (audioMusica != null)
            audioMusica.Stop();

        if (sonidoVictoria != null)
            audioEfectos.PlayOneShot(sonidoVictoria);

        if (panelGanaste != null)
            panelGanaste.SetActive(true);

        // Parar el jugador y los enemigos
        PararJuego();
    }

    // Muestra el cartel de "PERDISTE"
    public void MostrarPerdiste()
    {
        // Si ya se acabó la partida, no hacemos nada
        if (terminado) return;
        terminado = true;

        Debug.Log("¡PERDISTE!");

        if (audioMusica != null)
            audioMusica.Stop();

        if (sonidoDerrota != null)
            audioEfectos.PlayOneShot(sonidoDerrota);

        if (panelPerdiste != null)
            panelPerdiste.SetActive(true);

        // Parar el jugador y los enemigos
        PararJuego();
    }

    // Detiene la partida: libera el ratón, apaga el control del jugador
    // y desactiva a todos los enemigos, y por último congela el tiempo
    void PararJuego()
    {
        // Liberar el ratón para poder ver el cartel
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Desactivar el control del jugador para que no se pueda mover
        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null)
        {
            FirstPersonController control = jugador.GetComponent<FirstPersonController>();
            if (control != null)
                control.enabled = false;
        }

        // Desactivar a todos los enemigos para que dejen de moverse y atacar
        foreach (seguir s in FindObjectsByType<seguir>(FindObjectsSortMode.None))
            s.enabled = false;

        foreach (patrullero p in FindObjectsByType<patrullero>(FindObjectsSortMode.None))
            p.enabled = false;

        foreach (cercano c in FindObjectsByType<cercano>(FindObjectsSortMode.None))
            c.enabled = false;

        foreach (dispara d in FindObjectsByType<dispara>(FindObjectsSortMode.None))
            d.enabled = false;

        // Congelar el tiempo (para las físicas, las balas, etc.)
        Time.timeScale = 0f;
    }

    // Abrir o cerrar el panel de instrucciones
    public void CambiarInstrucciones()
    {
        if (instruccionesAbiertas)
        {
            OcultarInstrucciones();
        }
        else
        {
            MostrarInstrucciones();
        }
    }

    // Abrir o cerrar el panel de pausa
    public void CambiarPausa()
    {
        if (pausado)
        {
            OcultarPausa();
        }
        else
        {
            MostrarPausa();
        }
    }

    // Muestra el panel de pausa
    public void MostrarPausa()
    {
        Debug.Log("Mostrar pausa");

        if (panelPausa != null)
        {
            panelPausa.SetActive(true);
            panelPausa.transform.SetAsLastSibling();
        }

        pausado = true;
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Oculta el panel de pausa y vuelve al juego
    public void OcultarPausa()
    {
        Debug.Log("Ocultar pausa");

        if (panelPausa != null)
            panelPausa.SetActive(false);

        if (panelInstrucciones != null)
            panelInstrucciones.SetActive(false);

        if (panelOpciones != null)
            panelOpciones.SetActive(false);

        if (audioInstrucciones != null)
            audioInstrucciones.Stop();

        pausado = false;
        instruccionesAbiertas = false;
        opcionesAbiertas = false;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Muestra las instrucciones durante la partida
    public void MostrarInstrucciones()
    {
        Debug.Log("Mostrar instrucciones");

        if (panelInstrucciones != null)
        {
            panelInstrucciones.SetActive(true);
            panelInstrucciones.transform.SetAsLastSibling();
        }

        if (panelPausa != null)
            panelPausa.SetActive(false);

        if (sonidoInstrucciones != null)
        {
            audioInstrucciones.clip = sonidoInstrucciones;
            audioInstrucciones.Play();
        }

        instruccionesAbiertas = true;
        pausado = true;
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Oculta las instrucciones y vuelve al juego
    public void OcultarInstrucciones()
    {
        Debug.Log("Ocultar instrucciones");

        if (panelInstrucciones != null)
            panelInstrucciones.SetActive(false);

        if (panelPausa != null)
            panelPausa.SetActive(true);

        if (audioInstrucciones != null)
            audioInstrucciones.Stop();

        instruccionesAbiertas = false;
    }

    // Muestra las opciones durante la partida
    public void MostrarOpciones()
    {
        Debug.Log("Mostrar opciones");

        if (panelOpciones != null)
        {
            panelOpciones.SetActive(true);
            panelOpciones.transform.SetAsLastSibling();
        }

        if (panelPausa != null)
            panelPausa.SetActive(false);

        opcionesAbiertas = true;
        pausado = true;
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Oculta las opciones y vuelve al panel de pausa
    public void OcultarOpciones()
    {
        Debug.Log("Ocultar opciones");

        if (panelOpciones != null)
            panelOpciones.SetActive(false);

        if (panelPausa != null)
            panelPausa.SetActive(true);

        opcionesAbiertas = false;
    }

    // Se llama desde el slider de volumen
    public void CambiarVolumen(float volumen)
    {
        AudioListener.volume = volumen;
    }

    // Dice si las instrucciones están abiertas
    public bool InstruccionesAbiertas()
    {
        return pausado || instruccionesAbiertas || opcionesAbiertas;
    }

    // Suma una moneda recogida
    public void RecogerMoneda()
    {
        if (monedas < monedasObjetivo)
            monedas++;

        ActualizarMision();
    }

    // Comprueba si el jugador tiene todas las monedas
    public bool TieneTodasLasMonedas()
    {
        return monedas >= monedasObjetivo;
    }

    // Actualiza el texto de la misión
    void ActualizarMision()
    {
        if (textoMision != null)
            textoMision.text = "Misión: encuentra las " + monedasObjetivo + " monedas y sal\nMonedas: " + monedas + "/" + monedasObjetivo;
    }

    // Prepara el texto de misión si no está puesto en el Canvas
    void PrepararMision()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null)
            return;

        if (textoMision == null)
        {
            textoMision = CrearTexto(canvas.transform, "TextoMision", "", new Vector2(0, -45), new Vector2(780, 80), 24);

            RectTransform rect = textoMision.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
        }
    }

    // Prepara el menú de pausa y opciones si faltan
    void PrepararMenuPausa()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null)
            return;

        if (panelPausa == null)
        {
            panelPausa = CrearPanel(canvas.transform, "PanelPausa", new Vector2(650, 360));
            CrearTexto(panelPausa.transform, "TextoPausa", "PAUSA", new Vector2(0, 115), new Vector2(500, 70), 36);

            Button botonInstrucciones = CrearBoton(panelPausa.transform, "BotonPausaInstrucciones", "INSTRUCCIONES", new Vector2(0, 35), new Vector2(300, 55));
            botonInstrucciones.onClick.AddListener(MostrarInstrucciones);

            Button botonOpciones = CrearBoton(panelPausa.transform, "BotonPausaOpciones", "OPCIONES", new Vector2(0, -45), new Vector2(300, 55));
            botonOpciones.onClick.AddListener(MostrarOpciones);

            Button botonVolver = CrearBoton(panelPausa.transform, "BotonPausaVolver", "VOLVER", new Vector2(0, -125), new Vector2(220, 55));
            botonVolver.onClick.AddListener(OcultarPausa);

            panelPausa.SetActive(false);
        }

        if (panelOpciones == null)
        {
            panelOpciones = CrearPanel(canvas.transform, "PanelOpciones", new Vector2(650, 300));
            CrearTexto(panelOpciones.transform, "TextoOpciones", "OPCIONES\n\nVolumen", new Vector2(0, 70), new Vector2(580, 120), 30);
            sliderVolumen = CrearSlider(panelOpciones.transform, "SliderVolumen", new Vector2(0, 0), new Vector2(420, 35));

            Button botonVolver = CrearBoton(panelOpciones.transform, "BotonVolverOpciones", "VOLVER", new Vector2(0, -105), new Vector2(220, 55));
            botonVolver.onClick.AddListener(OcultarOpciones);

            panelOpciones.SetActive(false);
        }

        if (sliderVolumen != null)
        {
            sliderVolumen.minValue = 0f;
            sliderVolumen.maxValue = 1f;
            sliderVolumen.value = AudioListener.volume;
            sliderVolumen.onValueChanged.AddListener(CambiarVolumen);
        }
    }

    // Crea un panel sencillo
    GameObject CrearPanel(Transform padre, string nombre, Vector2 tamano)
    {
        GameObject panel = new GameObject(nombre, typeof(RectTransform));
        panel.transform.SetParent(padre, false);

        Image imagen = panel.AddComponent<Image>();
        imagen.color = new Color(0f, 0f, 0f, 0.85f);

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = tamano;

        return panel;
    }

    // Crea un botón sencillo
    Button CrearBoton(Transform padre, string nombre, string texto, Vector2 posicion, Vector2 tamano)
    {
        GameObject objetoBoton = new GameObject(nombre, typeof(RectTransform));
        objetoBoton.transform.SetParent(padre, false);

        Image imagen = objetoBoton.AddComponent<Image>();
        imagen.color = Color.red;

        Button boton = objetoBoton.AddComponent<Button>();
        boton.targetGraphic = imagen;

        RectTransform rect = objetoBoton.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = posicion;
        rect.sizeDelta = tamano;

        CrearTexto(objetoBoton.transform, "Text (TMP)", texto, Vector2.zero, tamano, 30);

        return boton;
    }

    // Crea texto para botones y paneles
    TextMeshProUGUI CrearTexto(Transform padre, string nombre, string texto, Vector2 posicion, Vector2 tamano, int tamanoLetra)
    {
        GameObject objetoTexto = new GameObject(nombre, typeof(RectTransform));
        objetoTexto.transform.SetParent(padre, false);

        TextMeshProUGUI textoUI = objetoTexto.AddComponent<TextMeshProUGUI>();
        textoUI.text = texto;
        textoUI.fontSize = tamanoLetra;
        textoUI.color = Color.white;
        textoUI.alignment = TextAlignmentOptions.Center;

        RectTransform rect = objetoTexto.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = posicion;
        rect.sizeDelta = tamano;

        return textoUI;
    }

    // Crea un slider de volumen sencillo
    Slider CrearSlider(Transform padre, string nombre, Vector2 posicion, Vector2 tamano)
    {
        GameObject objetoSlider = new GameObject(nombre, typeof(RectTransform));
        objetoSlider.transform.SetParent(padre, false);

        RectTransform rect = objetoSlider.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = posicion;
        rect.sizeDelta = tamano;

        Slider slider = objetoSlider.AddComponent<Slider>();

        GameObject fondo = new GameObject("Fondo", typeof(RectTransform));
        fondo.transform.SetParent(objetoSlider.transform, false);
        Image imagenFondo = fondo.AddComponent<Image>();
        imagenFondo.color = Color.gray;

        RectTransform rectFondo = fondo.GetComponent<RectTransform>();
        rectFondo.anchorMin = Vector2.zero;
        rectFondo.anchorMax = Vector2.one;
        rectFondo.offsetMin = Vector2.zero;
        rectFondo.offsetMax = Vector2.zero;

        GameObject relleno = new GameObject("Relleno", typeof(RectTransform));
        relleno.transform.SetParent(objetoSlider.transform, false);
        Image imagenRelleno = relleno.AddComponent<Image>();
        imagenRelleno.color = Color.red;

        RectTransform rectRelleno = relleno.GetComponent<RectTransform>();
        rectRelleno.anchorMin = Vector2.zero;
        rectRelleno.anchorMax = Vector2.one;
        rectRelleno.offsetMin = Vector2.zero;
        rectRelleno.offsetMax = Vector2.zero;

        GameObject boton = new GameObject("Handle", typeof(RectTransform));
        boton.transform.SetParent(objetoSlider.transform, false);
        Image imagenBoton = boton.AddComponent<Image>();
        imagenBoton.color = Color.white;

        RectTransform rectBoton = boton.GetComponent<RectTransform>();
        rectBoton.sizeDelta = new Vector2(25, 45);

        slider.fillRect = rectRelleno;
        slider.handleRect = rectBoton;
        slider.targetGraphic = imagenBoton;

        return slider;
    }
}
