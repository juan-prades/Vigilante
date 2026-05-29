using UnityEngine;

public class Moneda : MonoBehaviour
{
    private bool recogida = false; // Para no contar dos veces la misma moneda

    void OnTriggerEnter(Collider other)
    {
        // Comprobar que quien toca la moneda es el jugador
        if (other.CompareTag("Player") && !recogida)
        {
            recogida = true;

            Debug.Log("Moneda recogida");

            if (UIManager.Instance != null)
                UIManager.Instance.RecogerMoneda();

            gameObject.SetActive(false);
        }
    }
}
