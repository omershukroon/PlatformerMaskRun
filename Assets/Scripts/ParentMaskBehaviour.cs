using UnityEngine;

public class ParentMaskBehaviour : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string maskType; // snorkle, tengu, gas, anonymous

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // בדיקה אם זה השחקן
        if (collision.CompareTag("Player"))
        {
            EquipSequence();
        }
    }

    private void EquipSequence()
    {
        // קריאה למנהל המרכזי להפעלת האנימציה המתאימה
        if (GlobalCinematicManager.Instance != null)
        {
            GlobalCinematicManager.Instance.ActivateCinematic(maskType);
        }
        else
        {
            Debug.LogError("GlobalCinematicManager missing in scene!");
        }

        // החזרה ל-Pool (כיבוי המסכה בעולם)
        gameObject.SetActive(false);
    }
}