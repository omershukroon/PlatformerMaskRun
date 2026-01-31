using UnityEngine;
using System.Collections.Generic;

public class MaskPuller : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private GameObject[] maskPrefabs; 
    [SerializeField] private int amountPerType = 3;    

    [Header("Spawn Settings")]
    [SerializeField] private LayerMask groundLayer;    
    [SerializeField] private float raycastStartY = 7f; 
    [SerializeField] private float heightAboveGround = 1f; 

    private List<GameObject> maskPool = new List<GameObject>();
    private float nextSpawnTime;
    private bool firstSpawnSet = false; // Ensures the timer starts correctly at first click

    void Start()
    {
        InitializePool();

        SetNextSpawnTime();
    }

    void Update()
    {
        // 1. Only run if the game has started
        if (GameManager.Instance != null && GameManager.Instance.isGameActive)
        {
            // 2. Initialize the first spawn timer once the game starts
            if (!firstSpawnSet)
            {
                SetNextSpawnTime();
                firstSpawnSet = true;
            }

            // 3. Regular spawning logic
            if (Time.time >= nextSpawnTime)
            {
                SpawnMaskFromPool();
                SetNextSpawnTime();
            }
        }
    }

    private void InitializePool()
    {
        foreach (GameObject prefab in maskPrefabs)
        {
            for (int i = 0; i < amountPerType; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.transform.SetParent(this.transform);
                obj.SetActive(false); 
                maskPool.Add(obj);
            }
        }
    }

    private void SpawnMaskFromPool()
    {
        float currentX = transform.position.x;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(currentX, raycastStartY), Vector2.down, 20f, groundLayer);

        if (hit.collider != null)
        {
            GameObject mask = GetRandomInactiveMask();

            if (mask != null)
            {
                float spawnY = hit.point.y + heightAboveGround;
                mask.transform.position = new Vector3(currentX, spawnY, 0);
                mask.SetActive(true);
            }
        }
    }

    private GameObject GetRandomInactiveMask()
    {
        List<GameObject> inactiveMasks = maskPool.FindAll(m => !m.activeInHierarchy);

        if (inactiveMasks.Count > 0)
        {
            return inactiveMasks[Random.Range(0, inactiveMasks.Count)];
        }
        return null; 
    }

    private void SetNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(15f, 20f);
    }
}