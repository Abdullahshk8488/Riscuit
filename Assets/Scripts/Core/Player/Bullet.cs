using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float destroyAfterSeconds;

    private void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    public void Shoot(Vector2 direction)
    {
        rb.linearVelocity = direction * bulletSpeed;
    }    
}
