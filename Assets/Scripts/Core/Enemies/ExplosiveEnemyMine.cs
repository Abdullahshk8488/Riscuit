using UnityEngine;

public class ExplosiveEnemyMine : BaseIcing
{

    protected override void Start()
    {

    }

    public override void Shoot(Vector2 direction)
    {
        
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           if (collision.GetComponent<Player>() != null)
           {
               collision.GetComponent<IDamageable>().DamageTaken(damage);
               Destroy(gameObject);
            }
        }
    }
}
