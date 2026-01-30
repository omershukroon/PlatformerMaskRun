using UnityEngine;
using System.Collections.Generic;

public class MaskPuller : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private GameObject[] maskPrefabs; // תגרור לכאן את 4 הפריפבס של המסכות
    [SerializeField] private int amountPerType = 3;    // כמה מכל סוג ליצור

    [Header("Spawn Settings")]
    [SerializeField] private LayerMask groundLayer;    // תבחר ב-Inspector את שכבת ה-Ground
    [SerializeField] private float raycastStartY = 7f; // מאיזה גובה להתחיל לחפש רצפה
    [SerializeField] private float heightAboveGround = 1f; // גובה הריחוף מעל הרצפה

    private List<GameObject> maskPool = new List<GameObject>();
    private float nextSpawnTime;

    void Start()
    {
        // 1. יצירת ה-Pool
        InitializePool();

        // 2. קביעת זמן ההופעה הראשון (בין 15 ל-20 שניות)
        SetNextSpawnTime();
    }

    void Update()
    {
        // בדיקה אם הגיע הזמן ליצור מסכה
        if (Time.time >= nextSpawnTime)
        {
            SpawnMaskFromPool();
            SetNextSpawnTime();
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
                obj.SetActive(false); // כבוי בברירת מחדל
                maskPool.Add(obj);
            }
        }
    }

    private void SpawnMaskFromPool()
    {
        // 1. חיפוש מיקום Y לפי ה-Ground
        // יורים קרן מהשמיים למטה במיקום ה-X של האובייקט
        float currentX = transform.position.x;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(currentX, raycastStartY), Vector2.down, 20f, groundLayer);

        if (hit.collider != null)
        {
            // 2. מציאת מסכה כבויה רנדומלית מה-Pool
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
        // מערבב את הרשימה כדי שלא תמיד תצא אותה מסכה אם כולן פנויות
        List<GameObject> inactiveMasks = maskPool.FindAll(m => !m.activeInHierarchy);

        if (inactiveMasks.Count > 0)
        {
            return inactiveMasks[Random.Range(0, inactiveMasks.Count)];
        }
        return null; // אין מסכות פנויות ב-Pool
    }

    private void SetNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(15f, 20f);
    }
}