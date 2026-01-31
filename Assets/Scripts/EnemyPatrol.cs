using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;
    public bool facesLeftByDefault = true;
    public bool isGroundedEnemy = false; // NEW: Set TRUE for the Mouse prefab

    [Header("Patrol Boundaries")]
    public float leftLimit = -18.1f;
    public float rightLimit = 17.01f;

    private bool movingRight = false;
    private EnemySpawner spawner;
    private float initialY; // NEW: To lock the mouse to the ground

    void Start()
    {
        spawner = Object.FindFirstObjectByType<EnemySpawner>();
        initialY = transform.position.y; // NEW: Capture the spawn height
    }

    void Update()
    {
        // NEW: If grounded, ensure it stays at the initial Y even if physics tries to nudge it
        if (isGroundedEnemy)
        {
            transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
        }

        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= rightLimit) Flip();
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= leftLimit) Flip();
        }
    }

    public void SetInitialDirection(bool startsOnRight)
    {
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
        if (facesLeftByDefault)
        {
            scale.x = movingRight ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        }
        else
        {
            scale.x = movingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        }
        transform.localScale = scale;
    }

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.RemoveEnemyFromTrack(this.gameObject);
        }
    }
}