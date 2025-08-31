using UnityEngine;

public class LongEnemyBullet : BaseIcing
{
    [SerializeField] private Collider2D bulletCollider;

    private GameObject _player;

    public override void Shoot(Vector2 direction)
    {
        base.Shoot(direction);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                collision.GetComponent<IDamageable>().DamageTaken(damage);

                Destroy(gameObject);
            }
        }
    }
}
