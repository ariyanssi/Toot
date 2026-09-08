using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 15f;

    private Animator animator;
    private Rigidbody2D rb;
    private float moveDirection = 0f;
    private float currentSpeed = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveDirection = 0f;

        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed)
                moveDirection = -1f;
            else if (keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed)
                moveDirection = 1f;
        }

        float targetSpeed = moveDirection * moveSpeed;
        float rate = (moveDirection != 0f) ? acceleration : deceleration;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, rate * Time.deltaTime);

        // فلیپ کردن کاراکتر بر اساس جهت حرکت
        if (moveDirection != 0f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(moveDirection) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        bool isMoving = Mathf.Abs(currentSpeed) > 0.05f;
        animator.SetBool("IsMoving", isMoving);

        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);
    }

    public void MoveLeft()   => moveDirection = -1f;
    public void MoveRight()  => moveDirection = 1f;
    public void StopMoving() => moveDirection = 0f;
}