using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float EnemySpeed = 0f;         // Hareket hızı
    public float RotationSpeed = 5f;      // Dönüş hızı
    private GameObject m_player;

    private void Awake()
    {
        // Player'ı etiket ("tag") üzerinden buluyoruz
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (m_player == null)
            return;

        // Oyuncunun pozisyonuna göre yön vektörünü hesapla
        Vector3 direction = m_player.transform.position - transform.position;
        direction.y = 0f; // Y ekseninde dönmeyi engelle (sadece yatay düzlemde dönsün)
        direction.Normalize();

        // Düşmanı oyuncuya doğru döndür
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
        }

        // Düşmanı oyuncuya doğru hareket ettir
        transform.Translate(Vector3.forward * EnemySpeed * Time.deltaTime);
    }
}
