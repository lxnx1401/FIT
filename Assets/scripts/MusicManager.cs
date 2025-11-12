using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [Header("Musik-Einstellungen")]
    [Tooltip("Die Musikquelle (AudioSource), die abgespielt werden soll.")]
    public AudioSource musicSource;

    [Tooltip("Szenen, in denen Musik abgespielt werden soll.")]
    public string[] allowedScenes = { "MainMenu", "Settings", "Credits" };

    private static MusicManager instance;

    void Awake()
    {
        // Nur eine Instanz behalten (Singleton-Prinzip)
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Auf Szenenwechsel reagieren
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool shouldPlay = false;

        // Prüfen, ob aktuelle Szene in der erlaubten Liste ist
        foreach (string sceneName in allowedScenes)
        {
            if (scene.name == sceneName)
            {
                shouldPlay = true;
                break;
            }
        }

        if (shouldPlay)
        {
            if (!musicSource.isPlaying)
                musicSource.Play();
        }
        else
        {
            if (musicSource.isPlaying)
                musicSource.Stop();
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
