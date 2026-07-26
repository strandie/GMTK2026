using UnityEngine;

public class TimerTextSpawner : MonoBehaviour
{
    public GameObject TimerSpawnedText;
    public float spawnAreaWidth = 5f;
    public float spawnAreaHeight = 1f;
    public float textMoveSpeed = 0.2f;
    public float textLifeSpan = 1f;
    public float scale = 1f;

    [Header("Randomized settings")]
    public float minAngleRotate = -10f;
    public float maxAngleRotate = 10f;

    public Gradient positiveColorGradient;
    public Gradient negativeColorGradient;
    public AnimationCurve positiveSizeGradient;
    public AnimationCurve negativeSizeGradient;

    public float maxPositiveVal = 5f;
    public float maxNegativeVal = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnTextNumber(float value)
    {
        string text = Mathf.Abs(value).ToString("#0.00");

        Vector3 spawnLocation = new Vector3(
            Random.Range(-spawnAreaWidth, spawnAreaWidth),
            Random.Range(-spawnAreaHeight, spawnAreaHeight),
            0f
        ) + transform.position;

        GameObject newTextObj = Instantiate(TimerSpawnedText);
        MoveTimerText textSettings = newTextObj.GetComponent<MoveTimerText>();
        newTextObj.transform.position = spawnLocation;

        float zRot = Random.Range(minAngleRotate, maxAngleRotate);
        newTextObj.transform.eulerAngles = new Vector3(0f, 0f, zRot);

        Color textColor = (value > 0f ? positiveColorGradient : negativeColorGradient).Evaluate(Mathf.Abs(value) / (value > 0f ? maxPositiveVal : maxNegativeVal));
        float textSize = (value > 0f ? positiveSizeGradient : negativeSizeGradient).Evaluate(Mathf.Abs(value) / (value > 0f ? maxPositiveVal : maxNegativeVal));
        textSettings.startColor = textColor;
        Color endColor = textColor; endColor.a = 0f;
        textSettings.endColor = endColor;
        textSettings.startScale = textSize * scale;
        textSettings.endScale = textSize * 0.8f * scale;
        textSettings.lifeSpan = textLifeSpan;
        textSettings.textValue = text;

        textSettings.moveDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * zRot + Mathf.PI*0.5f), Mathf.Sin(Mathf.Deg2Rad * zRot + Mathf.PI*0.5f), 0f) * textMoveSpeed;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaWidth, spawnAreaHeight, 0.01f));
    }
}
