using UnityEngine;

public class Linterna : MonoBehaviour
{
    public Light linterna;       // Arrastra aquí tu Spotlight
    public float intensidad = 2; // Intensidad por defecto cuando se enciende
    public Color color = Color.yellow; // Color fijo de la linterna (cámbialo en el inspector)
    public AudioClip sonidoLinterna; // Sonido al encender o apagar la linterna

    AudioSource audioLinterna; // Fuente de sonido de la linterna

    void Start()
    {
        audioLinterna = gameObject.AddComponent<AudioSource>();

        if (linterna != null)
            linterna.enabled = false; // Empieza apagada
    }

    void Update()
    {
        // Encender (tecla 4)
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (!linterna.enabled)
            {
                // Color fijo elegido en el inspector
                linterna.color = color;

                linterna.intensity = intensidad;
                linterna.enabled = true;

                if (sonidoLinterna != null)
                    audioLinterna.PlayOneShot(sonidoLinterna);
            }
        }

        // Apagar (tecla 5)
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (linterna.enabled)
            {
                linterna.enabled = false;

                if (sonidoLinterna != null)
                    audioLinterna.PlayOneShot(sonidoLinterna);
            }
        }
    }
}
