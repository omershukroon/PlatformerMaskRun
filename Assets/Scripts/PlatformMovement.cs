using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float speed = 5f;
    [SerializeField] private bool isActiv = true; // הגדרתי כברירת מחדל כ-true כדי שתראה תנועה מיד

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