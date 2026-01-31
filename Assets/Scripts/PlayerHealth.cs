using UnityEngine;
using UnityEngine.UI; // חשוב בשביל ה-UI

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("UI Settings")]
    [SerializeField] private GameObject heartsContainer; // גרור לכאן את ה-HeartsContainer מה-Canvas

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHeartsUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // מוודא שלא נרד מתחת ל-0

        UpdateHeartsUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHeartsUI()
    {
        if (heartsContainer == null) return;

        // עוברים על כל הלבבות שנמצאים בתוך הקונטיינר
        for (int i = 0; i < heartsContainer.transform.childCount; i++)
        {
            // אם האינדקס של הלב קטן מכמות החיים הנוכחית - הוא דולק
            // אם הוא גדול או שווה - הוא נכבה (השחקן איבד אותו)
            heartsContainer.transform.GetChild(i).gameObject.SetActive(i < currentHealth);
        }
    }

    private void Die()
    {
        Debug.Log("Game Over!");
        // כאן תוכל לקרוא ל-GameManager.Instance.GameOver()
    }
}