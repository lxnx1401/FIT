using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUIManager : MonoBehaviour
{
    [Header("Assign Icons (each should be a separate UI Image)")]
    public Image damageIcon;
    public Image fireRateIcon;
    public Image slowIcon;
    public Image shieldIcon;

    [Header("Pulse Settings")]
    public float pulseSpeed = 6f;
    public float pulseAmount = 0.08f;
    public float maxScale = 1.15f;

    // Her ikon için ayrı coroutine tutuyoruz ki çakışma olmasın
    private Coroutine damageRoutine;
    private Coroutine fireRateRoutine;
    private Coroutine slowRoutine;
    private Coroutine shieldRoutine;

    private void Awake()
    {
        if (damageIcon) damageIcon.gameObject.SetActive(false);
        if (fireRateIcon) fireRateIcon.gameObject.SetActive(false);
        if (slowIcon) slowIcon.gameObject.SetActive(false);
        if (shieldIcon) shieldIcon.gameObject.SetActive(false);
    }

    // Dışarıdan çağırılan public fonksiyon
    public void ShowIcon(Image icon, float duration)
    {
        if (icon == null) return;

        // Hangi ikon olduğunu buluyoruz
        if (icon == damageIcon)
        {
            if (damageRoutine != null) StopCoroutine(damageRoutine);
            damageRoutine = StartCoroutine(ShowIconRoutine(icon, duration));
        }
        else if (icon == fireRateIcon)
        {
            if (fireRateRoutine != null) StopCoroutine(fireRateRoutine);
            fireRateRoutine = StartCoroutine(ShowIconRoutine(icon, duration));
        }
        else if (icon == slowIcon)
        {
            if (slowRoutine != null) StopCoroutine(slowRoutine);
            slowRoutine = StartCoroutine(ShowIconRoutine(icon, duration));
        }
        else if (icon == shieldIcon)
        {
            if (shieldRoutine != null) StopCoroutine(shieldRoutine);
            shieldRoutine = StartCoroutine(ShowIconRoutine(icon, duration));
        }
    }

    private IEnumerator ShowIconRoutine(Image icon, float duration)
    {
        icon.gameObject.SetActive(true);
        icon.fillAmount = 1f;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Radial dolma
            icon.fillAmount = 1f - (elapsed / duration);

            // Pulse animasyonu
            float scale = maxScale + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            icon.transform.localScale = Vector3.one * scale;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // İkonu gizle
        icon.fillAmount = 1f;
        icon.transform.localScale = Vector3.one;
        icon.gameObject.SetActive(false);
    }
}
