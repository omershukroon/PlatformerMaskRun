using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    public float speed = 5f;
    public float resetXPosition = -15.35f;
    public float startXPosition = 15f;

    // המרחק הכולל שהחלק עובר מקצה לקצה
    private float tripLength;

    void Start()
    {
        // חישוב המרחק פעם אחת בתחילת המשחק
        tripLength = startXPosition - resetXPosition;
    }

    void Update()
    {
        // תנועה שמאלה
        transform.position += Vector3.left * speed * Time.deltaTime;

        // בדיקה אם עברנו את הגבול
        if (transform.position.x <= resetXPosition)
        {
            // במקום להציב מיקום קבוע, אנחנו מוסיפים את אורך הטיול
            // זה שומר על ה"שארית" הקטנה ומונע רווחים
            Vector3 newPos = transform.position;
            newPos.x += tripLength;
            transform.position = newPos;
        }
    }
}
