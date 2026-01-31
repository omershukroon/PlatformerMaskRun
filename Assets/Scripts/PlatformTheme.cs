using UnityEngine;

public class PlatformTheme : MonoBehaviour
{
    [Header("Root Object")]
    [SerializeField] private Transform platformPoolerRoot; // גרור לכאן את אובייקט ה-PlatformPooler

    [System.Serializable]
    public class PlatformMaskAssets
    {
        public string maskName;
        public Color themeColor = Color.white;

        [Header("Regular Platform Sprites")]
        public Sprite regLeft;
        public Sprite regMiddle;
        public Sprite regRight;

        [Header("Broken Platform Sprites")]
        public Sprite brokenLeft;
        public Sprite brokenMiddle;
        public Sprite brokenRight;
    }

    [Header("Themes Configuration")]
    [SerializeField] private PlatformMaskAssets[] themes;
    [SerializeField] private PlatformMaskAssets defaultTheme;

    public void ChangePlatforms(string maskName)
    {
        PlatformMaskAssets activeTheme = GetThemeData(maskName);

        // תיקון Alpha לאטימות מלאה
        Color finalColor = activeTheme.themeColor;
        finalColor.a = 1f;

        // עוברים על כל הילדים של ה-Pooler (גם אלו ש-SetActive(false))
        // השימוש ב-GetComponentsInChildren(true) מבטיח שנגיע גם לאלו שב"מחסן"
        foreach (Transform platform in platformPoolerRoot.GetComponentsInChildren<Transform>(true))
        {
            // אנחנו מחפשים רק את האובייקטים שהם "הורים" של פלטפורמה (שיש להם 3 ילדים)
            if (platform.parent == platformPoolerRoot && platform.childCount >= 3)
            {
                UpdateSinglePlatform(platform, activeTheme, finalColor);
            }
        }
    }

    private void UpdateSinglePlatform(Transform platform, PlatformMaskAssets assets, Color color)
    {
        // לפי המבנה ששלחת: ילד 0 = שמאל, ילד 1 = אמצע, ילד 2 = ימין
        SpriteRenderer leftSR = platform.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer midSR = platform.GetChild(1).GetComponent<SpriteRenderer>();
        SpriteRenderer rightSR = platform.GetChild(2).GetComponent<SpriteRenderer>();

        // בדיקה האם זו פלטפורמה שבורה לפי השם של ה-Prefab
        bool isBroken = platform.name.Contains("Broken");

        if (isBroken)
        {
            if (leftSR) { leftSR.sprite = assets.brokenLeft; leftSR.color = color; }
            if (midSR) { midSR.sprite = assets.brokenMiddle; midSR.color = color; }
            if (rightSR) { rightSR.sprite = assets.brokenRight; rightSR.color = color; }
        }
        else
        {
            if (leftSR) { leftSR.sprite = assets.regLeft; leftSR.color = color; }
            if (midSR) { midSR.sprite = assets.regMiddle; midSR.color = color; }
            if (rightSR) { rightSR.sprite = assets.regRight; rightSR.color = color; }
        }
    }

    private PlatformMaskAssets GetThemeData(string name)
    {
        foreach (var theme in themes)
        {
            if (theme.maskName.ToLower() == name.ToLower()) return theme;
        }
        return defaultTheme;
    }
}