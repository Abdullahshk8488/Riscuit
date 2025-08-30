using UnityEngine;

public class ExplosiveEnemyMine : BaseIcing
{
    public override void Shoot(Vector2 direction)
    {
        
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
