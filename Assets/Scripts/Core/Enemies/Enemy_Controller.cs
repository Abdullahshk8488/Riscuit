using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour, IDamageable
{
    [Header("Pathfinding")]
    [field: SerializeField] public Node currentNode { get; private set; }
    [field: SerializeField] public List<Node> path { get; private set; }
    [Header("Health")]
    [field: SerializeField] public float MaxHealth { get; set; }
    [field: SerializeField] public float CurrentHealth { get; set; }
    [Header("Enemy Data")]
    public BaseIcing bulletPrefab;
    public AmmoDrop ammoDropPrefab;
    public Transform bulletSpawnLocation;
    public Player player;
    public float speed = 3;
    [Header("Animator")]
    public Animator enemyAnimator;
    [SerializeField] private Animator attackAnimator;
    [SerializeField] private float animationDuration;

    public bool IsShooting { get; set; } = true;
    public bool CanShoot { get; set; } = true;
    public bool OnCooldown { get; set; } = false;

    private IEnemyBaseState _currentState;
    public EnemyPursueState PursueState = new EnemyPursueState();
    public EnemyAttackState AttackState = new EnemyAttackState();
    public EnemyRunAwayState RunAwayState = new EnemyRunAwayState();
    public EnemyDeadState DeadState = new EnemyDeadState();



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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        _currentState.TriggerEnter(this, collision);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        _currentState.TriggerExit(this, collision);
    }

    public void SwitchState(IEnemyBaseState newState)
    {
        _currentState = newState;
        path.Clear();
        _currentState.EnterState(this);
    }

    public void DamageTaken(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        Debug.Log($"Enemy took {damageAmount} damage. Current health: {CurrentHealth}");
    }
    public void Die()
    {
        StartCoroutine(Dying());
    }

    private IEnumerator Dying()
    {
        yield return new WaitForSeconds(1.0f);
        Instantiate(ammoDropPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void Shot()
    {
        if (OnCooldown) return;

        OnCooldown = true;
        StartCoroutine(AttackSequence());
    }

    private IEnumerator AttackSequence()
    {
        // Pre attack animation
        enemyAnimator.SetBool("IsAttacking", true);
        attackAnimator.SetBool("ResetAmmo", false);

        yield return new WaitForSeconds(animationDuration);

        // Shoot bullet
        BaseIcing bullet = Instantiate(bulletPrefab);
        bullet.transform.position = bulletSpawnLocation.position;
        Vector2 direction = (player.transform.position - transform.position).normalized;
        bullet.Shoot(direction);

        // Go on cooldown
        StartCoroutine(ResetCooldown());
    }

    private IEnumerator ResetCooldown()
    {
        attackAnimator.SetBool("ResetAmmo", true);
        yield return new WaitForSeconds(1.0f / bulletPrefab.FireRate);
        OnCooldown = false;
    }
}
