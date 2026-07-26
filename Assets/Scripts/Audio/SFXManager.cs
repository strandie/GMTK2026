using UnityEngine;
using System.Collections.Generic;

public class SFXManager : MonoBehaviour
{

    public AnimationCurve dashPitchScaling;

    public AudioClip dashSound;
    private float globalVolume = 1f;
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
        globalVolume = val;
    }
    public void PlayDash(float relativeSpeed)
    {
        //relativeSpeed is a float from 0-1
        float pitch = dashPitchScaling.Evaluate(relativeSpeed);
        PlayClip(dashSound, 1f, pitch);
    }

    [SerializeField] private AudioSource sourcePrefab; // simple prefab w/ AudioSource, no clip
    [SerializeField] private int poolSize = 8;
    [SerializeField] private Vector2 pitchRange = new Vector2(0.9f, 1.1f);

    private readonly List<AudioSource> pool = new List<AudioSource>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource src = Instantiate(sourcePrefab, transform);
            src.playOnAwake = false;
            pool.Add(src);
        }
    }

    public void PlayClip(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        //Debug.Log("Playing clip at " + (volume * globalVolume) + " volume");
        AudioSource src = GetAvailableSource();
        src.pitch = pitch;
        src.clip = clip;
        src.volume = volume * globalVolume;
        src.Play();
    }

    private AudioSource GetAvailableSource()
    {
        foreach (var src in pool)
        {
            if (!src.isPlaying) return src;
        }

        // Pool exhausted — grow it (or steal the oldest one, your call)
        AudioSource extra = Instantiate(sourcePrefab, transform);
        extra.playOnAwake = false;
        pool.Add(extra);
        return extra;
    }
}
