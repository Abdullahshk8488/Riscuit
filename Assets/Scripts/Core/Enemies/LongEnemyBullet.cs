using UnityEngine;

public class LongEnemyBullet : BaseIcing
{
    [SerializeField] private Collider2D bulletCollider;

    private GameObject _player;

    public override void Shoot(Vector2 direction)
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        Vector2 toPlayer = ((Vector2)_player.transform.position - (Vector2)transform.position).normalized;

        float angle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
