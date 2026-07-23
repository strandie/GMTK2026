using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float skinWidth = 0.05f;

    private CapsuleCollider2D playerCollider;

    private Rigidbody2D rb;

    public float dashDistance = 5f;
    public float dashSpeed = 20f;

    private Vector2 dashTarget;
    private bool isDashing;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
    }

    private float GetDashDistance(Vector2 direction, float distance)
    {
        RaycastHit2D hit = Physics2D.CapsuleCast(
            rb.position,
            playerCollider.size,
            playerCollider.direction,
            0f,
            direction,
            distance,
            groundLayer
        );

        if (hit.collider != null)
        {
            // Stop slightly before the wall
            return Mathf.Max(hit.distance - skinWidth, 0f);
        }

        return distance;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isDashing)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 direction = (mousePosition - rb.position).normalized;

            float distance = Vector2.Distance(rb.position, mousePosition);

            // Limit dash distance
            distance = Mathf.Min(distance, dashDistance);

            // Check for walls before setting target
            distance = GetDashDistance(direction, distance);

            dashTarget = rb.position + direction * distance;

            rb.linearVelocity = Vector2.zero;

            isDashing = true;
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.MovePosition(
                Vector2.MoveTowards(
                    rb.position,
                    dashTarget,
                    dashSpeed * Time.fixedDeltaTime
                )
            );

            if (Vector2.Distance(rb.position, dashTarget) < 0.01f)
            {
                rb.position = dashTarget;
                isDashing = false;
            }
        }
    }
}
