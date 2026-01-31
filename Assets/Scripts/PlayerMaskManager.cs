using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMaskManager : MonoBehaviour
{
    [Header("Player Masks References")]
    [SerializeField] private GameObject snorkleMask;
    [SerializeField] private GameObject tenguMask;
    [SerializeField] private GameObject gasMask;
    [SerializeField] private GameObject anonymousMask;


    public void ActivateMaskOnFace(string maskName)
    {
        DeactivateAllMasks();
        switch (maskName.ToLower())
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
                Debug.LogWarning("Mask Name " + maskName + " not found!");
                break;
        }

    }
    public void DeactivateAllMasks()
    {
        if (snorkleMask != null) snorkleMask.SetActive(false);
        if (tenguMask != null) tenguMask.SetActive(false);
        if (gasMask != null) gasMask.SetActive(false);
        if (anonymousMask != null) anonymousMask.SetActive(false);
    }
}
