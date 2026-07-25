using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PlayerDeathParticles : MonoBehaviour
{
    private ParticleSystem ps;
    public bool Triggered = false;
    private bool playedAnimation = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!playedAnimation && Triggered)
        {
            ps.Play();
            playedAnimation = true;
        }
    }

    public void Reset()
    {
        ps.Stop();
        playedAnimation = false;
    }
}
