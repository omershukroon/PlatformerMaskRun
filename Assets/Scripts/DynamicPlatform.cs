using UnityEngine;

public class DynamicPlatform : MonoBehaviour
{
    [Header("Parts References")]
    [SerializeField] private SpriteRenderer leftEdge;
    [SerializeField] private SpriteRenderer middlePart;
    [SerializeField] private SpriteRenderer rightEdge;
    [SerializeField] private BoxCollider2D platformCollider;

    private void OnEnable()
    {
        GenerateRandomLength();
    }

    public void GenerateRandomLength()
    {
        // 1. קבלת רוחב יחידה אחת מהספרייט
        float actualUnitSize = middlePart.sprite.bounds.size.x;

        // 2. הגרלת מכפיל (1 עד 4) וחישוב רוחב אמצע חדש
        int randomMultiplier = Random.Range(1, 5);
        float newMiddleWidth = actualUnitSize * randomMultiplier;

        // 3. עדכון ה-Sprite Renderer (במצב Tiled)
        middlePart.size = new Vector2(newMiddleWidth, middlePart.size.y);

        // 4. חישוב רוחב הקצוות
        float leftWidth = leftEdge.bounds.size.x;
        float rightWidth = rightEdge.bounds.size.x;

        // 5. סידור המיקומים (יישור לשמאל)
        // קצה שמאל נשאר ב-0
        leftEdge.transform.localPosition = Vector3.zero;

        // האמצע מתחיל אחרי הקצה השמאלי (וזז חצי רוחב ימינה כי הוא מתרחב מהמרכז)
        float middlePosX = (leftWidth / 2f) + (newMiddleWidth / 2f);
        middlePart.transform.localPosition = new Vector3(middlePosX, 0, 0);

        // קצה ימין ממוקם בסוף האמצע
        float rightPosX = (leftWidth / 2f) + newMiddleWidth + (rightWidth / 2f);
        rightEdge.transform.localPosition = new Vector3(rightPosX, 0, 0);

        // 6. התאמת הקוליידר
        float totalWidth = leftWidth + newMiddleWidth + rightWidth;
        platformCollider.size = new Vector2(totalWidth, platformCollider.size.y);

        // מרכז הקוליידר צריך להיות בדיוק באמצע המרחק הכולל
        platformCollider.offset = new Vector2(totalWidth / 2f - (leftWidth / 2f), platformCollider.offset.y);
    }
}