using UnityEngine;

public class FloorMovement : MonoBehaviour
{

    public float speed = 5f;
    public float resetXPosition = -15f;
    public float startXPosition = 15f;

    private float tripLength;

    void Start()
    {
        tripLength = startXPosition - resetXPosition;
    }

    void Update()
    {
        // Only move if the GameManager says the game is active
        if (GameManager.Instance != null && GameManager.Instance.isGameActive)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        if (transform.position.x <= resetXPosition)
        {
            Vector3 newPos = transform.position;
            newPos.x += tripLength;
            transform.position = newPos;
        }
    }
}

