using UnityEngine;
using System.Collections;

public class PlatformSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlatformPooler pooler;

    [Header("Spawn Settings")]
    [SerializeField] private float minSpawnTime = 2f;
    [SerializeField] private float maxSpawnTime = 4f;

    [SerializeField] private string[] platformTags = { "Floating", "Broken" };

    void Start()
    {
        if (pooler == null)
        {
            pooler = GetComponent<PlatformPooler>();
        }

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        // 1. WAIT here until the game actually starts
        while (GameManager.Instance == null || !GameManager.Instance.isGameActive)
        {
            yield return null; // Wait for the next frame and check again
        }

        // 2. Once the game is active, start the infinite spawning loop
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            // Double check game is still active (useful if you add a 'Game Over' later)
            if (GameManager.Instance.isGameActive)
            {
                string randomTag = platformTags[Random.Range(0, platformTags.Length)];
                pooler.SpawnFromPool(randomTag);
            }
        }
    }
}