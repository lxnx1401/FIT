using UnityEngine;

public class GiftPickup : MonoBehaviour
{
    public enum PowerUpType { DamageUp, FireRateUp, SlowEnemies, Shield }

    public PowerUpType powerUpType;
    public float duration = 5f;

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
