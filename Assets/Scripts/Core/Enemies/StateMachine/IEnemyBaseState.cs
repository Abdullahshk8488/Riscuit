using UnityEngine;

public interface IEnemyBaseState
{
    void EnterState(Enemy_Controller enemy);

    void UpdateState(Enemy_Controller enemy);

    void CollisionEnter(Enemy_Controller enemy, Collision2D collision);

}
