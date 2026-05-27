using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic; // Banda sonora
    public AudioClip hitSound; // Sonido al presionar "E"
    public AudioClip jumpSound; // Sonido al presionar "Espacio"

    [Header("Audio Sources")]
    private AudioSource musicSource;
    private AudioSource effectsSource;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float musicVolume = 0.5f; // Volumen de la música
    [Range(0f, 1f)] public float effectsVolume = 1.0f; // Volumen de los efectos

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        effectsSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;
    }

    void Start()
    {
        PlayBackgroundMusic();
    }

    void Update()
    {
        musicSource.volume = musicVolume;
        effectsSource.volume = effectsVolume;
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
    }

    public void PlayEffectSound(AudioClip clip)
    {
        if (clip != null)
        {
            effectsSource.PlayOneShot(clip);
        }
    }

    public void PlayZoneSound(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            effectsSource.PlayOneShot(clip, volume);
        }
    }
}
