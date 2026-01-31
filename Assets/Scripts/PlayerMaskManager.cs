using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMaskManager : MonoBehaviour
{
    [Header("Player Masks References")]
    [SerializeField] private GameObject snorkleMask;
    [SerializeField] private GameObject tenguMask;
    [SerializeField] private GameObject gasMask;
    [SerializeField] private GameObject anonymousMask;
    [Header("Ciling References")]
    [SerializeField] private GameObject Ciling;

    [Header("Timer Settings")]
    [SerializeField] private float maskDuration = 10f; // משך הזמן שהמסכה נשארת

    [Header("Status Flags")]
    public bool isSnorkleMask = false;
    public bool isTenguMask = false;
    public bool isGasMask = false;
    public bool isAnonymousMask = false;
    public bool isNoMask = true;

    public void ActivateMaskOnFace(string maskName)
    {
        // 1. קודם כל מבטלים טיימרים קודמים כדי שאיסוף מסכה חדשה יאפס את ה-10 שניות
        CancelInvoke(nameof(DeactivateAllMasks));

        // 2. כיבוי הכל ואיפוס בולס
        DeactivateAllMasks();

        // 2. מדליקים רק את מה שצריך
        switch (maskName.ToLower())
        {
            case "snorkle":
                if (snorkleMask != null) snorkleMask.SetActive(true);
                isSnorkleMask = true;
                Ciling.SetActive(true);
                break;
            case "tengu":
                if (tenguMask != null) tenguMask.SetActive(true);
                isTenguMask = true;
                Ciling.SetActive(false);
                break;
            case "gas":
                if (gasMask != null) gasMask.SetActive(true);
                isGasMask = true;
                Ciling.SetActive(false);
                break;
            case "anonymous":
                if (anonymousMask != null) anonymousMask.SetActive(true);
                isAnonymousMask = true;
                Ciling.SetActive(false);
                break;
            default:
                Debug.LogWarning("Mask Name " + maskName + " not found!");
                break;
        }

        isNoMask = false;

        // 4. עדכון ה-Theme והתנועה
        if (ThemeManager.Instance != null)
        {
            ThemeManager.Instance.UpdateTheme(maskName);
        }

        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.SetMovementStyle(isSnorkleMask);
        }

        // 5. הפעלת טיימר להסרת המסכה בעוד 10 שניות
        Invoke(nameof(DeactivateAllMasks), maskDuration);
    }

    public void DeactivateAllMasks()
    {
        // 1. Clear enemies from the screen whenever masks are removed
        EnemySpawner spawner = Object.FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.DestroyAllActiveEnemies();
        }

        // 2. Turn off all mask visuals
        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null) pc.SetMovementStyle(false);

        // אם המסכה יורדת מעצמה, נרצה להחזיר את ה-Theme למצב רגיל (למשל Default)
        if (ThemeManager.Instance != null && !isNoMask)
        {
            ThemeManager.Instance.UpdateTheme("default");
        }

        // כיבוי אובייקטים
        if (snorkleMask != null) snorkleMask.SetActive(false);
        if (tenguMask != null) tenguMask.SetActive(false);
        if (gasMask != null) gasMask.SetActive(false);
        if (anonymousMask != null) anonymousMask.SetActive(false);
        if (Ciling != null) Ciling.SetActive(false);

        // 3. Reset all status flags
        isSnorkleMask = false;
        isTenguMask = false;
        isGasMask = false;
        isAnonymousMask = false;
        isNoMask = true;
    }
}