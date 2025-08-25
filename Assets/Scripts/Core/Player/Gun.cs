using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gun : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform bulletSpawnLocation;
    [SerializeField] private float fireRate;
    [SerializeField] private float ammoCount;

    private bool _canShoot = true;
    private bool _onCooldown = false;
    private bool _isShooting = false;
    private Vector2 _direction = Vector2.right;

    private void Update()
    {
        if (
            _onCooldown
            || !_canShoot
            || !_isShooting
            || ammoCount == 0
            )
        {
            return;
        }

        Shoot();
    }

    public void SetShootDirection(Vector2 direction)
    {
        _direction = direction;
        // y direction takes priority
        if (direction.y != 0.0f)
        {
            _direction.x = 0.0f;
        }
        //else if (direction.x != 0.0f)
        //{
        //    moveDirection = direction.x > 0.0f ? PlayerMovement.Direction.Right : PlayerMovement.Direction.Left;
        //}

        // Rotate the gun
        transform.right = _direction;
    }

    public void SetIsShooting(bool isShooting)
    {
        _isShooting = isShooting;
    }

    public void Shoot()
    {
        // Spawn the bullet
        Bullet bullet = Instantiate(bulletPrefab);
        bullet.transform.position = bulletSpawnLocation.position;
        bullet.Shoot(_direction);

        // Go on cooldown
        _onCooldown = true;
        StartCoroutine(ResetCooldown());
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(1.0f / fireRate);
        _onCooldown = false;
    }

    public void Reload(float ammo)
    {
        ammoCount += ammo;
    }
}
