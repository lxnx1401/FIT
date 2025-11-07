using UnityEngine;

public class GiftPickup : MonoBehaviour
{
    public enum PowerUpType { DamageUp, FireRateUp, SlowEnemies, Shield }

    public PowerUpType powerUpType;
    public float duration = 5f;

    [Header("Floating & Rotation")]
    public float floatAmplitude = 0.25f; // yukarı-aşağı mesafe
    public float floatFrequency = 1f;    // ne kadar hızlı süzülecek
    public float rotationSpeed = 50f;    // kendi etrafında dönme hızı

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        // Yukarı-aşağı süzme
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Kendi etrafında dönme
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PowerUpManager manager = other.GetComponent<PowerUpManager>();
            if (manager != null)
                manager.ActivatePowerUp(powerUpType, duration);

            Destroy(gameObject);
        }
    }
}
