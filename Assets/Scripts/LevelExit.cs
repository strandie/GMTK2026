using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelExit : MonoBehaviour
{
    public string nextSceneName;
    public float enterTime = 2f;

    public Transform portalCenter;
    public float walkSpeed = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(EnterPortal(other.gameObject));
        }
    }

    IEnumerator EnterPortal(GameObject player)
    {
        // Disable player movement
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
            movement.enabled = false;

        // Player walks to center of door
        /*while (Vector2.Distance(player.transform.position, portalCenter.position) > 0.1f)
        {
            player.transform.position = Vector3.MoveTowards(
                player.transform.position,
                portalCenter.position,
                walkSpeed * Time.deltaTime);

            yield return null;
        }*/

        // Wait until player enters door
        yield return new WaitForSeconds(enterTime);

        SceneManager.LoadScene(nextSceneName);
    }
}
