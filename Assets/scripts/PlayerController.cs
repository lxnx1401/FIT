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

        bool walk = keyboard.wKey.isPressed;
        bool back = keyboard.sKey.isPressed;
        bool left = keyboard.aKey.isPressed;
        bool right = keyboard.dKey.isPressed;

        anim.SetBool("walk", walk);
        anim.SetBool("back", back);
        anim.SetBool("left", left);
        anim.SetBool("right", right);
    }
}
