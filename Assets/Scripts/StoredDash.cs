using UnityEngine;

public class StoredDash : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D capsule;
    private DashVisualizer visualizer;

    private Vector2 direction;
    private Vector2 dashTarget;

    private float charge;
    private bool charging;
    private bool dashing;

    public float maxDistance = 5f;
    public float chargeSpeed = 8f;
    public float dashSpeed = 15f;

    public LayerMask groundLayer;
    public float skinWidth = 0.1f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
        visualizer = GetComponentInChildren<DashVisualizer>();
    }


    private void Update()
    {
        Vector2 mouse =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);


        if (Input.GetMouseButtonDown(1))
        {
            charging = true;
            charge = 0;
        }


        if (charging && Input.GetMouseButton(1))
        {
            direction =
                (mouse - rb.position).normalized;


            charge += chargeSpeed * Time.deltaTime;


            float maxAvailableDistance =
                GetSafeDistance(direction, maxDistance);


            charge =
                Mathf.Clamp(
                    charge,
                    0,
                    maxAvailableDistance
                );


            if (visualizer != null)
            {
                visualizer.Draw(direction, charge);
            }
        }


        if (charging && Input.GetMouseButtonUp(1))
        {
            charging = false;


            float safeDistance =
                GetSafeDistance(direction, charge);


            dashTarget =
                rb.position +
                direction * safeDistance;


            dashing = true;


            visualizer?.Hide();
        }


        if (Input.GetMouseButtonDown(0))
        {
            charging = false;
            charge = 0;

            visualizer?.Hide();
        }
    }


    private float GetSafeDistance(Vector2 dir, float distance)
    {
        RaycastHit2D hit = Physics2D.CapsuleCast(
            capsule.bounds.center,
            capsule.bounds.size,
            capsule.direction,
            0f,
            dir,
            distance,
            groundLayer
        );


        if (hit.collider != null)
        {
            return Mathf.Max(
                hit.distance - skinWidth,
                0f
            );
        }


        return distance;
    }


    private void FixedUpdate()
    {
        if (!dashing)
            return;


        float step =
            dashSpeed * Time.fixedDeltaTime;


        // Check obstacle every physics frame
        RaycastHit2D hit = Physics2D.CapsuleCast(
            capsule.bounds.center,
            capsule.bounds.size,
            capsule.direction,
            0f,
            direction,
            step + skinWidth,
            groundLayer
        );


        if (hit.collider != null)
        {
            // Stop right before wall
            float stopDistance =
                Mathf.Max(
                    hit.distance - skinWidth,
                    0f
                );


            rb.MovePosition(
                rb.position +
                direction * stopDistance
            );


            dashing = false;
            return;
        }


        rb.MovePosition(
            Vector2.MoveTowards(
                rb.position,
                dashTarget,
                step
            )
        );


        if (Vector2.Distance(
            rb.position,
            dashTarget
        ) < 0.02f)
        {
            rb.position = dashTarget;
            dashing = false;
        }
    }
}