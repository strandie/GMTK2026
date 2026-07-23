using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float coyoteTime = 0.15f;

    private float coyoteTimer;

    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        sr = GetComponent<SpriteRenderer>();
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        return hit.collider != null;
    }

    // Visualize ray for IsGrounded
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.DrawRay(
                groundCheck.position,
                Vector2.down * groundCheckDistance
            );
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && coyoteTimer > 0)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            coyoteTimer = 0;
        }
    }

    private void FixedUpdate()
    {
        bool grounded = IsGrounded();

        if (grounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }

        rb.linearVelocity = new Vector2(
            moveInput.x * moveSpeed,
            rb.linearVelocity.y
        );

        if (moveInput.x > 0.1f)
            sr.flipX = false;

        if (moveInput.x < -0.1f)
            sr.flipX = true;
    }
}