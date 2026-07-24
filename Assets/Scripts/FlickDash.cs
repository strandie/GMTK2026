using UnityEngine;

public class FlickDash : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector2 lastMouse;

    [Header("Flick Settings")]
    public float multiplier = 0.01f;
    public float maxDistance = 10f;
    public float dashSpeed = 20f;

    [Header("Debug")]
    public DashVisualizer visualizer;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        lastMouse =
            Input.mousePosition;
    }


    private void Update()
    {
        Vector2 mouse =
            Input.mousePosition;


        Vector2 velocity =
            mouse - lastMouse;


        lastMouse = mouse;


        // Hold right click
        if (Input.GetMouseButtonDown(1))
        {
            float distance =
                Mathf.Clamp(
                    velocity.magnitude * multiplier,
                    0,
                    maxDistance
                );


            Vector2 dir =
                velocity.normalized;


            if (dir != Vector2.zero)
            {
                rb.MovePosition(
                    rb.position +
                    dir * distance
                );


                visualizer?.Draw(
                    dir,
                    distance
                );
            }
        }
    }
}