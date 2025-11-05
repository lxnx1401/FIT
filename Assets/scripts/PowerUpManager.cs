using System.Collections;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private PlayerShooting shooting;
    private Renderer playerRenderer;
    private Color originalColor;
    private bool isShielded = false;

    [Header("Shield Visual (Prefab)")]
    public GameObject shieldVisualPrefab; // Mor şeffaf küre prefab'ı
    private GameObject activeShield;       // Aktif kalkan nesnesi

    private void Start()
    {
        shooting = GetComponent<PlayerShooting>();
        playerRenderer = GetComponentInChildren<Renderer>();
        if (playerRenderer != null)
            originalColor = playerRenderer.material.color;
    }

    public void ActivatePowerUp(GiftPickup.PowerUpType type, float duration)
    {
        switch (type)
        {
            case GiftPickup.PowerUpType.DamageUp:
                StartCoroutine(DamageBoost(duration));
                break;
            case GiftPickup.PowerUpType.FireRateUp:
                StartCoroutine(FireRateBoost(duration));
                break;
            case GiftPickup.PowerUpType.SlowEnemies:
                StartCoroutine(SlowEnemies(duration));
                break;
            case GiftPickup.PowerUpType.Shield:
                StartCoroutine(ActivateShield(duration));
                break;
        }
    }

    // 💥 Hasar artışı
    private IEnumerator DamageBoost(float duration)
    {
        shooting.bulletDamage *= 2f;
        shooting.isDamageBoosted = true;
        yield return new WaitForSeconds(duration);
        shooting.bulletDamage /= 2f;
        shooting.isDamageBoosted = false;
    }

    // ⚡ Ateş hızı artışı
    private IEnumerator FireRateBoost(float duration)
    {
        shooting.TimeBetweenShots /= 3f;
        yield return new WaitForSeconds(duration);
        shooting.TimeBetweenShots *= 3f;
    }

    // 🧊 Düşmanları yavaşlatma
     private IEnumerator SlowEnemies(float duration)
    {
        EnemyMovement[] enemies = FindObjectsOfType<EnemyMovement>();

        foreach (var e in enemies)
        {
            e.EnemySpeed *= 0.3f;

            // 🔵 Renk yanıp sönme efekti başlat
            var rend = e.GetComponentInChildren<Renderer>();
            if (rend != null)
                StartCoroutine(EnemyFlashEffect(rend, duration));
        }

        yield return new WaitForSeconds(duration);

        foreach (var e in enemies)
            e.EnemySpeed /= 0.3f;
    }

    // 💡 Düşman rengini açık mavi yapıp hızlı hızlı yanıp sönmesini sağlar
    private IEnumerator EnemyFlashEffect(Renderer rend, float duration)
    {
        Color original = rend.material.color;
        Color flashColor = new Color(0f, 0.7f, 1f); // açık mavi
        float elapsed = 0f;
        float flashSpeed = 0.15f; // yanıp sönme hızı

        while (elapsed < duration)
        {
            rend.material.color = flashColor;
            yield return new WaitForSeconds(flashSpeed);
            rend.material.color = original;
            yield return new WaitForSeconds(flashSpeed);
            elapsed += flashSpeed * 2f;
        }

        // Etki bitince rengi geri yükle
        rend.material.color = original;
    }

    // 🛡️ Kalkan
  private IEnumerator ActivateShield(float duration)
{
    if (isShielded) yield break;
    isShielded = true;

    // 🟣 Mor aura prefab'ını oluştur
    if (shieldVisualPrefab != null)
    {
        activeShield = Instantiate(shieldVisualPrefab, transform.position, Quaternion.identity);
        activeShield.transform.SetParent(transform);
        activeShield.transform.localPosition = new Vector3(0f, 0.8f, 0f);

        // Shield script'ini al
        Shield shield = activeShield.GetComponent<Shield>();

        // 🕐 Bir frame bekle ki Shield.Start() çalışsın (_renderer null olmasın)
        yield return null;

        if (shield != null)
        {
            // 🔹 Açılma animasyonu
            shield.OpenCloseShield();
        }
    }

    // 🛡️ Hasar engelle
    EnemyAttack.IgnorePlayerDamage = true;

    // ⏳ Süre boyunca açık kalsın
    yield return new WaitForSeconds(duration);

    // ❌ Hasarı tekrar aktif et
    EnemyAttack.IgnorePlayerDamage = false;

    // 🔻 Kapanma animasyonu
    if (activeShield != null)
    {
        Shield shield = activeShield.GetComponent<Shield>();
        if (shield != null)
        {
            shield.OpenCloseShield(); // kapanma animasyonu
            yield return new WaitForSeconds(1f); // animasyonun bitmesi için biraz bekle
        }

        Destroy(activeShield);
    }

    isShielded = false;
}



}
