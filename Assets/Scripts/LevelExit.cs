using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelExit : MonoBehaviour
{
    public static LevelExit Instance;
    public string nextSceneName;
    public float shrinkTime = 1.5f;

    bool activated = false;

    public void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;
            StartCoroutine(EnterPortal(other.gameObject));
        }
    }

    public void SkipLevel()
    {
        Debug.Log("Loading next scene: " + nextSceneName);

        // Start screen wipe
        ScreenWipe wipe = FindAnyObjectByType<ScreenWipe>();

        if (wipe != null)
        {
            wipe.Close();
        }
    }

    IEnumerator EnterPortal(GameObject player)
    {
        // Stop player movement
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
            movement.enabled = false;

        // Disable flick dash
        FlickDash dash = player.GetComponent<FlickDash>();
        if (dash != null)
            dash.enabled = false;

        // Stop physics
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        Vector3 startScale = player.transform.localScale;
        float timer = 0f;
        float spinSpeed = 720f;

        while (timer < shrinkTime)
        {
            timer += Time.deltaTime;
            float t = timer / shrinkTime;

            player.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

            player.transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);

            yield return null;
        }

        Debug.Log("Loading next scene: " + nextSceneName);

        // Start screen wipe
        ScreenWipe wipe = FindAnyObjectByType<ScreenWipe>();

        if (wipe != null)
        {
            wipe.Close();
        }
    }
}
