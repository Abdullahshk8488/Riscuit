using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    [field: SerializeField] public Gun PlayerGun { get; private set; }
    [field: SerializeField] public Dash PlayerDash { get; private set; }
    [field: SerializeField] public static bool IsMoving { get; private set; }
    private void Awake()
    {
        PlayerManager.Instance.SetPlayer(this);
    }

    public void Move(Vector2 direction)
    {
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
}
