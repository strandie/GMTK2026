using UnityEngine;
using TMPro;

public class MoveTimerText : MonoBehaviour
{

    public Vector3 moveDirection = Vector3.up;
    public float lifeSpan = 5f;
    public Color startColor = Color.white;
    public Color endColor = Color.white;
    public float startScale = 1f;
    public float endScale = 1f;
    public string textValue = "0.00";

    private float timeAlive = 0f;
    private TextMeshProUGUI text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeAlive = 0f;
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = textValue;
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        float t = timeAlive / lifeSpan;

        transform.position += moveDirection;
        text.color = Color.Lerp(startColor, endColor, t);
        transform.localScale = Vector3.one * Mathf.Lerp(startScale, endScale, t);

        if (timeAlive > lifeSpan)
        {
            Destroy(gameObject);
        }
    }
}
