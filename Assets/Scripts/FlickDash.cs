using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class FlickDash : MonoBehaviour
{
    /*
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
 
    // Accumulated virtual "world" offset since mouse-down.
    // Replaces startMouseWorld/currentMouseWorld, which relied on
    // Input.mousePosition — that stops being meaningful once the
    // cursor is locked (CursorLockMode.Locked).
    private Vector2 accumulatedDelta;
 
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
 
    [Header("Mouse Sensitivity")]
    [Tooltip("Converts raw mouse delta (Input.GetAxis) into the same 'world units' " +
             "the old screen->world tracking produced. Tune this to match the feel " +
             "you had before locking the cursor.")]
    public float mouseSensitivity = 0.1f;
 
 
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
 
 
 
    // Raw mouse movement delta since last frame, scaled to "world units".
    // Works identically whether the cursor is free or locked, because
    // Input.GetAxis("Mouse X"/"Mouse Y") reads device delta, not
    // absolute screen position.
    private Vector2 GetMouseDeltaWorld()
    {
        float dx = Input.GetAxis("Mouse X") * mouseSensitivity;
        float dy = Input.GetAxis("Mouse Y") * mouseSensitivity;
 
        return new Vector2(dx, dy);
    }
 
 
 
    private void Update()
    {
        if(playerMovement.IsFrozen()) return;
 
 
 
        // Start flick
        if (Input.GetMouseButtonDown(0))
        {
            flicking = true;
 
            accumulatedDelta = Vector2.zero;
 
            flickDirection = Vector2.zero;
 
            flickDistance = 0;
            flickTime = 0;
        }
 
 
 
        // Record flick
        if (flicking && Input.GetMouseButton(0))
        {
            accumulatedDelta += GetMouseDeltaWorld();
 
 
            flickDistance =
                Mathf.Clamp(
                    accumulatedDelta.magnitude,
                    0,
                    maxFlickDistance
                );
 
 
            flickDirection =
                accumulatedDelta.sqrMagnitude > 0f
                    ? accumulatedDelta.normalized
                    : Vector2.zero;
 
 
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
    }*/

    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
 
    // Accumulated virtual "world" offset since mouse-down.
    // Replaces startMouseWorld/currentMouseWorld, which relied on
    // Input.mousePosition — that stops being meaningful once the
    // cursor is locked (CursorLockMode.Locked).
    private Vector2 accumulatedDelta;
 
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
 
    [Header("Mouse Sensitivity")]
    [Tooltip("Converts raw mouse delta (Input.GetAxis) into the same 'world units' " +
             "the old screen->world tracking produced. Tune this to match the feel " +
             "you had before locking the cursor.")]
    public float mouseSensitivity = 0.1f;
 
 
    [Header("Distance Limits")]
    public float maxFlickDistance = 5f;
    public float minimumFlickDistance = 0.2f;
 
 
    [Header("Motion Trigger Mode (trackpad-friendly)")]
    [Tooltip("When enabled, flicks are triggered by mouse/trackpad motion instead of " +
             "click-drag-release. Useful for trackpad users who find click-drag awkward.")]
    public bool motionTriggeredMode = false;
 
    [Tooltip("Speed (world units/sec) that must be exceeded to START a motion flick. " +
             "Filters out slow accidental drift.")]
    public float motionStartSpeedThreshold = 3f;
 
    [Tooltip("Once flicking, speed must drop below this (world units/sec) to be " +
             "considered 'stopped'.")]
    public float motionStopSpeedThreshold = 1f;
 
    [Tooltip("How long speed must stay below the stop threshold before the flick " +
             "is released and the dash fires. Prevents brief pauses/mis-taps from " +
             "cutting a flick short.")]
    public float motionStopDelay = 0.08f;
 
    [Tooltip("Minimum time after a dash fires before another motion flick can start. " +
             "Prevents residual hand motion from immediately re-triggering.")]
    public float motionRetriggerCooldown = 0.15f;
 
    private float lowSpeedTimer;
    private float motionCooldownTimer;
 
 
    [Header("Debug")]
    public DashVisualizer visualizer;
    public float visualizerScale = 0.5f; // Tune to approximately match distance travelled
 
 
 
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }
 
 
 
    // Raw mouse movement delta since last frame, scaled to "world units".
    // Works identically whether the cursor is free or locked, because
    // Input.GetAxis("Mouse X"/"Mouse Y") reads device delta, not
    // absolute screen position.
    private Vector2 GetMouseDeltaWorld()
    {
        float dx = Input.GetAxis("Mouse X") * mouseSensitivity;
        float dy = Input.GetAxis("Mouse Y") * mouseSensitivity;
 
        return new Vector2(dx, dy);
    }
 
 
 
    private void Update()
    {
        if(playerMovement.IsFrozen()) return;
 
        if (motionTriggeredMode)
        {
            UpdateMotionTriggeredMode();
        }
        else
        {
            UpdateClickDragMode();
        }
    }
 
 
 
    private void UpdateClickDragMode()
    {
        // Start flick
        if (Input.GetMouseButtonDown(0))
        {
            BeginFlick();
        }
 
 
 
        // Record flick
        if (flicking && Input.GetMouseButton(0))
        {
            RecordFlick();
        }
 
 
 
 
        // Release dash
        if (flicking && Input.GetMouseButtonUp(0))
        {
            ReleaseFlick();
        }
 
 
 
        // Cancel
        if (Input.GetMouseButtonDown(1))
        {
            CancelFlick();
        }
    }
 
 
 
    private void UpdateMotionTriggeredMode()
    {
        if (motionCooldownTimer > 0f)
        {
            motionCooldownTimer -= Time.deltaTime;
        }
 
        Vector2 delta = GetMouseDeltaWorld();
 
        // Instantaneous speed this frame, in world units/sec.
        float instSpeed =
            Time.deltaTime > 0f
                ? delta.magnitude / Time.deltaTime
                : 0f;
 
 
        if (!flicking)
        {
            // Not currently flicking: watch for motion crossing the
            // start threshold to kick things off. Cooldown blocks
            // immediate re-trigger right after a dash.
            if (
                motionCooldownTimer <= 0f &&
                instSpeed >= motionStartSpeedThreshold
            )
            {
                BeginFlick();
 
                // Count this frame's motion immediately so a fast
                // flick isn't penalized for the frame it was detected on.
                RecordFlick();
            }
 
            return;
        }
 
 
        // Currently flicking: keep accumulating.
        RecordFlick();
 
 
        // Track how long speed has stayed below the stop threshold.
        if (instSpeed < motionStopSpeedThreshold)
        {
            lowSpeedTimer += Time.deltaTime;
        }
        else
        {
            lowSpeedTimer = 0f;
        }
 
 
        if (lowSpeedTimer >= motionStopDelay)
        {
            ReleaseFlick();
 
            motionCooldownTimer = motionRetriggerCooldown;
        }
    }
 
 
 
    private void BeginFlick()
    {
        flicking = true;
 
        accumulatedDelta = Vector2.zero;
 
        flickDirection = Vector2.zero;
 
        flickDistance = 0;
        flickTime = 0;
 
        lowSpeedTimer = 0f;
    }
 
 
 
    private void RecordFlick()
    {
        accumulatedDelta += GetMouseDeltaWorld();
 
 
        flickDistance =
            Mathf.Clamp(
                accumulatedDelta.magnitude,
                0,
                maxFlickDistance
            );
 
 
        flickDirection =
            accumulatedDelta.sqrMagnitude > 0f
                ? accumulatedDelta.normalized
                : Vector2.zero;
 
 
        flickTime += Time.deltaTime;
 
 
 
        visualizer?.Draw(
            flickDirection,
            CalculateFlickSpeed() * visualizerScale
        );
    }
 
 
 
    private void ReleaseFlick()
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
 
            rb.linearVelocity =
                dashVelocity;
 
 
            IsDashing = true;
 
            dashTimer = 0;
 
            // Trigger timer cost
 
            TimerManager.Instance.SubtractFromTimer(flatDashCost + speed * 0.05f * dashCostScale);
        }
 
 
        visualizer?.Hide();
    }
 
 
 
    private void CancelFlick()
    {
        flicking = false;
 
        lowSpeedTimer = 0f;
 
        visualizer?.Hide();
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