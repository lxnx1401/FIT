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
    public Material normalBulletMat;     
    public Material boostedBulletMat;    

    [Header("Audio Settings")]
    public AudioSource audioSource;          // Referenz zur AudioSource
    public AudioClip[] shootSounds;          // Eine oder mehrere Schuss-Sounds

    private float m_timeStamp = 0f;
    private Mouse mouse;

    void Start()
    {
        mouse = Mouse.current;

        // Falls keine AudioSource manuell gesetzt wurde, versuch sie automatisch zu finden
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (mouse == null) return;

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
            Debug.LogWarning("BulletPrefab oder BulletSpawn ist nicht zugewiesen!");
            return;
        }

        // 🔸 SOUND abspielen
        PlayShootSound();

        // Projektil erstellen
        GameObject bullet = Instantiate(BulletPrefab, BulletSpawn.position, BulletSpawn.rotation);

        Renderer rend = bullet.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            if (isDamageBoosted && boostedBulletMat != null)
                rend.material = boostedBulletMat; 
            else if (normalBulletMat != null)
                rend.material = normalBulletMat;
        }

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = bullet.transform.forward * BulletSpeed;
        }
        else
        {
            Debug.LogWarning("Bullet Prefab hat kein Rigidbody!");
        }

        Destroy(bullet, 2.0f);
    }

    void PlayShootSound()
    {
        if (audioSource == null || shootSounds.Length == 0) return;

        // Wähle zufälligen Sound, falls mehrere vorhanden sind
        AudioClip clip = shootSounds[Random.Range(0, shootSounds.Length)];
        audioSource.PlayOneShot(clip);
    }
}
