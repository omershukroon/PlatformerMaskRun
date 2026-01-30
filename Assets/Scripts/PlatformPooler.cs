using System.Collections.Generic;
using UnityEngine;

public class PlatformPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    [Header("Pool Setup")]
    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    [Header("World Boundaries")]
    public float despawnXPosition = -15f;
    public float minY = -2f;
    public float maxY = 3f;

    [Header("Height Spacing Logic")]
    [Tooltip("The minimum vertical distance between consecutive platforms")]
    public float minHeightDifference = 1.28f;
    private float lastSpawnHeight;

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // אתחול הגובה האחרון למרכז הטווח כדי להתחיל משם
        lastSpawnHeight = (minY + maxY) / 2f;

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);

                if (obj.TryGetComponent(out PlatformMovement movement))
                {
                    movement.SetMovement(false);
                }

                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag)) return null;

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        // מיקום ה-X של ה-Pool Manager
        float spawnX = transform.position.x;

        // הגרלת גובה חכמה שמתחשבת במרחק המינימלי
        float randomY = GetSmartRandomHeight();
        lastSpawnHeight = randomY;

        objectToSpawn.transform.position = new Vector3(spawnX, randomY, 0);

        objectToSpawn.SetActive(true);

        // הפעלת לוגיקת האורך הרנדומלי
        if (objectToSpawn.TryGetComponent(out DynamicPlatform dynamic))
        {
            dynamic.GenerateRandomLength();
        }

        if (objectToSpawn.TryGetComponent(out PlatformMovement movement))
        {
            movement.SetMovement(true);
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    // פונקציה פנימית לחישוב גובה רנדומלי עם מרווח
    private float GetSmartRandomHeight()
    {
        float newY = lastSpawnHeight;
        int attempts = 0;

        // מנסה להגריל גובה עד שהוא רחוק מספיק, אך לא יותר מ-10 ניסיונות למניעת לולאה אינסופית
        while (Mathf.Abs(newY - lastSpawnHeight) < minHeightDifference && attempts < 10)
        {
            newY = Random.Range(minY, maxY);
            attempts++;
        }

        return newY;
    }

    void Update()
    {
        foreach (var pool in poolDictionary)
        {
            foreach (GameObject obj in pool.Value)
            {
                if (obj.activeInHierarchy && obj.transform.position.x <= despawnXPosition)
                {
                    ReturnToPool(obj);
                }
            }
        }
    }

    void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        if (obj.TryGetComponent(out PlatformMovement movement))
        {
            movement.SetMovement(false);
        }
    }
}