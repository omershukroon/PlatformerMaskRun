using UnityEngine;
using System.Collections;

public class PlatformSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlatformPooler pooler; // קישור לסקריפט ה-Pooler

    [Header("Spawn Settings")]
    [SerializeField] private float minSpawnTime = 2f;
    [SerializeField] private float maxSpawnTime = 4f;

    // רשימת הטאגים שתרצה להגריל מתוכם (Floating, Broken)
    [SerializeField] private string[] platformTags = { "Floating", "Broken" };

    void Start()
    {
        // בדיקה שקישרת את ה-Pooler ב-Inspector
        if (pooler == null)
        {
            pooler = GetComponent<PlatformPooler>();
        }

        // התחלת לולאת היצירה
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        // לולאה אינסופית שרצה כל זמן שהמשחק פעיל
        while (true)
        {
            // 1. המתנה של זמן רנדומלי בין המינימום למקסימום שהגדרת
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            // 2. בחירת סוג פלטפורמה רנדומלי מתוך המערך
            string randomTag = platformTags[Random.Range(0, platformTags.Length)];

            // 3. קריאה לפונקציה ב-Pooler שתוציא פלטפורמה במיקום של ה-Manager
            pooler.SpawnFromPool(randomTag);
        }
    }
}