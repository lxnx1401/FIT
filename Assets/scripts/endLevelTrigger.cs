using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelTrigger : MonoBehaviour
{
    public string nextSceneName = "NextSceneName";
    private float timer;
    private bool timerRunning = true;

    void Start()
    {
        Time.timeScale = 1f; // sicherstellen, dass das Level normal läuft
        timer = 0f;
    }

    void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timerRunning = false;

            // Bestzeit speichern
            PlayerPrefs.SetFloat("BestTime", timer);

            // Spiel einfrieren
            Time.timeScale = 0f;

            // Szene laden (mit kleiner Verzögerung, sonst hängt Unity)
            StartCoroutine(LoadScene());
        }
    }

    private System.Collections.IEnumerator LoadScene()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(nextSceneName);
    }
}
