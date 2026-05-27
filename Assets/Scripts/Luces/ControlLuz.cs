using UnityEngine;

public class ControlLuz : MonoBehaviour
{
    public Light luz;              // Arrastra aquí la luz desde el inspector
    public float paso = 0.2f;      // Cantidad que sube o baja la intensidad

    void Update()
    {
        // Aumentar intensidad (tecla 1)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            luz.intensity += paso;
        }

        // Disminuir intensidad (tecla 2)
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            luz.intensity -= paso;
            if (luz.intensity < 0) luz.intensity = 0;
        }

        // Apagar completamente (tecla 3)
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            luz.intensity = 0;
        }
    }
}
