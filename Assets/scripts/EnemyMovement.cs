using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float EnemySpeed = 3f;         // Hareket hızı
    public float RotationSpeed = 5f;      // Dönüş hızı

    [Header("Attack Settings")]
    public float attackRange = 15f;        // Oyuncuya yaklaşınca saldırmaya başlama mesafesi

    private GameObject m_player;

    private void Awake()
    {
        // Player'ı tag üzerinden bul
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (m_player == null)
            return;

        // Oyuncuya olan mesafeyi hesapla
        float distanceToPlayer = Vector3.Distance(transform.position, m_player.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            MoveTowardsPlayer();
        }
        // Eğer menzil dışındaysa hareket etmeyecek
    }

    private void MoveTowardsPlayer()
    {
        // Oyuncuya doğru yön vektörü
        Vector3 direction = m_player.transform.position - transform.position;
        direction.y = 0f; // Y ekseninde dönmeyi engelle

        if (direction != Vector3.zero)
        {
            // Dönüş
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
        }

        // Hareket
        transform.Translate(Vector3.forward * EnemySpeed * Time.deltaTime);
    }

    // Debug için sahnede saldırı menzili gözüksün
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
