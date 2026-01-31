using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject[] enemyPrefabs;
    public Transform enemyParent;
    public PlayerMaskManager maskManager;

    [Header("Settings")]
    public float spawnRate = 4f;
    public int maxEnemyCount = 4;
    public float minDistanceBetweenEnemies = 2.5f;

    [Header("Spawn Bounds")]
    public float minY = -3.5f;
    public float maxY = 4.5f;
    public float rightLimit = 17.01f;

    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        if (maskManager == null)
            maskManager = Object.FindFirstObjectByType<PlayerMaskManager>();

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (GameManager.Instance != null && GameManager.Instance.isGameActive &&
                activeEnemies.Count < maxEnemyCount)
            {
                AttemptSpawn();
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }

    void AttemptSpawn()
    {
        GameObject selectedPrefab = null;
        bool forceGroundSpawn = false; // NEW: logic for grounded enemies

        if (maskManager.isSnorkleMask)
        {
            selectedPrefab = enemyPrefabs[Random.Range(0, 2)]; // Blue/Yellow Fish
        }
        else if (maskManager.isTenguMask)
        {
            selectedPrefab = enemyPrefabs[2]; // Bat
        }
        else if (maskManager.isGasMask) // NEW: Gas Mask Logic
        {
            int randomIndex = Random.Range(3, 5); // Fly (3) or Mouse (4)
            selectedPrefab = enemyPrefabs[randomIndex];

            if (randomIndex == 4) forceGroundSpawn = true; // If Mouse, force to floor
        }

        if (selectedPrefab != null)
        {
            SpawnEnemy(selectedPrefab, forceGroundSpawn);
        }
    }

    void SpawnEnemy(GameObject prefab, bool forceGround)
    {
        // If grounded, use the minY (floor); otherwise, randomize height
        float randomY = forceGround ? minY : Random.Range(minY, maxY);
        Vector2 spawnPos = new Vector2(rightLimit, randomY);

        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null && Vector2.Distance(spawnPos, enemy.transform.position) < minDistanceBetweenEnemies)
                return;
        }

        GameObject newEnemy = Instantiate(prefab, spawnPos, Quaternion.identity);
        newEnemy.SetActive(true);
        newEnemy.transform.position = new Vector3(rightLimit, randomY, -1f); // Layering

        if (enemyParent != null) newEnemy.transform.SetParent(enemyParent);

        activeEnemies.Add(newEnemy);

        EnemyPatrol patrol = newEnemy.GetComponent<EnemyPatrol>();
        if (patrol != null) patrol.SetInitialDirection(true);
    }

    public void RemoveEnemyFromTrack(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy)) activeEnemies.Remove(enemy);
    }

    public void DestroyAllActiveEnemies()
    {
        // Loop backwards through the list to avoid errors while removing items
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] != null)
            {
                Destroy(activeEnemies[i]);
            }
        }

        // Clear the list so the count returns to zero
        activeEnemies.Clear();
    }
}