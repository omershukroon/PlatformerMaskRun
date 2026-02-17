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
    private bool hasGameStartedBefore = false;

    // משתנה שישמור את שם המסכה האחרונה שיצאה
    private string lastSpawnedMaskName = "";

    void Start()
    {
        InitializePool();
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameActive)
        {
            // אם המשחק פעיל אבל זו הפעם הראשונה שאנחנו מגלים זאת:
            if (!hasGameStartedBefore)
            {
                SetNextSpawnTime(); // קבע את הדיליי הראשון מהרגע הזה
                hasGameStartedBefore = true;
            }

            if (Time.time >= nextSpawnTime)
            {
                SpawnMaskFromPool();
                SetNextSpawnTime();
            }
        }
        else
        {
            // אם המשחק נעצר (למשל הפסד), אפשר לאפס את הדגל כדי שגם בהפעלה הבאה יהיה דיליי
            hasGameStartedBefore = false;
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
                // אנחנו נותנים לאובייקט ב-Pool את השם של הפריפב כדי שנוכל לזהות את הסוג שלו
                obj.name = prefab.name;
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
                // עדכון השם של המסכה האחרונה שיצאה
                lastSpawnedMaskName = mask.name;

                float spawnY = hit.point.y + heightAboveGround;
                mask.transform.position = new Vector3(currentX, spawnY, 0);
                mask.SetActive(true);
            }
        }
    }

    private GameObject GetRandomInactiveMask()
    {
        // 1. מוצאים את כל המסכות הכבויות
        List<GameObject> inactiveMasks = maskPool.FindAll(m => !m.activeInHierarchy);

        if (inactiveMasks.Count == 0) return null;

        // 2. מסננים החוצה את המסכות מהסוג שיצא פעם אחרונה
        // (רק אם יש לנו יותר מסוג אחד של מסכה פנויה, כדי לא להיתקע)
        List<GameObject> filteredMasks = inactiveMasks.FindAll(m => m.name != lastSpawnedMaskName);

        // 3. אם יש מסכות מסוגים אחרים - נבחר אחת מהן
        if (filteredMasks.Count > 0)
        {
            return filteredMasks[Random.Range(0, filteredMasks.Count)];
        }

        // 4. אם נשארו רק מסכות מהסוג האחרון (מקרה קצה), נאלץ להוציא אחת מהן
        return inactiveMasks[Random.Range(0, inactiveMasks.Count)];
    }

    private void SetNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(15f, 20f);
    }
}