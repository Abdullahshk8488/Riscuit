using UnityEngine;

public class EnemyAttackState : IEnemyBaseState
{

    public void EnterState(Enemy_Controller enemy)
    {
        Debug.Log("Entering Attack State");
    }

    public void UpdateState(Enemy_Controller enemy)
    {
        if (enemy.CurrentHealth <= 0)
        {
            enemy.SwitchState(enemy.DeadState);
            return;
        }
        if (
            enemy.OnCooldown
            || !enemy.CanShoot
            || !enemy.IsShooting
            )
        {
            return;
        }

        enemy.Shot();
    }

    public void TriggerEnter(Enemy_Controller enemy, Collider2D collision)
    {
    }

    public void TriggerExit(Enemy_Controller enemy, Collider2D collision)
    {
        enemy.CanShoot = false;
        enemy.IsShooting = false;

        enemy.SwitchState(enemy.PursueState);
    }
}
