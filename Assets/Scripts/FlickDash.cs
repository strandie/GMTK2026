using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class FlickDash : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;

    private Vector2 startMouseWorld;
    private Vector2 currentMouseWorld;

    private Vector2 flickDirection;

    private float flickDistance;
    private float flickTime;

    private bool flicking;


    public bool IsDashing { get; private set; }


    [Header("Dash Duration")]
    public float dashDuration = 0.25f;

    private float dashTimer;


    [Header("Flick Settings")]
    public float velocityMultiplier = 5f;
    public float maxVelocity = 25f;
    public float dashCostScale = 0.5f;
    public float flatDashCost = 1f;


    [Header("Distance Limits")]
    public float maxFlickDistance = 5f;
    public float minimumFlickDistance = 0.2f;


    [Header("Debug")]
    public DashVisualizer visualizer;
    public float visualizerScale = 0.5f; // Tune to approximately match distance travelled



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }



    private Vector2 GetMouseWorld()
    {
        Vector3 screenPos = Input.mousePosition;

        screenPos.z =
            -Camera.main.transform.position.z;

        return Camera.main.ScreenToWorldPoint(screenPos);
    }



    private void Update()
    {
        if(playerMovement.IsFrozen()) return;

        Vector2 mouseWorld = GetMouseWorld();



        // Start flick
        if (Input.GetMouseButtonDown(0))
        {
            flicking = true;

            startMouseWorld = mouseWorld;
            currentMouseWorld = mouseWorld;

            flickDirection = Vector2.zero;

            flickDistance = 0;
            flickTime = 0;
        }



        // Record flick
        if (flicking && Input.GetMouseButton(0))
        {
            currentMouseWorld = mouseWorld;


            flickDistance =
                Vector2.Distance(
                    startMouseWorld,
                    currentMouseWorld
                );


            flickDistance =
                Mathf.Clamp(
                    flickDistance,
                    0,
                    maxFlickDistance
                );


            flickDirection =
                (
                    currentMouseWorld -
                    startMouseWorld
                ).normalized;


            flickTime += Time.deltaTime;



            visualizer?.Draw(
                flickDirection,
                CalculateFlickSpeed() * visualizerScale
            );
        }




        // Release dash
        if (flicking && Input.GetMouseButtonUp(0))
        {
            flicking = false;


            if (
                flickDistance >= minimumFlickDistance &&
                flickTime > 0
            )
            {
                float speed = CalculateFlickSpeed();


                Vector2 dashVelocity =
                    flickDirection * speed;


                /*Debug.Log(
                    "Dash Velocity: "
                    + dashVelocity
                );*/


                rb.linearVelocity =
                    dashVelocity;


                IsDashing = true;

                dashTimer = 0;

                // Trigger timer cost
                
                TimerManager.Instance.SubtractFromTimer(flatDashCost + speed * 0.05f * dashCostScale);
            }


            visualizer?.Hide();
        }



        // Cancel
        if (Input.GetMouseButtonDown(1))
        {
            flicking = false;

            visualizer?.Hide();
        }
    }

    private float CalculateFlickSpeed()
    {
        float speed =
            (flickDistance / flickTime)
            *
            velocityMultiplier;


        speed =
            Mathf.Clamp(
                speed,
                0,
                maxVelocity
            );
        return speed;
    }



    private void FixedUpdate()
    {
        if (IsDashing)
        {
            dashTimer += Time.fixedDeltaTime;


            if (dashTimer >= dashDuration)
            {
                IsDashing = false;
            }
        }
    }
}