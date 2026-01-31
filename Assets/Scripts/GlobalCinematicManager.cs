using UnityEngine;

public class GlobalCinematicManager : MonoBehaviour
{
    // הגדרת ה-Singleton
    public static GlobalCinematicManager Instance;

    [Header("Cinematic Masks References")]
    [SerializeField] private GameObject snorkleMask;
    [SerializeField] private GameObject tenguMask;
    [SerializeField] private GameObject gasMask;
    [SerializeField] private GameObject anonymousMask;

    private void Awake()
    {
        // בדיקה שקיים רק עותק אחד של המנהל
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // פונקציה מרכזית להפעלת מסכה לפי סוג
    public void ActivateCinematic(string maskType)
    {
        // כיבוי כל המסכות הקולנועיות ליתר ביטחון לפני הפעלה
        DeactivateAll();

        switch (maskType.ToLower())
        {
            case "snorkle":
                if (snorkleMask != null) snorkleMask.SetActive(true);
                break;
            case "tengu":
                if (tenguMask != null) tenguMask.SetActive(true);
                break;
            case "gas":
                if (gasMask != null) gasMask.SetActive(true);
                break;
            case "anonymous":
                if (anonymousMask != null) anonymousMask.SetActive(true);
                break;
            default:
                Debug.LogWarning("Mask type " + maskType + " not found!");
                break;
        }
    }


    private void DeactivateAll()
    {
        if (snorkleMask) snorkleMask.SetActive(false);
        if (tenguMask) tenguMask.SetActive(false);
        if (gasMask) gasMask.SetActive(false);
        if (anonymousMask) anonymousMask.SetActive(false);
    }
}