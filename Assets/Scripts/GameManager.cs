using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isGameActive { get; private set; } = false;
    private static bool shouldAutoStart = false;

    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameHUDPanel;
    [SerializeField] private GameObject pauseMenuPanel;

    [Header("Player Data & UI")]
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_Text displayNameText;
    [SerializeField] private TMP_Text scoreText;

    private static string playerName = "";
    private int currentScore = 0;
    private float scoreTimer = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // אם playerName ריק, ה-InputField יהיה ריק כברירת מחדל
        if (nameInputField != null)
        {
            nameInputField.text = playerName;
        }

        if (shouldAutoStart)
        {
            shouldAutoStart = false;
            StartGame();
        }
    }

    void Update()
    {
        if (isGameActive)
        {
            scoreTimer += Time.deltaTime;
            if (scoreTimer >= 3f)
            {
                AddScore(1);
                scoreTimer = 0f;
            }
        }
    }

    public void StartGame()
    {
        if (isGameActive) return;

        // בדיקה מה הוזן בתיבת הטקסט
        if (nameInputField != null && !string.IsNullOrWhiteSpace(nameInputField.text))
        {
            playerName = nameInputField.text;
        }
        else
        {
            // אם התיבה ריקה, אנחנו קובעים את השם ל-"Player"
            playerName = "Player";
        }

        if (displayNameText != null) displayNameText.text = playerName;
        UpdateScoreUI();

        isGameActive = true;
        Time.timeScale = 1;

        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (gameHUDPanel != null) gameHUDPanel.SetActive(true);
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore.ToString();
        }
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        shouldAutoStart = false;

        // כאן הקסם: איפוס השם הסטטי כדי שהתיבה תהיה ריקה בטעינה הבאה
        playerName = "";

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        shouldAutoStart = true;
        // ב-Restart אנחנו לא מאפסים את השם כי אנחנו רוצים שהוא יישמר לסיבוב הבא
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TogglePause()
    {
        if (!isGameActive) return;

        if (Time.timeScale == 0)
            ResumeGame();
        else
            PauseGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
    }
}