using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public static bool IgnorePlayerDamage = false; // Kalkan aktifse oyuncuya hasar yok
    public int health = 2;
    private Renderer rend;
    private Color originalColor;
    public float flashDuration = 0.1f;

    private void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;
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
                Destroy(gameObject);
        }
    }

    // 🔴 Hasar alınca kırmızı yanıp sönme
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
