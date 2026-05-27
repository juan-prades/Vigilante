using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;   // Singleton, igual que el SoundManager

    [Header("Paneles")]
    public GameObject panelGanaste;     // Arrastra aquí el panel de "GANASTE"
    public GameObject panelPerdiste;    // Arrastra aquí el panel de "PERDISTE"

    private bool terminado = false;     // Para que solo se muestre un cartel

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
    }

    void Start()
    {
        // Los dos paneles empiezan ocultos
        if (panelGanaste != null)
            panelGanaste.SetActive(false);

        if (panelPerdiste != null)
            panelPerdiste.SetActive(false);
    }

    // Muestra el cartel de "GANASTE"
    public void MostrarGanaste()
    {
        // Si ya se acabó la partida, no hacemos nada
        if (terminado) return;
        terminado = true;

        Debug.Log("¡GANASTE!");

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
}
