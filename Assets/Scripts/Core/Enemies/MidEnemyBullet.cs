using UnityEngine;

public class MidEnemyBullet : BaseIcing
{
    [SerializeField] private Collider2D bulletCollider;
    [SerializeField] private Animator animator;

    public override void Shoot(Vector2 direction)
    {
        base.Shoot(direction);
        animator.SetBool("IsShooting", true);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
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
