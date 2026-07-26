using UnityEngine;
using System.Collections.Generic;

public class EnemyPlacementManager : MonoBehaviour
{
    public static EnemyPlacementManager Instance {get; private set;}
    
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private List<(AbstractEnemyController enemy, Vector3 spawnLoc)> enemies;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemies = new List<(AbstractEnemyController, Vector3)>();
        foreach (Transform child in this.transform)
        {
            var enemy = child.GetComponent<AbstractEnemyController>();
            if(enemy != null)
            {
                enemies.Add((enemy, enemy.transform.position));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetEnemies()
    {
        foreach((AbstractEnemyController enemy, Vector3 pos) in enemies)
        {
            if(enemy.IsDead()) enemy.ResetEnemy(pos);
        }
    }
}
