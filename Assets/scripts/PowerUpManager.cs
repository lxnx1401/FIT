using System.Collections;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private PlayerShooting shooting;
    private Renderer playerRenderer;
    private Color originalColor;
    private bool isShielded = false;

    [Header("Shield Visual (Prefab)")]
    public GameObject shieldVisualPrefab;
    private GameObject activeShield;

    [Header("UI Manager")]
    public PowerUpUIManager uiManager;

    [Header("Audio Settings")]
    public AudioSource audioSource;              // AudioSource zum Abspielen
    public AudioClip[] powerUpSounds;            // Verschiedene Sounds für Power-Ups
    public bool randomizePitch = true;           // Optional: leichte Variation

    private void Start()
    {
        shooting = GetComponent<PlayerShooting>();
        playerRenderer = GetComponentInChildren<Renderer>();
        if (playerRenderer != null)
            originalColor = playerRenderer.material.color;

        if (uiManager == null)
            uiManager = FindObjectOfType<PowerUpUIManager>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void ActivatePowerUp(GiftPickup.PowerUpType type, float duration)
    {
        // 🔸 SOUND abspielen
        PlayPowerUpSound();

        switch (type)
        {
            case GiftPickup.PowerUpType.DamageUp:
                StartCoroutine(DamageBoost(duration));
                if (uiManager) uiManager.ShowIcon(uiManager.damageIcon, duration);
                break;

            case GiftPickup.PowerUpType.FireRateUp:
                StartCoroutine(FireRateBoost(duration));
                if (uiManager) uiManager.ShowIcon(uiManager.fireRateIcon, duration);
                break;

            case GiftPickup.PowerUpType.SlowEnemies:
                StartCoroutine(SlowEnemies(duration));
                if (uiManager) uiManager.ShowIcon(uiManager.slowIcon, duration);
                break;

            case GiftPickup.PowerUpType.Shield:
                StartCoroutine(ActivateShield(duration));
                if (uiManager) uiManager.ShowIcon(uiManager.shieldIcon, duration);
                break;
        }
    }

    // 🔉 Sound abspielen, wenn PowerUp aktiviert wird
    private void PlayPowerUpSound()
    {
        if (audioSource == null || powerUpSounds.Length == 0) return;

        AudioClip clip = powerUpSounds[Random.Range(0, powerUpSounds.Length)];

        if (randomizePitch)
            audioSource.pitch = Random.Range(0.95f, 1.05f);
        else
            audioSource.pitch = 1f;

        audioSource.PlayOneShot(clip);
    }

    private IEnumerator DamageBoost(float duration)
    {
        shooting.bulletDamage *= 2f;
        shooting.isDamageBoosted = true;
        yield return new WaitForSeconds(duration);
        shooting.bulletDamage /= 2f;
        shooting.isDamageBoosted = false;
    }

    private IEnumerator FireRateBoost(float duration)
    {
        shooting.TimeBetweenShots /= 3f;
        yield return new WaitForSeconds(duration);
        shooting.TimeBetweenShots *= 3f;
    }

    private IEnumerator SlowEnemies(float duration)
    {
        EnemyMovement[] enemies = FindObjectsOfType<EnemyMovement>();

        foreach (var e in enemies)
        {
            e.EnemySpeed *= 0.3f;
            var rend = e.GetComponentInChildren<Renderer>();
            if (rend != null)
                StartCoroutine(EnemyFlashEffect(rend, duration));
        }

        yield return new WaitForSeconds(duration);

        foreach (var e in enemies)
            e.EnemySpeed /= 0.3f;
    }

    private IEnumerator EnemyFlashEffect(Renderer rend, float duration)
    {
        Color original = rend.material.color;
        Color flashColor = new Color(0f, 0.7f, 1f);
        float elapsed = 0f;
        float flashSpeed = 0.15f;

        while (elapsed < duration)
        {
            rend.material.color = flashColor;
            yield return new WaitForSeconds(flashSpeed);
            rend.material.color = original;
            yield return new WaitForSeconds(flashSpeed);
            elapsed += flashSpeed * 2f;
        }

        rend.material.color = original;
    }

    private IEnumerator ActivateShield(float duration)
    {
        if (isShielded) yield break;
        isShielded = true;

        if (shieldVisualPrefab != null)
        {
            activeShield = Instantiate(shieldVisualPrefab, transform.position, Quaternion.identity);
            activeShield.transform.SetParent(transform);
            activeShield.transform.localPosition = new Vector3(0f, 0.8f, 0f);

            Shield shield = activeShield.GetComponent<Shield>();
            yield return null;
            if (shield != null)
                shield.OpenCloseShield();
        }

        EnemyAttack.IgnorePlayerDamage = true;
        yield return new WaitForSeconds(duration);
        EnemyAttack.IgnorePlayerDamage = false;

        if (activeShield != null)
        {
            Shield shield = activeShield.GetComponent<Shield>();
            if (shield != null)
                shield.OpenCloseShield();
            yield return new WaitForSeconds(1f);
            Destroy(activeShield);
        }

        isShielded = false;
    }
}
