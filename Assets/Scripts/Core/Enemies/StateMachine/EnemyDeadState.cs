using UnityEngine;

public class EnemyDeadState : IEnemyBaseState
{
    public void EnterState(Enemy_Controller enemy)
    {
        Debug.Log("Entering Dead State");
        enemy.enemyAnimator.SetBool("IsDead", true);
        enemy.Die();
    }

    public void TriggerEnter(Enemy_Controller enemy, Collider2D collision)
    {

    }

    public void UpdateState(Enemy_Controller enemy)
    {

    }

    public void TriggerExit(Enemy_Controller enemy, Collider2D collision)
    {

    }
}
