using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance;

    [Header("Sub-Managers")]
    [SerializeField] private BackgroundTheme background;
    [SerializeField] private GroundTheme ground;
    [SerializeField] private PlatformTheme platforms;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateTheme(string maskName)
    {
        string theme = maskName.ToLower();

        // המנהל פוקד על כולם להשתנות
        background.ChangeBackground(theme);
        ground.ChangeGround(theme);
        platforms.ChangePlatforms(theme);

        Debug.Log("World Theme Changed to: " + theme);
    }
}