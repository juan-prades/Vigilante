using UnityEngine;

public class LuzZonalTitileo : MonoBehaviour
{
    public Light luzZonal;          // Asigna aquí tu Point Light
    public bool encendida = false;  // Estado inicial
    public float intensidadBase = 2f;
    public float variacion = 0.5f;  // Cuánto fluctúa la intensidad
    public float velocidad = 10f;   // Velocidad del parpadeo

    void Start()
    {
        if (luzZonal != null)
            luzZonal.enabled = encendida;
    }

    void Update()
    {
        // Alternar luz con tecla 7
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            encendida = !encendida;
            luzZonal.enabled = encendida;
        }

        // Titileo (solo si está encendida)
        if (encendida)
        {
            float ruido = Mathf.PerlinNoise(Time.time * velocidad, 0f);
            luzZonal.intensity = intensidadBase + (ruido - 0.5f) * variacion;
        }
    }
}
