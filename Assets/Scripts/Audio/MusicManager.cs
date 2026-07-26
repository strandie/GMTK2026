using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip introClip;
    public AudioClip loopClip;

    [Header("Audio Sources")]
    public AudioSource introSource;
    public AudioSource loopSource;

    void Start()
    {
        // Enforce loop behavior strictly through the script
        introSource.loop = false;
        loopSource.loop = true;

        PlayIntroAndLoop();
    }

    void PlayIntroAndLoop()
    {
        // 1. Get the current precise time of the audio system
        double startTime = AudioSettings.dspTime + 0.2; // 0.2s buffer to prevent stutter

        // 2. Calculate when the loop should exactly begin
        double durationOfIntro = (double)introClip.samples / introClip.frequency;
        double loopStartTime = startTime + durationOfIntro;

        // 3. Assign clips and schedule them
        introSource.clip = introClip;
        introSource.PlayScheduled(startTime);

        loopSource.clip = loopClip;
        loopSource.PlayScheduled(loopStartTime);
    }

    public void SetVolume(float val)
    {
        introSource.volume = val;
        loopSource.volume = val;
    }
}
