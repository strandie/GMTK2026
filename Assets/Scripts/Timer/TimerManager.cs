using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance {get; private set;}
    [SerializeField] private TimerTextSpawner timerTextSpawner;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public TextMeshProUGUI timerText;
    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        timerText.text = timer.ToString("00.00");
    }

    public void SetTimer(float val)
    {
        timer = val;
    }
    public void AddToTimer(float val)
    {
        timer += val;
        timerTextSpawner.SpawnTextNumber(val);
    }
    public void SubtractFromTimer(float val)
    {
        timer -= val;
        timerTextSpawner.SpawnTextNumber(-val);
    }
}
