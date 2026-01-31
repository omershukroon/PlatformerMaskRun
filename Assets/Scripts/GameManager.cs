using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Allows other scripts to find this easily

    public bool isGameActive { get; private set; } = false;

    [Header("Managed Parents")]
    public GameObject backgroundParent;
    public GameObject floorParent;
    public GameObject platformSpawner;
    public GameObject maskSpawner;
    public GameObject fishSpawner;

    void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        if (isGameActive) return; // Don't start twice

        isGameActive = true;
        Debug.Log("Game Started! Spawning and movement initiated.");
    }
}