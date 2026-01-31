using UnityEngine;

public class NormalMovement : PlayerMovementBase
{
    [Header("Jump Settings")]
    public float jumpForce = 13f;
    public int maxJumps = 2;
    private int jumpsRemaining;
    private bool isGrounded = true;

    [Header("Gravity Settings")]
    public float tenguGravity = 2f;
    public float defaultGravity = 3f;

    private PlayerMaskManager maskManager;

    public override void Init(Rigidbody2D playerRb, PlayerController playerController)
    {
        base.Init(playerRb, playerController);
        jumpsRemaining = maxJumps;

        // השגת רפרנס למנהל המסכות שיושב על השחקן
        maskManager = controller.GetComponent<PlayerMaskManager>();
    }

    private void OnEnable()
    {
        if (rb != null)
        {
            UpdateGravity(); // עדכון מיידי כשנכנסים למצב תנועה רגיל
        }
    }

    public override void HandleMovement()
    {
        // עדכון ה-Gravity בכל פריים למקרה שהמסכה השתנתה תוך כדי תנועה רגילה
        UpdateGravity();

        controller.GetComponent<Animator>().SetBool("isRunning", true);
        controller.GetComponent<Animator>().SetBool("isSwimming", false);
    }

    private void UpdateGravity()
    {
        if (maskManager != null)
        {
            // אם מסכת טנגו פעילה - כוח משיכה נמוך, אחרת כוח משיכה רגיל
            rb.gravityScale = maskManager.isTenguMask ? tenguGravity : defaultGravity;
        }
        else
        {
            rb.gravityScale = defaultGravity;
        }
    }

    public override void HandleJump()
    {
        if (isGrounded || jumpsRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpsRemaining--;
            isGrounded = false;
            controller.GetComponent<Animator>().SetBool("isJumping", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & controller.groundLayer) != 0)
        {
            isGrounded = true;
            jumpsRemaining = maxJumps;
            controller.GetComponent<Animator>().SetBool("isJumping", false);
        }
    }
}