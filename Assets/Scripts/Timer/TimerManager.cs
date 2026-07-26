using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance {get; private set;}
    

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public TextMeshProUGUI timerText;
    private float timer;
    public bool StopTimer;
    [SerializeField] private TimerTextSpawner timerTextSpawner;
    [SerializeField] private PlayerMovement player;

    public float timerStartValue = 20f;

    [Header("Color Shift")]
    public float maxTimerValue = 60f;
    public Gradient timerColorOverTime;


    [Header("Rotation Jitter")]
    public float rotateJitterMaxAngle = 10f;
    public float rotateReturnSpeed = 0.1f;
    public float rotateToSpeed = 1f;
    private float targetRotation = 0f;
    private float currRotation = 0f;


    [Header("Size Jitter")]
    public float sizeJitterStrength = 0.5f;
    public float sizeJitterDecayRate = 0.1f;
    public float sizeJitterDecayOffset = 0.5f;
    public float sizeGrowSpeed = 1f;
    public float sizeShrinkSpeed = 0.1f;
    private float jitterSizeIncreaseFunction(float inputSize)
    {
        inputSize = Mathf.Max(1f, inputSize); // safeguard
        // Basically don't want the jitter to increase indefinitely, so using a decaying function
        float output = sizeJitterStrength / (sizeJitterDecayRate * inputSize + sizeJitterDecayOffset) + 1f;
        return output;
    }
    private float originalScale;
    private float sizeScale = 1f;
    private float targetSizeScale = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalScale = timerText.transform.localScale.x;
        timer = timerStartValue;
    }

    // Update is called once per frame
    void Update()
    {
        if(!StopTimer) timer -= Time.deltaTime;

        if(timer <= 0f && !StopTimer)
        {
            timer = 0f;
            player.BeginDeath();
            StopTimer = true;
        }

        timerText.text = "<mspace=0.35em>" + timer.ToString("00.00") + "</mspace>";
        timerText.color = timerColorOverTime.Evaluate(timer / maxTimerValue);

        sizeScale = Mathf.MoveTowards(sizeScale, targetSizeScale,
                        targetSizeScale > sizeScale ? sizeGrowSpeed : sizeShrinkSpeed);
        if (sizeScale >= targetSizeScale) targetSizeScale = 1f; // Once reaching target, go to baseline
        timerText.transform.localScale = Vector3.one * sizeScale * originalScale;

        bool rotatingToBaseline = targetRotation == 0f;
        currRotation = Mathf.MoveTowards(currRotation, targetRotation,
                        rotatingToBaseline ? rotateReturnSpeed : rotateToSpeed);
        if(!rotatingToBaseline && Mathf.Abs(currRotation) >= Mathf.Abs(targetRotation))
        {
            // finished rotation
            targetRotation = 0f;
        }
        timerText.transform.rotation = Quaternion.Euler(Vector3.forward * currRotation);
    }

    public void ResetTimer()
    {
        timer = timerStartValue;
        StopTimer = false;
    }

    public void SetTimer(float val)
    {
        timer = val;
        StopTimer = false;
    }
    public void AddToTimer(float val)
    {
        timer += val;
        timerTextSpawner.SpawnTextNumber(val);
        targetSizeScale = jitterSizeIncreaseFunction(sizeScale);
        targetRotation = Random.Range(-rotateJitterMaxAngle, rotateJitterMaxAngle);
    }
    public void SubtractFromTimer(float val)
    {
        timer -= val;
        timerTextSpawner.SpawnTextNumber(-val);
        targetSizeScale = jitterSizeIncreaseFunction(sizeScale);
        targetRotation = Random.Range(-rotateJitterMaxAngle, rotateJitterMaxAngle);
    }
}
