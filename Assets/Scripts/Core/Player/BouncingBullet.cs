using UnityEngine;
public class BouncingBullet : BaseIcing
{
    [SerializeField] private int numberOfBounces;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            return;
        }

        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy_Controller>().DamageTaken(damage);
        }

        BounceBullet(collision);
        numberOfBounces--;
        if (numberOfBounces == 0)
        {
            Destroy(gameObject);
        }
    }

    private void BounceBullet(Collider2D collision)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[1];
        int filled = collision.GetContacts(contacts);
        var firstContact = contacts[0];

        Vector2 newVelocity = -Vector2.Reflect(rb.linearVelocity, firstContact.normal).normalized;
        Shoot(newVelocity);
    }
}
