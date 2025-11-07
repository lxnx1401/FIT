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
    public GameObject snowDeathEffect;  // Kar patlaması particle
    public GameObject eyePrefab;        // Kömür göz
    public GameObject carrotPrefab;     // Havuç
    public float partForce = 3f;        // Fırlama kuvveti

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
            {
                Die();
            }
        }
    }

    // ------------------------------
    // ÖLÜM FONKSİYONU
    // ------------------------------
    private void Die()
    {
        // ❄️ Kar patlama efekti
        if (snowDeathEffect != null)
            Instantiate(snowDeathEffect, transform.position + Vector3.up * 1f, Quaternion.identity);

        // 👀 İki göz fırlatma
        if (eyePrefab != null)
        {
            SpawnPart(eyePrefab);
            SpawnPart(eyePrefab);
        }

        // 🥕 Havuç burun fırlatma
        if (carrotPrefab != null)
        {
            SpawnPart(carrotPrefab);
        }

        Destroy(gameObject);
    }

    // Göz & burun parçalarını fırlatma
    private void SpawnPart(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, transform.position + Vector3.up * 1f, Random.rotation);

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 randomDir = Random.onUnitSphere;
            randomDir.y = Mathf.Abs(randomDir.y); // yukarı ağırlıklı

            rb.AddForce(randomDir * partForce, ForceMode.Impulse);
        }

        Destroy(obj, 1.2f);
    }

    // 🔴 Hasar alırken kısa kırmızı flash
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
