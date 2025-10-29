using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int health = 2; // Düşmanın canı - 2 mermiye kadar dayanacak

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject); // Oyuncuya temas ederse, oyuncuyu yok et
        }
        else if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject); // Mermiyi yok et

            // Düşmanın canını azalt
            health--;

            // Canı sıfır veya altına düştüyse düşmanı yok et
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
