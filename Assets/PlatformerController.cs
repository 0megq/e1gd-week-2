using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformerController : MonoBehaviour
{
    [SerializeField] float maxSpeed = 10.0f;
    [SerializeField] float moveAcceleration = 5.0f;
    [SerializeField] float groundFriction = 5.0f;
    [SerializeField] float jumpImpulse = 5.0f;
    [SerializeField] float dashSpeed = 20.0f;
    [SerializeField] float dashDuration = 0.2f;

    Rigidbody2D rb;
    SpriteRenderer renderer;
    Vector2 moveInput;
    bool isGrounded;

    float timeSinceLastJumpPress = Mathf.Infinity;
    float timeOnGround = 0.0f;
    float timeSinceDash = Mathf.Infinity;
    
    float jumpInputQueueTime = 0.1f;
    bool canDash = true;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move() {
        timeSinceDash += Time.fixedDeltaTime;
        timeSinceLastJumpPress += Time.fixedDeltaTime;

        if (timeSinceDash < dashDuration)
        {
            renderer.color = Color.skyBlue;
            rb.gravityScale = 0.0f;
            return;
        } else
        {
            renderer.color = Color.white;
            rb.gravityScale = 1.0f;
        }

        if (isGrounded) {
            timeOnGround += Time.fixedDeltaTime;
            // apply friction
            rb.linearVelocityX = Mathf.MoveTowards(rb.linearVelocityX, 0, groundFriction * Time.fixedDeltaTime);
        }

        // apply move input
        if (Mathf.Abs(moveInput.x) > 0 && (Mathf.Abs(rb.linearVelocityX) < maxSpeed || MathF.Sign(rb.linearVelocityX) != MathF.Sign(moveInput.x))) {
            rb.linearVelocityX = Mathf.MoveTowards(rb.linearVelocityX, maxSpeed * MathF.Sign(moveInput.x), Time.fixedDeltaTime * moveAcceleration);
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnDash()
    {
        if (moveInput == Vector2.zero) return;
        if (!canDash) return;

        rb.linearVelocity = moveInput * dashSpeed;
        timeSinceDash = 0.0f;
        canDash = false;
    }

    void OnJump()
    {
        if (isGrounded) {
            canDash = true;
            rb.linearVelocityY = jumpImpulse;
        }
        else
            timeSinceLastJumpPress = 0f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            timeOnGround = 0f;
            isGrounded = true;
            if (timeSinceLastJumpPress < jumpInputQueueTime)
            {
                OnJump();
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
