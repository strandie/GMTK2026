using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Transform pauseContent;
    public Transform settingsContent;
    private System.Collections.Generic.List<Transform> items;
    public Image screenOverlay;
    private Color overlayColor;
    private Color transparent;
    public float menuTransitionDuration = 0.2f;

    public PlayerMovement player;
    public TimerManager timer;

    private Coroutine activeRoutine;

    private bool MenuActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        items = new System.Collections.Generic.List<Transform>();
        foreach(Transform child in pauseContent)
        {
            items.Add(child);
            child.localScale = Vector3.zero;
        }
        overlayColor = screenOverlay.color;
        transparent = new Color(0f, 0f, 0f, 0f);
        screenOverlay.color = transparent;

        pauseContent.gameObject.SetActive(false);
        settingsContent.gameObject.SetActive(false);
    }

    private void SyncSettingsWithMenu()
    {
        // Update volume
        settingsContent.GetChild(0).GetComponentInChildren<Slider>().value = SettingsSingleton.Instance.GetVolume();
        settingsContent.GetChild(1).GetComponentInChildren<Toggle>().isOn = SettingsSingleton.Instance.GetMouseHidden();
        settingsContent.GetChild(2).GetComponentInChildren<Toggle>().isOn = SettingsSingleton.Instance.GetClicklessDashEnabled();
    }

    public void RevealMenu()
    {
        MenuActive = true;
        if(activeRoutine != null) StopCoroutine(activeRoutine); activeRoutine = null;
        activeRoutine = StartCoroutine(RevealPauseMenuRoutine());
    }
    public void HideMenu()
    {
        MenuActive = false;
        if(activeRoutine != null) StopCoroutine(activeRoutine); activeRoutine = null;
        activeRoutine = StartCoroutine(HidePauseMenuRoutine());
    }
    private IEnumerator RevealPauseMenuRoutine()
    {
        pauseContent.gameObject.SetActive(true);
        timer.StopTimer = true;
        player.FreezePlayerState();

        float time = 0f;
        while(time < menuTransitionDuration)
        {
            time += Time.deltaTime;
            float t = time / menuTransitionDuration;
            foreach(Transform item in items)
            {
                item.localScale = Vector3.one * Mathf.Min(t, 1f);
            }
            screenOverlay.color = Color.Lerp(transparent, overlayColor, t);
            yield return null;
        }

        activeRoutine = null;
    }
    private IEnumerator HidePauseMenuRoutine()
    {
        float time = 0f;
        while(time < menuTransitionDuration)
        {
            time += Time.deltaTime;
            float t = time / menuTransitionDuration;

            if(settingsContent.gameObject.activeSelf)
            {
                settingsContent.localScale = Vector3.one * (1f - Mathf.Min(t, 1f));
            }
            else
            {
                foreach(Transform item in items)
                {
                    item.localScale = Vector3.one * (1f - Mathf.Min(t, 1f));
                }
            }
            screenOverlay.color = Color.Lerp(overlayColor, transparent, t);
            yield return null;
        }
        pauseContent.gameObject.SetActive(false);
        settingsContent.gameObject.SetActive(false);
        timer.StopTimer = false;
        player.UnfreezePlayerState();

        activeRoutine = null;
    }

    public void OpenSettingsMenu()
    {
        SyncSettingsWithMenu();
        StartCoroutine(OpenSettingsMenuRoutine());
    }
    public void CloseSettingsMenu()
    {
        StartCoroutine(CloseSettingsMenuRoutine());
    }
    private IEnumerator OpenSettingsMenuRoutine()
    {
        Debug.Log("Opened settings mennu");
        // Hide pause buttons
        float time = 0f;
        while(time < menuTransitionDuration * 0.5f)
        {
            time += Time.deltaTime;
            float t = time / (menuTransitionDuration * 0.5f);
            foreach(Transform item in items)
            {
                item.localScale = Vector3.one * (1f - Mathf.Min(t, 1f));
            }
            yield return null;
        }
        pauseContent.gameObject.SetActive(false);

        // Show settings
        settingsContent.gameObject.SetActive(true);
        settingsContent.localScale = Vector3.zero;
        time = 0f;
        while(time < menuTransitionDuration * 0.5f)
        {
            time += Time.deltaTime;
            float t = time / (menuTransitionDuration * 0.5f);
            settingsContent.localScale = Vector3.one * Mathf.Min(t, 1f);
            yield return null;
        }
    }
    private IEnumerator CloseSettingsMenuRoutine()
    {
        // Hide settings
        float time = 0f;
        while(time < menuTransitionDuration * 0.5f)
        {
            time += Time.deltaTime;
            float t = time / (menuTransitionDuration * 0.5f);
            settingsContent.localScale = Vector3.one * (1f - Mathf.Min(t, 1f));
            yield return null;
        }
        settingsContent.gameObject.SetActive(false);

        // Show pause buttons
        time = 0f;
        pauseContent.gameObject.SetActive(true);
        while(time < menuTransitionDuration * 0.5f)
        {
            time += Time.deltaTime;
            float t = time / (menuTransitionDuration * 0.5f);
            foreach(Transform item in items)
            {
                item.localScale = Vector3.one * Mathf.Min(t, 1f);
            }
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(MenuActive) HideMenu();
            else RevealMenu();
        }
    }
}
