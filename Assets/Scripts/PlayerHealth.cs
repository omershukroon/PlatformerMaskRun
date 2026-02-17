using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 6;
    private int currentHealth;

    [Header("Invincibility Settings")]
    [SerializeField] private float invincibilityDuration = 1.5f; // משך החסינות בשניות
    private bool isInvincible = false;

    [Header("Visual Feedback")]
    [SerializeField] private SpriteRenderer playerSprite; // גרור לכאן את ה-SpriteRenderer של השחקן

    [Header("UI Settings")]
    [SerializeField] private GameObject heartsContainer;

    private PlayerMaskManager maskManager;

    void Start()
    {
        currentHealth = maxHealth;
        maskManager = GetComponent<PlayerMaskManager>(); 
        UpdateHeartsUI();
    }

    public void TakeDamage(int damage)
    {
        // אם השחקן חסין, אל תעשה כלום
        if (isInvincible) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHeartsUI();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // אם הוא לא מת, הפעל חסינות והבהוב
            StartCoroutine(BecomeInvincible());
        }
    }
    private IEnumerator BecomeInvincible()
    {
        isInvincible = true;

        float timer = 0;
        while (timer < invincibilityDuration)
        {
            // 1. הבהוב השחקן
            playerSprite.enabled = !playerSprite.enabled;

            // 2. הבהוב המסכה (אם קיימת)
            if (maskManager != null)
            {
                SpriteRenderer currentMaskSprite = maskManager.GetCurrentMaskSprite();
                if (currentMaskSprite != null)
                {
                    currentMaskSprite.enabled = playerSprite.enabled; // מסנכרן את ההבהוב עם השחקן
                }
            }

            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        // החזרת הכל למצב גלוי בסוף
        playerSprite.enabled = true;
        if (maskManager != null)
        {
            SpriteRenderer currentMaskSprite = maskManager.GetCurrentMaskSprite();
            if (currentMaskSprite != null) currentMaskSprite.enabled = true;
        }

        isInvincible = false;
    }


    private void UpdateHeartsUI()
    {
        if (heartsContainer == null) return;

        // עוברים על כל ה-Slots (צריכים להיות 3 סלוטים עבור 6 חיים)
        for (int i = 0; i < heartsContainer.transform.childCount; i++)
        {
            Transform slot = heartsContainer.transform.GetChild(i);

            // בתוך כל סלוט יש לנו: 0: Empty, 1: Half, 2: Full
            GameObject halfHeart = slot.GetChild(1).gameObject;
            GameObject fullHeart = slot.GetChild(2).gameObject;

            // חישוב לוגי עבור כל לב (כל i מייצג 2 נקודות חיים)
            int heartValue = (i + 1) * 2;

            if (currentHealth >= heartValue)
            {
                // לב מלא
                fullHeart.SetActive(true);
                halfHeart.SetActive(true);
            }
            else if (currentHealth == heartValue - 1)
            {
                // חצי לב (מכבים את המלא, משאירים את החצי)
                fullHeart.SetActive(false);
                halfHeart.SetActive(true);
            }
            else
            {
                // לב ריק (מכבים את המלא ואת החצי)
                fullHeart.SetActive(false);
                halfHeart.SetActive(false);
            }
        }
    }

    private void Die()
    {
        Debug.Log("Game Over!");
        // כאן תוכל להוסיף קריאה למסך ההפסד שלך
    }
}
