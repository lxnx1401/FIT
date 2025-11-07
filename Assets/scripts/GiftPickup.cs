using UnityEngine;

public class GiftPickup : MonoBehaviour
{
    public enum PowerUpType { DamageUp, FireRateUp, SlowEnemies, Shield }

    [Header("Random PowerUp")]
    public bool randomizePowerUp = true;   // ✅ Inspector'dan aç/kapa

    public PowerUpType powerUpType;        // ✅ Eğer random açık değilse bunu kullanır
    public float duration = 5f;

    [Header("Floating & Rotation")]
    public float floatAmplitude = 0.25f;
    public float floatFrequency = 1f;
    public float rotationSpeed = 50f;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;

        // ✅ Rastgele güç seçme
        if (randomizePowerUp)
        {
            int count = System.Enum.GetValues(typeof(PowerUpType)).Length;
            powerUpType = (PowerUpType)Random.Range(0, count);
        }
    }

    private void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

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
