using UnityEngine;

public abstract class PlayerMovementBase : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected PlayerController controller;

    public virtual void Init(Rigidbody2D playerRb, PlayerController playerController)
    {
        rb = playerRb;
        controller = playerController;
    }

    public abstract void HandleMovement();
    public abstract void HandleJump();
}