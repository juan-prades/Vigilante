using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public string escenaJuego = "SampleScene"; // Nombre de la escena del juego (cámbialo si la renombras)

    void Start()
    {
        // En el menú queremos el ratón visible para poder pulsar los botones
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Por si veníamos de una partida congelada (Time.timeScale = 0)
        Time.timeScale = 1f;
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
}
