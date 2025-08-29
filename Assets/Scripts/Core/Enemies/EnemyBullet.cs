using UnityEngine;

public class EnemyBullet : BaseIcing
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                //player.TakeDamage(1);
            }
        }
    }
}
