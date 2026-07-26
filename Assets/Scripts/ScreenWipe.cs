using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScreenWipe : MonoBehaviour
{
    public float speed = 20f;
    public string nextScene;

    public bool startCovered = false;

    Vector3 coveredPosition;
    Vector3 leftPosition;
    Vector3 rightPosition;

    void Start()
    {
        coveredPosition = Vector3.zero;

        leftPosition = new Vector3(-50, 0, -5);
        rightPosition = new Vector3(50, 0, -5);


        if (startCovered)
        {
            // Already covering the screen
            transform.position = coveredPosition;

            StartCoroutine(Open());
        }
        else
        {
            // Waiting to close
            transform.position = leftPosition;
        }
    }

    public void Close()
    {
        StartCoroutine(CloseAndLoad());
    }

    IEnumerator CloseAndLoad()
    {
        while (Vector3.Distance(transform.position, coveredPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                coveredPosition,
                speed * Time.deltaTime
            );

            yield return null;
        }


        SceneManager.LoadScene(nextScene);
    }

    IEnumerator Open()
    {
        while (Vector3.Distance(transform.position, rightPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                rightPosition,
                speed * Time.deltaTime
            );

            yield return null;
        }
    }
}