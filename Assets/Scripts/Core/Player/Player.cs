using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private Rigidbody2D rb;
    [field: SerializeField] public float moveSpeed { get; private set; }
    [field: SerializeField] public Gun PlayerGun { get; private set; }
    [field: SerializeField] public Dash PlayerDash { get; private set; }
    [field: SerializeField] public static bool IsMoving { get; private set; }
    [field: SerializeField] public float MaxHealth { get; set; }
    [field: SerializeField] public float CurrentHealth { get; set; }
    public static Vector2 MoveDirection { get; private set; }

    private void Awake()
    {
        PlayerManager.Instance.SetPlayer(this);
        CurrentHealth = MaxHealth;
    }

    public void Move(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            MoveDirection = direction;
        }

        if (PlayerDash.IsDashing)
        {
            return;
        }

        rb.linearVelocity = direction * moveSpeed;
    }

    public void SetIsMoving(bool isMoving)
    {
        IsMoving = isMoving;
    }

    public void DamageTaken(float damageAmount)
    {
        CurrentHealth -= damageAmount;
    }
}
