using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform bulletSpawnLocation;
    [SerializeField] private float fireRate;
    [SerializeField] private int startAmmo;
    [SerializeField] private float reloadRadius;
    [SerializeField] private int maxAmmo;
    [SerializeField] private SpriteRenderer ammoSprite;

    private int _currentAmmo = 0;
    private bool _canShoot = true;
    private bool _onCooldown = false;
    private bool _isShooting = false;
    private Vector2 _direction = Vector2.right;
    private BaseIcing _currentIcing;

    private void Awake()
    {
        _currentIcing = bulletPrefab;
        _currentAmmo = startAmmo;
        UpdateAmmoSprite();
    }

    private void Update()
    {
        if (
            _onCooldown
            || !_canShoot
            || !_isShooting
            || _currentAmmo == 0
            )
        {
            return;
        }

        Shoot();
    }

    private void UpdateAmmoSprite()
    {
        float xScale = (float)_currentAmmo / (float)maxAmmo;
        Vector3 scale = new Vector3(
            xScale,
            ammoSprite.transform.localScale.y,
            ammoSprite.transform.localScale.z);

        float xTransform = (xScale - 1.0f) * 0.5f;
        Vector3 position = new Vector3(
            xTransform,
            ammoSprite.transform.localPosition.y,
            ammoSprite.transform.localPosition.z);

        ammoSprite.transform.localScale = scale;
        ammoSprite.transform.localPosition = position;
    }

    public void SetShootDirection(Vector2 direction)
    {
        _direction = direction;
        // y direction takes priority
        if (direction.y != 0.0f)
        {
            _direction.x = 0.0f;
        }

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
        BaseIcing bullet = Instantiate(_currentIcing);
        bullet.transform.position = bulletSpawnLocation.position;
        bullet.Shoot(_direction);
        _currentAmmo--;
        UpdateAmmoSprite();

        // Go on cooldown
        _onCooldown = true;
        StartCoroutine(ResetCooldown());
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(1.0f / fireRate);
        _onCooldown = false;
    }

    public void Reload()
    {
        Debug.Log("Reloading");
        int ammo = 0;
        var collidedItems = Physics2D.OverlapCircleAll(transform.position, reloadRadius);
        foreach (var item in collidedItems)
        {
            if (item.CompareTag("Corpse"))
            {
                // Get the ammo component
                // Increment the ammo
                if(item.TryGetComponent(out BaseIcing newIcing))
                {
                    _currentIcing = newIcing;
                }

                ammo += 10;
            }
        }

        _currentAmmo += ammo;
        _currentAmmo = Mathf.Min(_currentAmmo, maxAmmo);
        UpdateAmmoSprite();
    }
}
