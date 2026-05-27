using UnityEngine;
using UnityEngine.UI;

public class BateriaLinterna : MonoBehaviour
{
    public Light linterna;       // Arrastra aquí la misma Spotlight que usa la Linterna
    public Image barra;          // Arrastra aquí la barra de UI (Image tipo Filled)
    public float bateria = 100f; // Batería actual (empieza llena)
    public float gasto = 10f;    // Cuánta batería se gasta por segundo encendida

    void Update()
    {
        // Solo gasta batería mientras la linterna está encendida
        if (linterna.enabled)
        {
            bateria -= gasto * Time.deltaTime;

            // Si se queda sin batería, se apaga sola
            if (bateria <= 0)
            {
                bateria = 0;
                linterna.enabled = false;
                Debug.Log("Linterna sin batería");
            }
        }

        // Actualizamos la barra de la interfaz (de 0 a 1)
        if (barra != null)
            barra.fillAmount = bateria / 100f;
    }
}
