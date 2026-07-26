using UnityEngine;

public abstract class AbstractEnemyController : MonoBehaviour
{
    protected bool isDead;
    public bool IsDead() {return isDead;}
    public abstract void ResetEnemy(Vector3 vec);
}
