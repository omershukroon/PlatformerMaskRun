using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private Camera cam;

    [Header("Movement Settings")]
    public float jumpForce = 10f;
    public float recoverySpeed = 2f; // How fast the player moves forward
    public LayerMask groundLayer;

    private bool hasStartedRunning = false;
    private bool isGrounded = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main; // Get the main camera
    }

    void Update()
    {

        // 1. Start Running Logic
        if (!hasStartedRunning && (Pointer.current.press.wasPressedThisFrame))
        {

            hasStartedRunning = true;
            anim.SetBool("isRunning", true);

            // Tell the Manager to start everything else!
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartGame();
            }
        }

        if (hasStartedRunning)
        {
            HandleJump();
            HandleScreenPosition();
        }
    }

    void HandleJump()
    {
        bool jumpPressed = Keyboard.current.spaceKey.wasPressedThisFrame ||
                          (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame);

        if (jumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            anim.SetBool("isJumping", true);
        }
    }

    void HandleScreenPosition()
    {
        // Convert the player's world position to a Viewport point (0 to 1)
        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);

        // If viewPos.x is less than 0.1 (meaning the player is in the leftmost 10% of the screen)
        if (viewPos.x < 0.15f) 
        {
            // Move the player slightly to the right (positive X)
            transform.Translate(Vector2.right * recoverySpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
            anim.SetBool("isJumping", false);
        }
    }
}