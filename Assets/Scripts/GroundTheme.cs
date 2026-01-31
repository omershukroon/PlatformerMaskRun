using UnityEngine;

public class GroundTheme : MonoBehaviour
{
    [Header("Root Object")]
    [SerializeField] private Transform floorRoot; // כאן תגרור את אובייקט ה-Floor מהסצנה

    [System.Serializable]
    public class GroundMaskAssets
    {
        public string maskName;
        public Sprite groundSprite; // הספריט החדש של הרצפה
        public Color groundColor = Color.white; // צבע אופציונלי לרצפה
    }

    [Header("Ground Themes")]
    [SerializeField] private GroundMaskAssets[] groundThemes;
    [SerializeField] private GroundMaskAssets defaultGround;

    public void ChangeGround(string maskName)
    {
        GroundMaskAssets activeTheme = GetGroundThemeData(maskName);

        // תיקון Alpha כדי למנוע היעלמות
        Color finalColor = activeTheme.groundColor;
        finalColor.a = 1f;

        // עוברים על כל חלקי הרצפה שהם ילדים של Floor
        foreach (Transform segment in floorRoot)
        {
            SpriteRenderer sr = segment.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = activeTheme.groundSprite;
                sr.color = finalColor;
            }
        }
    }

    private GroundMaskAssets GetGroundThemeData(string name)
    {
        foreach (var theme in groundThemes)
        {
            if (theme.maskName.ToLower() == name.ToLower()) return theme;
        }
        return defaultGround;
    }
}