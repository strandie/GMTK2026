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

    public float volume = 0.5f;
    public float GetVolume() {return volume;}
    public void SetVolume(float newVol)
    {
        volume = newVol;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
