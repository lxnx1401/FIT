using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
   public float walkSpeed = 4f;
    public float runSpeed = 7f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    CharacterController controller;
    Animator animator;
    Vector3 velocity;
    bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Bodenprüfung
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Bewegungseingabe
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        bool isMoving = move.magnitude > 0.1f;

        // Laufen vs. Rennen
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Animator-Speed (0 bis 1)
        float targetSpeed = 0f;
        if (isMoving)
            targetSpeed = Input.GetKey(KeyCode.LeftShift) ? 1f : 0.5f;

        animator.SetFloat("Speed", targetSpeed, 0.1f, Time.deltaTime);

        // Springen
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("isJumping", true);
        }

        // Schwerkraft
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Nach Landung
        if (isGrounded && animator.GetBool("isJumping"))
        {
            animator.SetBool("isJumping", false);
        }
    }
}
