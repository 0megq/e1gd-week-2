using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformerController : MonoBehaviour
{
    [SerializeField] float maxSpeed = 10.0f;
    [SerializeField] float moveAcceleration = 5.0f;
    [SerializeField] float groundFriction = 5.0f;
    [SerializeField] float airFriction = 5.0f;
    [SerializeField] float jumpImpulse = 5.0f;
    [SerializeField] float dashSpeed = 20.0f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] GroundChecker groundChecker;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip dashClip;
    public int coinsCollected = 0;

    Rigidbody2D rb;
    SpriteRenderer renderer;
    Vector2 moveInput;

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

    void Update()
    {
        animator.SetInteger("yVel", MathF.Sign(rb.linearVelocityY));
        animator.SetBool("grounded", groundChecker.isGrounded);
        animator.SetBool("moving", rb.linearVelocityX != 0.0f);
        if (UnityEngine.Random.value > 0.98)
            animator.SetTrigger("hello");
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move() {
        timeSinceDash += Time.fixedDeltaTime;
        timeSinceLastJumpPress += Time.fixedDeltaTime;
        
        // dash reset and jump queueing
        if (groundChecker.isGrounded)
        {
            canDash = true;
            if (timeSinceLastJumpPress < jumpInputQueueTime)
            {
                Jump();
            }
        }

        if (timeSinceDash < dashDuration)
        {
            renderer.color = Color.darkRed;
            rb.gravityScale = 0.0f;
            return;
        } else
        {
            renderer.color = Color.white;
            rb.gravityScale = 1.0f;
        }

        float friction = groundChecker.isGrounded ? groundFriction : airFriction;
        timeOnGround += Time.fixedDeltaTime;
        // apply friction
        if (moveInput.x == 0.0f)
            rb.linearVelocityX = Mathf.MoveTowards(rb.linearVelocityX, 0, friction * Time.fixedDeltaTime);

        // apply move input
        if (Mathf.Abs(moveInput.x) > 0 && (Mathf.Abs(rb.linearVelocityX) < maxSpeed || MathF.Sign(rb.linearVelocityX) != MathF.Sign(moveInput.x))) {
            rb.linearVelocityX = Mathf.MoveTowards(rb.linearVelocityX, maxSpeed * MathF.Sign(moveInput.x), Time.fixedDeltaTime * moveAcceleration);
        }

        if (rb.linearVelocityX > 0)
        {
            renderer.flipX = false;
        } else if (rb.linearVelocityX < 0)
        {
            renderer.flipX = true;
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
        audioSource.PlayOneShot(dashClip);
    }

    void OnJump()
    {
        Debug.Log("trying to hymp" + groundChecker.isGrounded);
        if (groundChecker.isGrounded) {
            Jump();
        } else
            timeSinceLastJumpPress = 0f;
    }

    void Jump()
    {
        rb.linearVelocityY = jumpImpulse;
        audioSource.PlayOneShot(jumpClip);
    }
}
