using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour, IDamagable
{
    [field: SerializeField] public Node currentNode { get; private set; }
    [field: SerializeField] public List<Node> path { get; private set; }
    [field: SerializeField] public float MaxHealth { get; set; }
    [field: SerializeField] public float CurrentHealth { get; set; }

    public Player player;
    public float speed = 3;


    private IEnemyBaseState _currentState;
    private EnemyPursueState PursueState = new EnemyPursueState();
    private EnemyAttackState AttackState = new EnemyAttackState();
    private EnemyRunAwayState RunAwayState = new EnemyRunAwayState();
    private EnemyDeadState DeadState = new EnemyDeadState();



    private void Awake()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.GetComponent<Player>();
        }

        if (player == null)
        {
            Debug.LogError("Player component not found on the GameObject tagged as 'Player'.");
        }

        CurrentHealth = MaxHealth;
        _currentState = PursueState;
        _currentState.EnterState(this);
    }

    private void Update()
    {
        _currentState.UpdateState(this);
    }

    private void SwitchState(IEnemyBaseState newState)
    {
        _currentState = newState;
        path.Clear();
        _currentState.EnterState(this);
    }

    public void DamageTaken(float damageAmount)
    {
        CurrentHealth -= damageAmount;
    }
}
