using UnityEngine;

public class SnorkelMovement : PlayerMovementBase
{
    public float buoyancyForce = 5f;
    public float diveForce = 3.5f;
    public float waterGravityScale = 0.5f; // כוח משיכה מוחלש מאוד בתוך המים

    // פונקציה שנקראת ברגע שהקונטרולר עובר לסקריפט הזה
    public void OnEnable()
    {
        if (rb != null)
        {
            // אנחנו מחלישים את כוח המשיכה כדי שהציפה תהיה קלה יותר
            rb.gravityScale = waterGravityScale;
            // איפוס מהירות כדי שלא ימשיך ליפול מהקפיצה הקודמת
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }
    }

    public override void HandleMovement()
    {
        // אפקט הציפה
        rb.AddForce(Vector2.up * buoyancyForce);

        Animator anim = controller.GetComponent<Animator>();
        anim.SetBool("isSwimming", true);
        anim.SetBool("isRunning", false);
        anim.SetBool("isJumping", false);
    }

    public override void HandleJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -diveForce);
    }

    // כשחוזרים לתנועה רגילה, צריך להחזיר את כוח המשיכה
    public void OnDisable()
    {
        if (rb != null)
        {
            rb.gravityScale = 1f; // מחזיר לכוח משיכה רגיל
        }
    }
}