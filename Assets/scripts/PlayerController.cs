using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Animator anim;
    PlayerInput playerInput;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip[] footstepClips;    // Lauf- und Sprint-Geräusche
    public AudioClip jumpClip;           // Sprung-Geräusch
    public float stepDelay = 0.5f;       // Zeit zwischen Schritten beim Gehen
    private float stepTimer;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // Eingaben
        bool w = keyboard.wKey.isPressed;
        bool s = keyboard.sKey.isPressed;
        bool a = keyboard.aKey.isPressed;
        bool d = keyboard.dKey.isPressed;
        bool shift = keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed;
        bool space = keyboard.spaceKey.wasPressedThisFrame; // springt nur beim Drücken

        // Logik
        bool jump = space; // nur ein Trigger-Frame
        bool sprint = w && shift; // sprintet nur, wenn Shift + w
        bool walk = (w || s) && !jump; // nur gehen, wenn kein Sprint und kein Sprung
        bool left = a && !jump;
        bool right = d && !jump;

        // Animator
        if (jump)
            anim.SetTrigger("jump"); // Trigger für Sprung
        else
        {
            anim.SetBool("walk", walk);
            anim.SetBool("sprint", sprint);
            anim.SetBool("left", left);
            anim.SetBool("right", right);
        }

        // 🔊 Audio
        HandleFootsteps(walk, sprint, left, right);
        HandleJump(jump);
    }

    void HandleFootsteps(bool walk, bool sprint, bool left, bool right)
    {
        // Idle → kein Sound
        if (!walk && !sprint && !left && !right)
        {
            stepTimer = 0f;
            return;
        }

        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f && footstepClips.Length > 0)
        {
            AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];

            if (sprint)
            {
                audioSource.pitch = Random.Range(1.5f, 2.0f); // schnellerer Pitch beim Sprint
                stepTimer = stepDelay / 1.5f;                // schnellere Schritte
            }
            else
            {
                audioSource.pitch = Random.Range(0.95f, 1.05f);
                stepTimer = stepDelay;
            }

            audioSource.PlayOneShot(clip);
        }
    }

    void HandleJump(bool jump)
    {
        if (jump && jumpClip != null)
        {
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(jumpClip);
        }
    }
}
