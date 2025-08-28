using UnityEngine;

public struct IcingData
{
    public BaseIcing icing;
    public int amount;
}

public abstract class BaseIcing : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float destroyAfterSeconds;
    [SerializeField] protected bool destroyOnHit = true;
    [SerializeField] protected float fireRate;
    [SerializeField] protected string animName;

    public float FireRate { get => fireRate; }
    public string AnimName { get => animName; }

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    public void Shoot(Vector2 direction)
    {
        rb.linearVelocity = direction * bulletSpeed;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            DamageEnemy();
        }

        if (destroyOnHit)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void DamageEnemy()
    {

    }
}
