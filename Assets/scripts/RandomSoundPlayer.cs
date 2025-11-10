using UnityEngine;
using System.Collections;

public class RandomSoundPlayer : MonoBehaviour
{
    [Header("Audio Einstellungen")]
    public AudioSource audioSource;          // Referenz zur AudioSource
    public AudioClip[] soundClips;           // Liste der möglichen Sounds

    [Header("Zeitintervall (Sekunden)")]
    public float minDelay = 10f;             // Minimales Intervall
    public float maxDelay = 20f;             // Maximales Intervall

    private void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (soundClips.Length > 0)
            StartCoroutine(PlaySounds());
        else
            Debug.LogWarning("⚠️ Keine AudioClips zugewiesen!");
    }

    private IEnumerator PlaySounds()
    {
        while (true)
        {
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);

            AudioClip clip = soundClips[Random.Range(0, soundClips.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
}
