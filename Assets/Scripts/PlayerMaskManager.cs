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
        DeactivateAllMasks();

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

        if (ThemeManager.Instance != null)
        {
            ThemeManager.Instance.UpdateTheme(maskName);
        }
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
        if (snorkleMask != null) snorkleMask.SetActive(false);
        if (tenguMask != null) tenguMask.SetActive(false);
        if (gasMask != null) gasMask.SetActive(false);
        if (anonymousMask != null) anonymousMask.SetActive(false);

        // 3. Reset all status flags
        isSnorkleMask = false;
        isTenguMask = false;
        isGasMask = false;
        isAnonymousMask = false;
    }
}