using UnityEngine;

public class BackgroundTheme : MonoBehaviour
{
    [Header("Root Object")]
    [SerializeField] private Transform backgroundRoot;

    [System.Serializable]
    public class MaskThemeAssets
    {
        public string maskName;
        public Sprite cloudSprite;
        public Sprite hillsSprite;
        public Sprite treesSprite;
        public Color themeColor = Color.white; // הגדרת ברירת מחדל ללבן אטום
    }

    [Header("Themes Configuration")]
    [SerializeField] private MaskThemeAssets[] themes;
    [SerializeField] private MaskThemeAssets defaultTheme;

    public void ChangeBackground(string maskName)
    {
        MaskThemeAssets activeTheme = GetThemeData(maskName);

        // תיקון ה-Alpha בקוד: יוצרים עותק של הצבע ומוודאים שהוא אטום
        Color finalColor = activeTheme.themeColor;
        finalColor.a = 1f;

        foreach (Transform rectangle in backgroundRoot)
        {
            if (rectangle.childCount < 2) continue;

            SpriteRenderer floorRenderer = rectangle.GetChild(0).GetComponent<SpriteRenderer>();
            SpriteRenderer cloudRenderer = rectangle.GetChild(1).GetComponent<SpriteRenderer>();

            if (cloudRenderer != null)
            {
                cloudRenderer.sprite = activeTheme.cloudSprite;
                cloudRenderer.color = finalColor; // שימוש בצבע המתוקן
            }

            if (floorRenderer != null)
            {
                floorRenderer.color = finalColor; // שימוש בצבע המתוקן

                if (rectangle.name.Contains("trees"))
                {
                    floorRenderer.sprite = activeTheme.treesSprite;
                }
                else
                {
                    floorRenderer.sprite = activeTheme.hillsSprite;
                }
            }
        }
    }

    private MaskThemeAssets GetThemeData(string name)
    {
        foreach (var theme in themes)
        {
            if (theme.maskName.ToLower() == name.ToLower()) return theme;
        }
        return defaultTheme;
    }
}