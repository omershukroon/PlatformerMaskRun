using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera cam;

    [Header("Global Settings")]
    public LayerMask groundLayer;
    public float recoverySpeed = 2f;

    [Header("Movement Strategies")]
    [SerializeField] private PlayerMovementBase normalMovement;
    [SerializeField] private PlayerMovementBase snorkelMovement;

    private PlayerMovementBase currentMovement;
    private bool hasStartedRunning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        if (normalMovement != null) normalMovement.Init(rb, this);
        if (snorkelMovement != null) snorkelMovement.Init(rb, this);

        currentMovement = normalMovement;
    }

    void Update()
    {
        if (!hasStartedRunning && Pointer.current.press.wasPressedThisFrame)
        {
            StartRunning();
        }

        if (hasStartedRunning && currentMovement != null)
        {
            currentMovement.HandleMovement();

            if (GetJumpInput())
            {
                currentMovement.HandleJump();
            }

            HandleScreenPosition();
        }
    }

    private void StartRunning()
    {
        hasStartedRunning = true;

        // במקום להפעיל פה ישירות isRunning, אנחנו נותנים ל-currentMovement
        // לקבוע מה האנימציה המתאימה לרגע תחילת הריצה.
        if (currentMovement != null)
        {
            currentMovement.HandleMovement();
        }

        if (GameManager.Instance != null) GameManager.Instance.StartGame();
    }

    private bool GetJumpInput()
    {
        return Keyboard.current.spaceKey.wasPressedThisFrame ||
               (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame);
    }

    public void SetMovementStyle(bool isSnorkeling)
    {
        // כיבוי של שתי האסטרטגיות קודם
        normalMovement.enabled = false;
        snorkelMovement.enabled = false;

        if (isSnorkeling)
        {
            currentMovement = snorkelMovement;
        }
        else
        {
            currentMovement = normalMovement;
        }

        // הפעלה של האסטרטגיה הנבחרת (זה יפעיל את ה-OnEnable שלהן)
        currentMovement.enabled = true;

        if (hasStartedRunning)
        {
            currentMovement.HandleMovement();
        }
    }

    void HandleScreenPosition()
    {
        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
        if (viewPos.x < 0.15f)
        {
            transform.Translate(Vector2.right * recoverySpeed * Time.deltaTime);
        }
    }
}