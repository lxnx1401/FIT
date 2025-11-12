using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalVolumeController : MonoBehaviour
{
    [Header("UI Settings")]
    [Tooltip("Der UI-Slider, der die Lautstärke steuert.")]
    public Slider volumeSlider;

    // Speichert die Lautstärke zwischen Szenen
    private static float currentVolume = 1f;

    // Singleton-ähnlicher Schutz
    private static GlobalVolumeController instance;

    void Awake()
    {
        // Nur eine Instanz behalten (persistenter Audio-Controller)
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

        // Slider initialisieren
        SetupSlider();
        ApplyVolume(currentVolume);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Wenn in der neuen Szene wieder ein Slider existiert, automatisch verknüpfen
        if (volumeSlider == null)
        {
            volumeSlider = FindObjectOfType<Slider>();
            SetupSlider();
        }
        else
        {
            SetupSlider();
        }
    }

    private void SetupSlider()
    {
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 3f;
            volumeSlider.value = currentVolume;

            volumeSlider.onValueChanged.RemoveAllListeners();
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void SetVolume(float value)
    {
        currentVolume = value;
        ApplyVolume(value);
    }

    private void ApplyVolume(float value)
    {
        // Begrenzung auf 0–3 (über 1 wird’s doppelt oder dreifach laut)
        AudioListener.volume = Mathf.Clamp(value, 0f, 3f);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
