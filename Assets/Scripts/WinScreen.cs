using UnityEngine;
using TMPro;

public class WinScreen : MonoBehaviour
{
    public TimerManager timer;
    public EnemyPlacementManager enemyManager;
    public float respawnInterval = 20f;
    private float dt = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer.StopTimer = true;
    }

    // Update is called once per frame
    void Update()
    {
        dt += Time.deltaTime;
        if ( dt > respawnInterval )
        {
            dt = 0f;
            enemyManager.ResetEnemies();
        }
        timer.GetComponent<TextMeshProUGUI>().enabled = false;
        timer.SetTimer(9999999f);
        timer.StopTimer = true;
    }
}
