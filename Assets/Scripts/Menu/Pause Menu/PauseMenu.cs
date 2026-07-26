using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Transform content;
    private System.Collections.Generic.List<Transform> items;
    public Image screenOverlay;
    private Color overlayColor;
    private Color transparent;
    public float revealPauseMenuDuration = 0.2f;

    public PlayerMovement player;
    public TimerManager timer;

    private Coroutine activeRoutine;

    private bool MenuActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        items = new System.Collections.Generic.List<Transform>();
        foreach(Transform child in content)
        {
            items.Add(child);
            child.localScale = Vector3.zero;
        }
        overlayColor = screenOverlay.color;
        transparent = new Color(0f, 0f, 0f, 0f);
        screenOverlay.color = transparent;

        content.gameObject.SetActive(false);
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
        content.gameObject.SetActive(true);
        timer.StopTimer = true;
        player.FreezePlayer();

        float time = 0f;
        while(time < revealPauseMenuDuration)
        {
            time += Time.deltaTime;
            float t = time / revealPauseMenuDuration;
            foreach(Transform item in items)
            {
                item.localScale = Vector3.one * Mathf.Min(t, 1f);
            }
            screenOverlay.color = Color.Lerp(transparent, overlayColor, t);
            yield return null;
        }
    }
    private IEnumerator HidePauseMenuRoutine()
    {
        float time = 0f;
        while(time < revealPauseMenuDuration)
        {
            time += Time.deltaTime;
            float t = time / revealPauseMenuDuration;
            foreach(Transform item in items)
            {
                item.localScale = Vector3.one * (1f - Mathf.Min(t, 1f));
            }
            screenOverlay.color = Color.Lerp(overlayColor, transparent, t);
            yield return null;
        }
        content.gameObject.SetActive(false);
        timer.StopTimer = false;
        player.UnfreezePlayer();
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
