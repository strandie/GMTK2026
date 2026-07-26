using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
    public MusicManager musicManager;
    public SFXManager sfxManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMusicVolume(float val)
    {
        musicManager.SetVolume(val);
    }
    public void SetSFXVolume(float val)
    {
        sfxManager.SetVolume(val);
    }

    public void PlayDashSFX(float relativeSpeed)
    {
        sfxManager.PlayDash(relativeSpeed);
    }
}
