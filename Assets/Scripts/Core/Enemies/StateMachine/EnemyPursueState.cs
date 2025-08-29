using System.Collections.Generic;
using UnityEngine;

public class EnemyPursueState : IEnemyBaseState
{
    private Player _player;
    private float _speed;
    private Node _currentNode;
    private List<Node> _path;

    public void EnterState(Enemy_Controller enemy)
    {
        Debug.Log("Entering Pursue State");
        _player = enemy.player;
        _speed = enemy.speed;
        _currentNode = enemy.currentNode;
        _path = enemy.path;
    }

    public void UpdateState(Enemy_Controller enemy)
    {
        Pursue();
        Createpath(enemy);
    }

    public void TriggerEnter(Enemy_Controller enemy, Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemy.CanShoot = true;
            enemy.IsShooting = true;
            enemy.OnCooldown = false;
            enemy.SwitchState(enemy.AttackState);
        }
    }

    public void TriggerExit(Enemy_Controller enemy, Collider2D collision)
    {

    }

    private void Pursue()
    {
        if (_path.Count == 0)
        {
            _path = AStarManager.Instance.GeneratePath(_currentNode, AStarManager.Instance.FindNearestNode(_player.transform.position));
        }
    }

    private void Createpath(Enemy_Controller enemy)
    {
        if (_path.Count > 0)
        {
            int x = 0;

            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, new Vector3(_path[x].transform.position.x, _path[x].transform.position.y, -1),
                _speed * Time.deltaTime);

            if (Vector2.Distance(enemy.transform.position, _path[x].transform.position) < 1.0f)
            {
                _currentNode = _path[x];
                _path.RemoveAt(x);
            }
        }
    }
}
