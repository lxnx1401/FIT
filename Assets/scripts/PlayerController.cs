using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Animator anim;
    PlayerInput playerInput;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        

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

        
    }
}
