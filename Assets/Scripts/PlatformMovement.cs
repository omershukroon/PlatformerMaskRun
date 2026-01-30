using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float speed = 5f;
    [SerializeField] private bool isActiv = true; // הגדרתי כברירת מחדל כ-true כדי שתראה תנועה מיד

    void Update()
    {
        // בדיקה האם הפלטפורמה אמורה לנוע
        if (isActiv)
        {
            // תנועה מימין לשמאל: הכפלת הכיוון שמאלה במהירות ובזמן שחלף
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
    }

    // פונקציית עזר למקרה שתרצה להפעיל/לכבות את התנועה מסקריפט אחר (כמו ה-Spawner)
    public void SetMovement(bool status)
    {
        isActiv = status;
    }
}