using UnityEngine;

public class MetaJuego : MonoBehaviour
{
    // Este script va en el cubo final (panel de control).
    // Cuando el jugador entra en su trigger, se gana la partida.

    void OnTriggerEnter(Collider other)
    {
        // Comprobar que quien entra es el jugador
        if (other.CompareTag("Player"))
        {
            Debug.Log("El jugador llegó a la meta");

            // Solo se puede salir si tiene todas las monedas
            if (UIManager.Instance != null && !UIManager.Instance.TieneTodasLasMonedas())
            {
                Debug.Log("Todavía faltan monedas");
                return;
            }

            // Avisar al UIManager para que muestre el cartel de victoria
            UIManager.Instance.MostrarGanaste();
        }
    }
}
