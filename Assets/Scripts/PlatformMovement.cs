using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float speed = 5f;
    [SerializeField] private bool isActiv = true; 

    void Update()
    {
        if (isActiv)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
    }

    public void SetMovement(bool status)
    {
        isActiv = status;
    }
}