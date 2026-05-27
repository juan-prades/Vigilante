using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public AudioClip zoneSound; // Sonido específico para esta zona

    [Range(0f, 1f)] public float zoneSoundVolume = 0.8f; // Volumen para este sonido

    private AudioSource zoneAudioSource; // Fuente de sonido para esta zona

    private void Start()
    {
        // Crear un AudioSource específico para la zona
        zoneAudioSource = gameObject.AddComponent<AudioSource>();
        zoneAudioSource.clip = zoneSound;
        zoneAudioSource.volume = zoneSoundVolume;
        zoneAudioSource.loop = true; // Para que el sonido se repita mientras esté en la zona
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de etiquetar al jugador como "Player"
        {
            if (zoneAudioSource != null && !zoneAudioSource.isPlaying)
            {
                zoneAudioSource.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (zoneAudioSource != null && zoneAudioSource.isPlaying)
            {
                zoneAudioSource.Stop();
            }
        }
    }
}
