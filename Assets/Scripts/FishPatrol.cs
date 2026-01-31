using UnityEngine;

public class FishPatrol : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;

    [Header("Patrol Boundaries")]
    // Set to your background limits
    public float leftLimit = -18.1f;
    public float rightLimit = 17.01f;

    private bool movingRight = false;

    void Update()
    {
        // 1. Handle Movement
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= rightLimit)
            {
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= leftLimit)
            {
                Flip();
            }
        }
    }

    public void SetInitialDirection(bool startsOnRight)
    {
        // If starting on the right, it must move Left initially
        movingRight = !startsOnRight;
        UpdateSpriteFacing();
    }

    void Flip()
    {
        movingRight = !movingRight;
        UpdateSpriteFacing();
    }

    void UpdateSpriteFacing()
    {
        Vector3 scale = transform.localScale;

        // Logic for sprites that face LEFT by default:
        if (movingRight)
        {
            // Move Right -> Flip sprite (Negative X)
            scale.x = -Mathf.Abs(scale.x);
        }
        else
        {
            // Move Left -> Natural state (Positive X)
            scale.x = Mathf.Abs(scale.x);
        }

        transform.localScale = scale;
    }

    void OnDestroy()
    {
        // Find the spawner in the scene and tell it this fish is gone
        FishSpawner spawner = Object.FindFirstObjectByType<FishSpawner>();
        if (spawner != null)
        {
            spawner.RemoveFishFromTrack(this.gameObject);
        }
    }
}