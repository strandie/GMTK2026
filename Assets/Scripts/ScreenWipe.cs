using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScreenWipe : MonoBehaviour
{
    public float speed = 20f;
    public string nextScene;

    public bool startCovered = false;

    private Vector3 coveredPosition;
    private Vector3 leftPosition;
    private Vector3 rightPosition;

    public Camera cam;

    void Start()
    {

        coveredPosition = new Vector3(0, 0, -5);

        // Get camera size
        float screenHeight = cam.orthographicSize * 2f;
        float screenWidth = screenHeight * cam.aspect;

        // Put rectangle just outside the screen
        float offset = screenWidth + 50f;

        leftPosition = new Vector3(-offset, 0, -5);
        rightPosition = new Vector3(offset, 0, -5);

        // Make rectangle cover the whole screen
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            transform.localScale = new Vector3(50f, 50f, 1f);
        }

        if (startCovered)
        {
            // Start covering the screen, then reveal
            transform.position = coveredPosition;
            StartCoroutine(Open());
        }
        else
        {
            // Start off-screen, waiting for portal
            transform.position = leftPosition;
        }
    }

    public void Close()
    {
        StartCoroutine(CloseAndLoad());
    }

    IEnumerator CloseAndLoad()
    {
        while (Vector3.Distance(transform.position, coveredPosition) > 0.05f)
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
        while (Vector3.Distance(transform.position, rightPosition) > 0.05f)
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