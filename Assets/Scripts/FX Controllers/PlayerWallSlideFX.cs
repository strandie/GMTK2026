using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerWallSlideFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftParticleSystem;
    [SerializeField] private ParticleSystem rightParticleSystem;

    private PlayerMovement playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        var leftEmission = leftParticleSystem.emission;
        var rightEmission = rightParticleSystem.emission;
        switch(playerMovement.whichWallSliding())
        {
            case -1:
                leftEmission.enabled = true;
                rightEmission.enabled = false;
                break;
            case 1:
                leftEmission.enabled = false;
                rightEmission.enabled = true;
                break;
            case 0:
                leftEmission.enabled = false;
                rightEmission.enabled = false;
                break;
        }
    }
}
