using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject[] fishPrefabs;
    public Transform enemyParent;
    public float spawnRate = 4f;

    [Header("Limitations")]
    public int maxFishCount = 4; // Total fish allowed at once
    public float minDistanceBetweenFish = 2.5f; // Distance buffer

    [Header("Vertical Spawn Bounds")]
    public float minY = -3.5f;
    public float maxY = 4.5f;

    [Header("Horizontal Patrol Bounds")]
    public float leftLimit = -18.1f;
    public float rightLimit = 17.01f;

    // List to track active fish for the distance check
    private List<GameObject> activeFish = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (GameManager.Instance == null || !GameManager.Instance.isGameActive)
        {
            yield return null;
        }

        while (true)
        {
            // Only try to spawn if we are under the limit
            if (activeFish.Count < maxFishCount)
            {
                SpawnFish();
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }

    void SpawnFish()
    {
        if (fishPrefabs == null || fishPrefabs.Length == 0) return;

        float randomY = Random.Range(minY, maxY);
        float spawnX = rightLimit;
        Vector2 spawnPos = new Vector2(spawnX, randomY);

        // 1. Distance Check: Don't spawn if too close to another fish
        foreach (GameObject fish in activeFish)
        {
            if (fish != null && Vector2.Distance(spawnPos, fish.transform.position) < minDistanceBetweenFish)
            {
                return; // Skip this spawn attempt
            }
        }

        // 2. Create the fish
        GameObject fishPrefab = fishPrefabs[Random.Range(0, fishPrefabs.Length)];
        GameObject newFish = Instantiate(fishPrefab, spawnPos, Quaternion.identity);

        // Ensure visual consistency
        newFish.SetActive(true);
        newFish.transform.position = new Vector3(spawnX, randomY, -1f);

        if (enemyParent != null)
        {
            newFish.transform.SetParent(enemyParent);
        }

        activeFish.Add(newFish);

        FishPatrol patrolScript = newFish.GetComponent<FishPatrol>();
        if (patrolScript != null)
        {
            patrolScript.SetInitialDirection(true);
        }
    }

    // Call this when a fish is destroyed to free up space
    public void RemoveFishFromTrack(GameObject fish)
    {
        if (activeFish.Contains(fish))
        {
            activeFish.Remove(fish);
        }
    }
}