using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject BulletPrefab;
    public Transform BulletSpawn;
    public float TimeBetweenShots = 0.3333f;
    public float BulletSpeed = 50f;

    [Header("Damage Settings")]
    [HideInInspector] public float bulletDamage = 1f;
    [HideInInspector] public bool isDamageBoosted = false;

    [Header("Bullet Materials")]
    public Material normalBulletMat;     // Beyaz materyal
    public Material boostedBulletMat;    // Turuncu materyal

    private float m_timeStamp = 0f;
    private Mouse mouse;

    void Start()
    {
        mouse = Mouse.current;
    }

    void Update()
    {
        if (mouse == null) return;

        // Sol mouse tuşu basılıysa ateş et
        if (Time.time >= m_timeStamp && mouse.leftButton.isPressed)
        {
            Fire();
            m_timeStamp = Time.time + TimeBetweenShots;
        }
    }

    void Fire()
    {
        if (BulletPrefab == null || BulletSpawn == null)
        {
            Debug.LogWarning("BulletPrefab veya BulletSpawn atanmamış!");
            return;
        }

        // Kartopunu oluştur
        GameObject bullet = Instantiate(BulletPrefab, BulletSpawn.position, BulletSpawn.rotation);

        // Renderer'i child içinde bile bul
        Renderer rend = bullet.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            if (isDamageBoosted && boostedBulletMat != null)
                rend.material = boostedBulletMat; // Turuncu
            else if (normalBulletMat != null)
                rend.material = normalBulletMat;  // Beyaz
        }

        // Merminin Rigidbody'sini ayarla
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = bullet.transform.forward * BulletSpeed;
        }
        else
        {
            Debug.LogWarning("Bullet prefabında Rigidbody component yok!");
        }

        // 2 saniye sonra yok et
        Destroy(bullet, 2.0f);
    }
}
