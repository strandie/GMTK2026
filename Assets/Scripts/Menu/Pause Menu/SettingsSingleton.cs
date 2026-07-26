using UnityEngine;

public class SettingsSingleton : MonoBehaviour
{
    public static SettingsSingleton Instance;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private float musicVolume = 0.5f;
    public float GetMusicVolume() {return musicVolume;}
    public void SetMusicVolume(float newVol)
    {
        musicVolume = newVol;
        AudioManager.Instance.SetMusicVolume(musicVolume);
    }
    private float sfxVolume = 0.5f;
    public float GetSFXVolume() {return sfxVolume;}
    public void SetSFXVolume(float newVol)
    {
        sfxVolume = newVol;
        AudioManager.Instance.SetSFXVolume(sfxVolume);
    }
    private bool mouseHidden = false;
    public bool GetMouseHidden() {return mouseHidden;}
    public void SetMouseHidden(bool val)
    {
        mouseHidden = val;
        PlayerMovement.Instance.GetComponent<FlickDash>().mouseHidden = val;
    }
    private bool clicklessDash = false;
    public bool GetClicklessDashEnabled() {return clicklessDash;}
    public void SetClicklessDash(bool val)
    {
        clicklessDash = val;
        PlayerMovement.Instance.GetComponent<FlickDash>().motionTriggeredMode = val;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
