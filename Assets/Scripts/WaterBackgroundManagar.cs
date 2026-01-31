using UnityEngine;

public class WaterBackgroundManager : MonoBehaviour
{
    [Header("Position Settings")]
    [SerializeField] private float lowY = -18f;    // גובה התחלתי (מתחת לרצפה)
    [SerializeField] private float highY = -8.5f;  // גובה בזמן שנורקל
    [SerializeField] private float moveSpeed = 5f; // מהירות עלייה/ירידה

    [Header("References")]
    [SerializeField] private PlayerMaskManager playerMask; // גרור לכאן את השחקן

    private float targetY;

    void Start()
    {
        // התחלה במצב נמוך
        Vector3 pos = transform.position;
        pos.y = lowY;
        transform.position = pos;
        targetY = lowY;
    }

    void Update()
    {
        // 1. קביעת היעד לפי מצב המסכה בשחקן
        if (playerMask != null)
        {
            if (playerMask.isSnorkleMask)
            {
                targetY = highY;
            }
            else
            {
                targetY = lowY;
            }
        }

        // 2. תנועה חלקה לעבר היעד
        // MoveTowards מוודא שאנחנו לא עוברים את היעד, אז זה לא יזוז אם אנחנו כבר שם
        float newY = Mathf.MoveTowards(transform.position.y, targetY, moveSpeed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}