using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private BaseIcing bulletPrefab;
    [SerializeField] private Transform bulletSpawnLocation;
    [SerializeField] private int startAmmo;
    [SerializeField] private float reloadRadius;
    [SerializeField] private int maxAmmo;
    [SerializeField] private SpriteRenderer ammoSprite;

    [SerializeField] private LayerMask ammoDropLayerMask;

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
        yield return new WaitForSeconds(1.0f / _currentIcing.FireRate);
        _onCooldown = false;
    }

    public void Reload()
    {
        var collidedItem = Physics2D.OverlapCircle(transform.position, reloadRadius, ammoDropLayerMask);

        AmmoDrop ammoDrop = collidedItem.GetComponent<AmmoDrop>();
        _currentAmmo = Mathf.Min(_currentAmmo + ammoDrop.AmmoAmount, maxAmmo);
        _currentIcing = ammoDrop.BulletPrefab;
        UpdateAmmoSprite();

        Destroy(collidedItem.gameObject);
    }
}
