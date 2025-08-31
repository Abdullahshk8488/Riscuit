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
        if (enemy.CurrentHealth <= 0)
        {
            enemy.SwitchState(enemy.DeadState);
            return;
        }

        // --- Separation Logic ---
        Vector2 separation = Vector2.zero;
        float separationRadius = 1.5f; // tweak as needed
        float separationStrength = 1.0f; // tweak as needed

        Collider2D[] hits = Physics2D.OverlapCircleAll(enemy.transform.position, separationRadius);
        int nearbyEnemies = 0;
        foreach (var hit in hits)
        {
            if (hit.gameObject != enemy.gameObject && hit.TryGetComponent<Enemy_Controller>(out var otherEnemy))
            {
                Vector2 away = (enemy.transform.position - otherEnemy.transform.position);
                float distance = away.magnitude;
                if (distance > 0)
                {
                    separation += away.normalized / distance;
                    nearbyEnemies++;
                }
            }
        }

        // --- Pathfinding Movement ---
        Pursue();
        Createpath(enemy);

        // --- Combine Movement ---
        Vector2 moveDirection = Vector2.zero;
        if (_path != null && _path.Count > 0)
        {
            Node nextNode = _path[0];
            moveDirection = (nextNode.transform.position - enemy.transform.position).normalized;
        }

        Vector2 finalDirection = (moveDirection + separation * separationStrength).normalized;
        enemy.transform.position += (Vector3)(finalDirection * enemy.speed * Time.deltaTime);
    }

    public void TriggerEnter(Enemy_Controller enemy, Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemy.CanShoot = true;
            enemy.IsShooting = true;
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
