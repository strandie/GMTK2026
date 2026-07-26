using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something touched the spike: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit spike!");

            PlayerMovement player = other.GetComponent<PlayerMovement>();

            if (player != null)
            {
                player.BeginDeath();
            }
            else
            {
                Debug.LogError("No PlayerMovement found!");
            }
        }
    }
}