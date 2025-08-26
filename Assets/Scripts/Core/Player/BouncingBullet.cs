using UnityEngine;
public class BouncingBullet : BaseIcing
{
    [SerializeField] private int numberOfBounces;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        numberOfBounces--;
        if (numberOfBounces == 0)
        {
            Destroy(gameObject);
        }
    }
}
