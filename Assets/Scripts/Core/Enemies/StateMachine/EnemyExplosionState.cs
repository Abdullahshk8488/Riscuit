using UnityEngine;

public class EnemyExplosionState : IEnemyBaseState
{
    public void EnterState(Enemy_Controller enemy)
    {
        Debug.Log("Entering Explosion State");
        enemy.enemyAnimator.SetBool("IsPrep", true);
        enemy.Shot();
    }

    public void TriggerEnter(Enemy_Controller enemy, Collider2D collision)
    {

    }

    public void TriggerExit(Enemy_Controller enemy, Collider2D collision)
    {

    }

    public void UpdateState(Enemy_Controller enemy)
    {

    }
}
