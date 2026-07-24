using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Ground")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;


    [Header("Jump")]
    [SerializeField] private float coyoteTime = 0.15f;
    [SerializeField] private float jumpBufferTime = 0.1f;


    [Header("Movement")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;


    [Header("Dash Reference")]
    [SerializeField] private FlickDash flickDash;



    private float coyoteTimer;
    private float jumpBufferTimer;


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
        RaycastHit2D hit =
            Physics2D.Raycast(
                groundCheck.position,
                Vector2.down,
                groundCheckDistance,
                groundLayer
            );


        return hit.collider != null;
    }



    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.DrawRay(
                groundCheck.position,
                Vector2.down *
                groundCheckDistance
            );
        }
    }



    public void OnMovement(
        InputAction.CallbackContext context
    )
    {
        moveInput =
            context.ReadValue<Vector2>();
    }



    public void OnJump(
        InputAction.CallbackContext context
    )
    {
        if (context.performed)
        {
            jumpBufferTimer =
                jumpBufferTime;
        }


        if (
            context.performed &&
            coyoteTimer > 0
        )
        {
            rb.linearVelocity =
                new Vector2(
                    rb.linearVelocity.x,
                    jumpForce
                );


            coyoteTimer = 0;
        }
    }




    private void FixedUpdate()
    {
        bool grounded =
            IsGrounded();



        // Coyote time
        if (grounded)
        {
            coyoteTimer =
                coyoteTime;
        }
        else
        {
            coyoteTimer -=
                Time.fixedDeltaTime;
        }



        // Jump buffer
        if (jumpBufferTimer > 0)
        {
            jumpBufferTimer -=
                Time.fixedDeltaTime;
        }



        if (
            coyoteTimer > 0 &&
            jumpBufferTimer > 0
        )
        {
            rb.linearVelocity =
                new Vector2(
                    rb.linearVelocity.x,
                    jumpForce
                );


            coyoteTimer = 0;
            jumpBufferTimer = 0;
        }




        // Normal movement ONLY when not dashing
        if (
            flickDash == null ||
            !flickDash.IsDashing
        )
        {
            rb.linearVelocity =
                new Vector2(
                    moveInput.x *
                    moveSpeed,

                    rb.linearVelocity.y
                );
        }



        // Sprite flip
        if (moveInput.x > 0.1f)
            sr.flipX = false;


        if (moveInput.x < -0.1f)
            sr.flipX = true;
    }
}