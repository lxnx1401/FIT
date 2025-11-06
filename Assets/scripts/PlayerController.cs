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

        bool walk = keyboard.wKey.isPressed || keyboard.sKey.isPressed || keyboard.aKey.isPressed ||keyboard.dKey.isPressed  ;

        anim.SetBool("walk", walk);
        
    }
}
