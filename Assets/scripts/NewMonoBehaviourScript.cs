using UnityEngine;
using UnityEngine.SceneManagement; // Für Szenenwechsel

public class PlayerDeathSound : MonoBehaviour
{
    [Header("Player Reference")]
    public GameObject player; // Ziehe hier dein Player-Objekt rein

    [Header("Audio Settings")]
    public AudioSource audioSource; // AudioSource auf der Kamera
    public AudioClip deathClip;     // Sound, der beim Tod abgespielt wird

    [Header("Scene Settings")]
    public string menuSceneName = "MainMenu"; // Name der Szene, zu der gewechselt werden soll

    private bool hasPlayed = false; // Damit Sound nur einmal abgespielt wird

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Wenn der Player nicht mehr existiert UND der Sound noch nicht abgespielt wurde
        if (!hasPlayed && (player == null))
        {
            hasPlayed = true;

            // Sound abspielen
            if (audioSource != null && deathClip != null)
            {
                audioSource.PlayOneShot(deathClip);
            }

            // Szene nach Soundwechsel laden
            // Wir warten kurz, damit der Sound hörbar ist
            float delay = (deathClip != null) ? deathClip.length : 0f;
            Invoke(nameof(LoadMenuScene), delay);
        }
    }

    void LoadMenuScene()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
