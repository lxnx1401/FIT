using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour 
{
    public GameObject BulletPrefab;
    public Transform BulletSpawn;
    public float TimeBetweenShots = 0.3333f;
    public float BulletSpeed = 50f;

    private float m_timeStamp = 0f;
    private Mouse mouse;

    void Start()
    {
        mouse = Mouse.current;
    }

    void Update()
    {
        if (mouse == null) return;

        // Sol mouse tuşu basılı ve ateş etme süresi dolmuşsa
        if ((Time.time >= m_timeStamp) && (mouse.leftButton.isPressed))
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

        var bullet = Instantiate(BulletPrefab, BulletSpawn.position, BulletSpawn.rotation);

        // Rigidbody velocity'sini yeni şekilde ayarla
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = bullet.transform.forward * BulletSpeed;
        }
        else
        {
            Debug.LogWarning("Mermide Rigidbody component'i yok!");
        }

        // Mermiyi 2 saniye sonra yok et
        Destroy(bullet, 2.0f);
    }
}