using UnityEngine;

public class MaskMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f; // מהירות התזוזה שביקשת

    void Update()
    {
        // הזזת המסכה שמאלה לאורך ציר ה-X
        // הכפלה ב-Time.deltaTime מבטיחה תנועה חלקה ללא קשר לקצב הפריימים
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // אופציונלי: בדיקה אם המסכה יצאה מגבולות המסך מצד שמאל
        if (transform.position.x < -15f)
        {
            // החזרה ל-Pool במקום Destroy
            gameObject.SetActive(false);
        }
    }
}