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

    private float fixedDeltaTime;
    private Coroutine timeChangeCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
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
        // 1. Ease down to slow motion
        while (Mathf.Abs(Time.timeScale - targetTimeScale) > 0.05f)
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, targetTimeScale, easeInSpeed * Time.unscaledDeltaTime);
            Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
            yield return null;
        }
        
        Time.timeScale = targetTimeScale;
        Time.fixedDeltaTime = fixedDeltaTime * targetTimeScale;

        // 2. Stay slowed down for target duration using real time
        yield return new WaitForSecondsRealtime(duration);

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
