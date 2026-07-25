using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerAnimatorUpdater : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public Gradient DashSpeedHueShift;
    public float MaxHueSpeedValue = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Grounded", playerMovement.IsGrounded());
        animator.SetBool("IsMoving", playerMovement.GetInputVector().sqrMagnitude > 0.1f);
        animator.SetBool("IsDashing", playerMovement.IsDashing());
        animator.SetBool("WallSlide", playerMovement.whichWallSliding() != 0);
        animator.SetFloat("MovementAngle", Mathf.Rad2Deg * Mathf.Atan2(rb.linearVelocityY, Mathf.Abs(rb.linearVelocityX)));

        if(playerMovement.IsDashing())
        {
            spriteRenderer.color = DashSpeedHueShift.Evaluate(rb.linearVelocity.sqrMagnitude / MaxHueSpeedValue);
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }
}
