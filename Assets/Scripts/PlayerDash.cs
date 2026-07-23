using UnityEngine;

public class PlayerDash : MonoBehaviour
{

    public float dashDistance = 5f;
    public float dashSpeed = 20f;

    private Rigidbody2D rb;
    private Vector2 dashTarget;
    private bool isDashing;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isDashing)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 direction = (mousePosition - rb.position).normalized;

            float distance = Vector2.Distance(rb.position, mousePosition);

            // Limit dash distance
            distance = Mathf.Min(distance, dashDistance);

            dashTarget = rb.position + direction * distance;

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
