using UnityEngine;

public interface IEnemyBaseState
{
    void EnterState(Enemy_Controller enemy);

    void UpdateState(Enemy_Controller enemy);

    void TriggerEnter(Enemy_Controller enemy, Collider2D collision);

    void TriggerExit(Enemy_Controller enemy, Collider2D collision);

}
