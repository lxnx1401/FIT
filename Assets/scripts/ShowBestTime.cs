using UnityEngine;
using TMPro;

public class ShowBestTimeTMP : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    void Start()
    {
        float best = PlayerPrefs.GetFloat("BestTime", 0f);

        // Minuten und Sekunden errechnen
        int minutes = Mathf.FloorToInt(best / 60f);
        int seconds = Mathf.FloorToInt(best % 60f);

        timeText.text = $"Final Time: {minutes:00}:{seconds:00}min";
    }
}
