using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Collision")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.08f;
    [SerializeField] private float wallCheckDistance = 0.08f;

    [Header("Running")]
    [SerializeField] private float maxRunSpeed = 9f;
    [SerializeField] private float groundAcceleration = 110f;
    [SerializeField] private float groundDeceleration = 140f;
    [SerializeField] private float airAcceleration = 70f;
    [SerializeField] private float airDeceleration = 45f;

    [Header("Jumping")]
    [SerializeField] private float jumpVelocity = 14f;
    [SerializeField] private float coyoteTime = 0.12f;
    [SerializeField] private float jumpBufferTime = 0.12f;
    [SerializeField] private float normalGravity = 3.5f;
    [SerializeField] private float fallGravity = 5.5f;
    [SerializeField] private float shortJumpGravity = 7f;
    [SerializeField] private float maxFallSpeed = 22f;

    [Header("Wall Movement")]
    [SerializeField] private float wallSlideSpeed = 4f;
    [SerializeField] private float wallJumpHorizontalVelocity = 11f;
    [SerializeField] private float wallJumpVerticalVelocity = 14f;
    [SerializeField] private float wallJumpControlLockTime = 0.12f;

    [Header("Dash Reference")]
    [SerializeField] private FlickDash flickDash;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsule;
    private SpriteRenderer spriteRenderer;

    private Vector2 moveInput;
    private float coyoteTimer;
    private float jumpBufferTimer;
    private float wallJumpControlLockTimer;
    private bool jumpHeld;
    private bool grounded;
    private int wallSide;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.freezeRotation = true;
        rb.gravityScale = normalGravity;
    }

    private void Update()
    {
        coyoteTimer -= Time.deltaTime;
        jumpBufferTimer -= Time.deltaTime;
        wallJumpControlLockTimer -= Time.deltaTime;
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpHeld = true;
            jumpBufferTimer = jumpBufferTime;
        }

        if (context.canceled)
        {
            jumpHeld = false;
        }
    }

    private void FixedUpdate()
    {
        UpdateCollisionState();

        if (grounded)
        {
            coyoteTimer = coyoteTime;
        }

        bool isDashing = flickDash != null && flickDash.IsDashing;
        if (isDashing)
        {
            rb.gravityScale = normalGravity;
            return;
        }

        TryJump();
        ApplyHorizontalMovement();
        ApplyWallSlide();
        ApplyBetterGravity();
        ClampFallSpeed();
        UpdateSpriteDirection();
    }

    private void UpdateCollisionState()
    {
        grounded = CapsuleCast(Vector2.down, groundCheckDistance);

        bool touchingLeftWall = CapsuleCast(Vector2.left, wallCheckDistance);
        bool touchingRightWall = CapsuleCast(Vector2.right, wallCheckDistance);

        wallSide = touchingLeftWall ? -1 : touchingRightWall ? 1 : 0;
    }

    private bool CapsuleCast(Vector2 direction, float distance)
    {
        RaycastHit2D hit = Physics2D.CapsuleCast(
            capsule.bounds.center,
            capsule.bounds.size,
            capsule.direction,
            0f,
            direction,
            distance,
            groundLayer
        );

        return hit.collider != null;
    }

    private void TryJump()
    {
        if (jumpBufferTimer <= 0f)
        {
            return;
        }

        if (!grounded && wallSide != 0)
        {
            rb.linearVelocity = new Vector2(
                -wallSide * wallJumpHorizontalVelocity,
                wallJumpVerticalVelocity
            );

            wallJumpControlLockTimer = wallJumpControlLockTime;
            coyoteTimer = 0f;
            jumpBufferTimer = 0f;
            return;
        }

        if (coyoteTimer > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
            coyoteTimer = 0f;
            jumpBufferTimer = 0f;
        }
    }

    private void ApplyHorizontalMovement()
    {
        if (wallJumpControlLockTimer > 0f)
        {
            return;
        }

        float targetSpeed = moveInput.x * maxRunSpeed;
        bool hasInput = Mathf.Abs(moveInput.x) > 0.01f;

        float acceleration;
        if (grounded)
        {
            acceleration = hasInput ? groundAcceleration : groundDeceleration;
        }
        else
        {
            acceleration = hasInput ? airAcceleration : airDeceleration;
        }

        float horizontalSpeed = Mathf.MoveTowards(
            rb.linearVelocity.x,
            targetSpeed,
            acceleration * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector2(horizontalSpeed, rb.linearVelocity.y);
    }

    private void ApplyWallSlide()
    {
        if (grounded || wallSide == 0 || rb.linearVelocity.y >= 0f)
        {
            return;
        }

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed)
        );
    }

    private void ApplyBetterGravity()
    {
        if (rb.linearVelocity.y < -0.01f)
        {
            rb.gravityScale = fallGravity;
        }
        else if (rb.linearVelocity.y > 0.01f && !jumpHeld)
        {
            rb.gravityScale = shortJumpGravity;
        }
        else
        {
            rb.gravityScale = normalGravity;
        }
    }

    private void ClampFallSpeed()
    {
        if (rb.linearVelocity.y < -maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                -maxFallSpeed
            );
        }
    }

    private void UpdateSpriteDirection()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        if (moveInput.x > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput.x < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        CapsuleCollider2D playerCapsule = GetComponent<CapsuleCollider2D>();
        if (playerCapsule == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Vector3 center = playerCapsule.bounds.center;
        Vector3 halfSize = playerCapsule.bounds.extents;

        Gizmos.DrawLine(
            center + Vector3.down * halfSize.y,
            center + Vector3.down * (halfSize.y + groundCheckDistance)
        );
        Gizmos.DrawLine(
            center + Vector3.left * halfSize.x,
            center + Vector3.left * (halfSize.x + wallCheckDistance)
        );
        Gizmos.DrawLine(
            center + Vector3.right * halfSize.x,
            center + Vector3.right * (halfSize.x + wallCheckDistance)
        );
    }
}
