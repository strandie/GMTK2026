using UnityEngine;
using System.Collections;

public class GlobalTimeManager : MonoBehaviour
{
    public static GlobalTimeManager Instance {get; private set;}

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

    private float timeScale = 1f;
    private float fixedDeltaTime;

    private float lastTimeScale = 1f;
    private float targetTimeScale = 1f;
    private Coroutine timeChangeCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        //Time.timeScale = timeScale;
        //Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
    }

    public void SetTimeScale(float newTimeScale, float duration, float easeInSpeed = 10f, float easeOutSpeed = 10f)
    {
        if(timeChangeCoroutine != null)
        {
            StopCoroutine(timeChangeCoroutine);
        }
        timeChangeCoroutine = StartCoroutine(SetTimeScaleRoutine(newTimeScale, duration, easeInSpeed, easeOutSpeed));
    }

    private IEnumerator SetTimeScaleRoutine(float targetTimeScale, float duration, float easeInSpeed, float easeOutSpeed)
    {
        Debug.Log("Changing Speed");
        // 1. Ease down to slow motion
        while (Mathf.Abs(Time.timeScale - targetTimeScale) > 0.05f)
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, targetTimeScale, easeInSpeed * Time.unscaledDeltaTime);
            Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
            yield return null;
        }
        
        Time.timeScale = targetTimeScale;
        Time.fixedDeltaTime = fixedDeltaTime * targetTimeScale;

        Debug.Log("Yield at target speed");
        // 2. Stay slowed down for target duration using real time
        yield return new WaitForSecondsRealtime(duration);

        Debug.Log("Return to normal speed");
        // 3. Ease back up to normal speed
        while (Mathf.Abs(Time.timeScale - 1f) > 0.05f)
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1.0f, easeOutSpeed * Time.unscaledDeltaTime);
            Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
            yield return null;
        }

        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = fixedDeltaTime;
    }
}
