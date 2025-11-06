using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour 
{
    private Vector2 m_moveInput;
    public float PlayerSpeed = 5f;
    public float SprintMultiplier = 2f; // wie viel schneller beim Sprinten

    void Update() 
    {
        // Eingaben holen
        m_moveInput = Vector2.zero;
        
        if (Keyboard.current.wKey.isPressed) m_moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed) m_moveInput.y -= 1;
        if (Keyboard.current.dKey.isPressed) m_moveInput.x += 1;
        if (Keyboard.current.aKey.isPressed) m_moveInput.x -= 1;

        if (m_moveInput.magnitude > 1)
            m_moveInput.Normalize();

        // Sprint nur bei W + Shift
        bool isSprinting = (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed)
                           && Keyboard.current.wKey.isPressed;

        float currentSpeed = isSprinting ? PlayerSpeed * SprintMultiplier : PlayerSpeed;

        // Bewegung anwenden
        Vector3 movement = new Vector3(m_moveInput.x, 0f, m_moveInput.y) * currentSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }
}


