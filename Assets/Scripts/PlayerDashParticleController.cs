using UnityEngine;

public class PlayerDashParticleController : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public ParticleSystem dashTrail;
    public Gradient trailColorBySpeed;
    public float maxColorSpeedVal = 15f;
    private bool wasDashing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerMovement.IsDashing())
        {
            // Align trail with travel direction
            dashTrail.transform.eulerAngles = Vector3.forward * playerMovement.GetMovementAngleDegrees();
            if(!wasDashing)
            {
                dashTrail.Play();
                // Set color based on dash strength
                var main = dashTrail.main;
                main.startColor = trailColorBySpeed.Evaluate(playerMovement.GetRBSpeed() / maxColorSpeedVal);
            }
            wasDashing = true;
        }
        else
        {
            dashTrail.Stop();
            wasDashing = false;
        }
    }

    public void BeginDash()
    {
        dashTrail.Play();
    }

    public void EndDash()
    {
        dashTrail.Stop();
    }
}
