using UnityEngine;

public class CinematicMaskEvents : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float followSpeed = 15f;
    [SerializeField] private float arrivalDistance = 0.1f;
    [SerializeField] private string maskName;

    private Transform playerTransform;
    private bool isTracking = false;

    void Update()
    {
        if (isTracking && playerTransform != null)
        {
            // תנועה חלקה לעבר המיקום הנוכחי של השחקן (כולל קפיצות)
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, followSpeed * Time.deltaTime);

            // בדיקה האם הגענו לשחקן
            if (Vector3.Distance(transform.position, playerTransform.position) < arrivalDistance)
            {
                FinishSequence();
            }
        }
    }

    // זו הפונקציה שנקראת מה-Animation Event
    public void OnMaskEquipFinished()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            isTracking = true; // מתחילים את המעקב ב-Update

            // ננתק את המסכה מהאבא שלה (אם יש) כדי שהיא תוכל לנוע בחופשיות בעולם
            transform.SetParent(null);
        }
    }

    private void FinishSequence()
    {
        isTracking = false;

        // 1. הפעלת המסכה הקבועה על פני השחקן
        //PlayerMaskManager maskManager = playerTransform.GetComponent<PlayerMaskManager>();
        //if (maskManager != null)
        //{
        //    maskManager.ActivateMaskOnFace(maskName);
        //}

        // 2. כאן אפשר להפעיל את שינוי ה-Theme של העולם
        Debug.Log("Mask Equipped & Theme Changed!");

        transform.localPosition = new Vector3(0, 0, transform.localPosition.z);

        // 3. העלמת המסכה הקולנועית שסיימה לרדוף
        gameObject.SetActive(false);
    }
}