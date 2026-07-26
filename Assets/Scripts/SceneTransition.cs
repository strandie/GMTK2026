using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    public RectTransform panel;
    public float speed = 800f;

    private Vector2 hiddenPosition;
    private Vector2 centerPosition;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        centerPosition = Vector2.zero;
        hiddenPosition = new Vector2(-Screen.width, 0);

        panel.anchoredPosition = hiddenPosition;
    }

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    IEnumerator Transition(string sceneName)
    {
        // Slide in
        while (panel.anchoredPosition.x < 0)
        {
            panel.anchoredPosition = Vector2.MoveTowards(
                panel.anchoredPosition,
                centerPosition,
                speed * Time.deltaTime
            );

            yield return null;
        }

        panel.anchoredPosition = centerPosition;

        // Load scene
        SceneManager.LoadScene(sceneName);

        yield return new WaitForSeconds(0.5f);

        // Move panel off to the right
        Vector2 exitPosition = new Vector2(Screen.width, 0);

        while (panel.anchoredPosition.x < Screen.width)
        {
            panel.anchoredPosition = Vector2.MoveTowards(
                panel.anchoredPosition,
                exitPosition,
                speed * Time.deltaTime
            );

            yield return null;
        }
    }
}