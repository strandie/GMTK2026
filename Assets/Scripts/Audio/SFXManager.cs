using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AnimationCurve dashPitchScaling;

    public AudioSource dash;
    public AudioClip dashSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetVolume(float val)
    {
        dash.volume = val;
    }
    public void PlayDash(float relativeSpeed)
    {
        //relativeSpeed is a float from 0-1
        dash.pitch = dashPitchScaling.Evaluate(relativeSpeed);
        dash.PlayOneShot(dashSound);
    }
}
