using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuPrincipal : MonoBehaviour
{
    public string escenaJuego = "SampleScene"; // Nombre de la escena del juego (cámbialo si la renombras)

    [Header("Sonido")]
    public AudioClip sonidoMenu; // Sonido del menú y las instrucciones

    [Header("Instrucciones")]
    public GameObject panelInstrucciones; // Panel que muestra las instrucciones

    [Header("Opciones")]
    public GameObject panelOpciones; // Panel que muestra las opciones
    public Slider sliderVolumen;     // Slider para subir y bajar el volumen

    private AudioSource audioMenu; // Fuente de sonido del menú

    void Start()
    {
        // En el menú queremos el ratón visible para poder pulsar los botones
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Por si veníamos de una partida congelada (Time.timeScale = 0)
        Time.timeScale = 1f;

        // El panel de instrucciones empieza oculto
        if (panelInstrucciones != null)
            panelInstrucciones.SetActive(false);

        // El panel de opciones empieza oculto
        if (panelOpciones != null)
            panelOpciones.SetActive(false);

        // Preparar el slider con el volumen actual
        if (sliderVolumen != null)
            sliderVolumen.value = AudioListener.volume;

        // Crear opciones si no están puestas en el Canvas
        PrepararOpciones();

        // Preparar el sonido del menú
        audioMenu = gameObject.AddComponent<AudioSource>();
        audioMenu.clip = sonidoMenu;
        audioMenu.loop = true;
        audioMenu.volume = 0.7f;

        if (sonidoMenu != null)
            audioMenu.Play();
    }

    // Se llama desde el botón JUGAR
    public void Jugar()
    {
        Debug.Log("Empezar partida");
        SceneManager.LoadScene(escenaJuego);
    }

    // Se llama desde el botón SALIR
    public void Salir()
    {
        Debug.Log("Salir del juego");
        Application.Quit();
    }

    // Se llama desde el botón INSTRUCCIONES
    public void MostrarInstrucciones()
    {
        Debug.Log("Mostrar instrucciones");

        if (panelInstrucciones != null)
            panelInstrucciones.SetActive(true);
    }

    // Se llama desde el botón VOLVER
    public void OcultarInstrucciones()
    {
        Debug.Log("Ocultar instrucciones");

        if (panelInstrucciones != null)
            panelInstrucciones.SetActive(false);
    }

    // Se llama desde el botón OPCIONES
    public void MostrarOpciones()
    {
        Debug.Log("Mostrar opciones");

        if (panelOpciones != null)
            panelOpciones.SetActive(true);
    }

    // Se llama desde el botón VOLVER de opciones
    public void OcultarOpciones()
    {
        Debug.Log("Ocultar opciones");

        if (panelOpciones != null)
            panelOpciones.SetActive(false);
    }

    // Se llama desde el slider de volumen
    public void CambiarVolumen(float volumen)
    {
        AudioListener.volume = volumen;
    }

    // Prepara el botón y el panel de opciones si faltan
    void PrepararOpciones()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null)
            return;

        if (GameObject.Find("BotonOpciones") == null)
        {
            Button botonOpciones = CrearBoton(canvas.transform, "BotonOpciones", "OPCIONES", new Vector2(0, 215), new Vector2(260, 55));
            botonOpciones.onClick.AddListener(MostrarOpciones);
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
        rectRelleno.anchorMax = new Vector2(1, 1);
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
