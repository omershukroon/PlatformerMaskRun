using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMaskManager : MonoBehaviour
{
    [Header("Player Masks References")]
    [SerializeField] private GameObject snorkleMask;
    [SerializeField] private GameObject tenguMask;
    [SerializeField] private GameObject gasMask;
    [SerializeField] private GameObject anonymousMask;

    [Header("Status Flags")]
    public bool isSnorkleMask = false;
    public bool isTenguMask = false;
    public bool isGasMask = false;
    public bool isAnonymousMask = false;

    public void ActivateMaskOnFace(string maskName)
    {
        // 1. קודם כל מכבים הכל ומאפסים את כל ה-bools
        DeactivateAllMasks();

        // 2. מדליקים רק את מה שצריך
        switch (maskName.ToLower())
        {
            case "snorkle":
                if (snorkleMask != null) snorkleMask.SetActive(true);
                isSnorkleMask = true;
                break;
            case "tengu":
                if (tenguMask != null) tenguMask.SetActive(true);
                isTenguMask = true;
                break;
            case "gas":
                if (gasMask != null) gasMask.SetActive(true);
                isGasMask = true;
                break;
            case "anonymous":
                if (anonymousMask != null) anonymousMask.SetActive(true);
                isAnonymousMask = true;
                break;
            default:
                Debug.LogWarning("Mask Name " + maskName + " not found!");
                break;
        }

        // 3. עדכון ה-Theme של העולם
        if (ThemeManager.Instance != null)
        {
            ThemeManager.Instance.UpdateTheme(maskName);
        }
    }

    public void DeactivateAllMasks()
    {
        // כיבוי אובייקטים
        if (snorkleMask != null) snorkleMask.SetActive(false);
        if (tenguMask != null) tenguMask.SetActive(false);
        if (gasMask != null) gasMask.SetActive(false);
        if (anonymousMask != null) anonymousMask.SetActive(false);

        // איפוס משתני מצב
        isSnorkleMask = false;
        isTenguMask = false;
        isGasMask = false;
        isAnonymousMask = false;
    }
}