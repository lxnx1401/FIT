using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2f;
    public float smoothTime = 0.1f;
    public Transform cameraTransform;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 currentVelocity;
    private Vector3 velocityRef;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        HandleInput();
        MovePlayer();
    }

    void HandleInput()
    {
        moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1;

        if (moveInput.magnitude > 1)
            moveInput.Normalize();
    }

    void MovePlayer()
    {
        bool isSprinting =
            (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed)
            && moveInput.y > 0; // sadece ileri koş

        float speed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;

        // Kameraya göre yön
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 targetDir = (right * moveInput.x + forward * moveInput.y).normalized;
        Vector3 targetVelocity = targetDir * speed;

        // Yumuşak hız değişimi
        currentVelocity = Vector3.SmoothDamp(
            currentVelocity,
            targetVelocity,
            ref velocityRef,
            smoothTime
        );

        // CharacterController ile hareket
        controller.Move(currentVelocity * Time.deltaTime);

        // Dönme (isteğe bağlı)
        if (targetDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        }
    }
}
