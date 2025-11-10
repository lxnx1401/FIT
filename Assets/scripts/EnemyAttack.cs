using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public static bool IgnorePlayerDamage = false;
    public int health = 2;

    private Renderer rend;
    private Color originalColor;
    public float flashDuration = 0.1f;

    [Header("Death Effects")]
    public GameObject snowDeathEffect;  // Schneepartikel
    public GameObject eyePrefab;        // Kohleauge
    public GameObject carrotPrefab;     // Karotte
    public float partForce = 3f;        // Wurfkraft

    [Header("Audio Settings")]
    public AudioSource audioSource;     // Soundquelle
    public AudioClip[] deathSounds;     // Verschiedene Todessounds
    public bool randomizePitch = true;  // Option für zufällige Tonhöhe

    private void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!IgnorePlayerDamage)
                Destroy(other.gameObject);
        }
        else if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);

            health--;
            StartCoroutine(DamageFlash());

            if (health <= 0)
            {
                Die();
            }
        }
    }

    // ------------------------------
    // TOD-FUNKTION
    // ------------------------------
    private void Die()
    {
        // 🔊 Sound abspielen
        PlayDeathSound();

        // ❄️ Schneepartikel
        if (snowDeathEffect != null)
            Instantiate(snowDeathEffect, transform.position + Vector3.up * 1f, Quaternion.identity);

        // 👀 Augen
        if (eyePrefab != null)
        {
            SpawnPart(eyePrefab);
            SpawnPart(eyePrefab);
        }

        // 🥕 Karotte
        if (carrotPrefab != null)
        {
            SpawnPart(carrotPrefab);
        }

        Destroy(gameObject);
    }

    // 🔉 Sound abspielen
    private void PlayDeathSound()
    {
        if (audioSource == null || deathSounds.Length == 0) return;

        AudioClip clip = deathSounds[Random.Range(0, deathSounds.Length)];

        if (randomizePitch)
            audioSource.pitch = Random.Range(0.95f, 1.05f);
        else
            audioSource.pitch = 1f;

        audioSource.PlayOneShot(clip);
    }

    // Göz & burun parçalarını fırlatma
    private void SpawnPart(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, transform.position + Vector3.up * 1f, Random.rotation);

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 randomDir = Random.onUnitSphere;
            randomDir.y = Mathf.Abs(randomDir.y); // eher nach oben
            rb.AddForce(randomDir * partForce, ForceMode.Impulse);
        }

        Destroy(obj, 1.2f);
    }

    // 🔴 Kurz rot aufblinken beim Treffer
    private IEnumerator DamageFlash()
    {
        if (rend != null)
        {
            rend.material.color = Color.red;
            yield return new WaitForSeconds(flashDuration);
            rend.material.color = originalColor;
        }
    }
}
