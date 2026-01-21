using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : NetworkBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireCooldown = 1f;

    [Header("Patterns")]
    [SerializeField] private BulletPatternType currentPattern = BulletPatternType.Straight;
    [SerializeField] private int spreadBulletCount = 3;
    [SerializeField] private float spreadAngle = 10f;
    [SerializeField] private float spiralRotationSpeed = 100f;



    private float nextFireTime;

    private void Update()
    {
        if (!IsOwner)
            return;

        HandleInput();
    }

    private void HandleInput()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;

        //Sraight Schießen mit linker Maustaste
        if (mouse.leftButton.isPressed && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireCooldown;
            ShootServerRpc(BulletPatternType.Straight);
        }

        // Spread Schießen mit rechter Maustaste
        if (mouse.rightButton.isPressed && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireCooldown;
            ShootServerRpc(BulletPatternType.Spread);
        }
    }

    [ServerRpc]
    private void ShootServerRpc(BulletPatternType pattern)
    {
        switch (pattern)
        {
            case BulletPatternType.Straight:
                FireStraight();
                break;
            case BulletPatternType.Spread:
                FireSpread();
                break;
        }
    }

    private void FireStraight()
    {
        SpawnProjectile(firePoint.rotation);
    }

    private void FireSpread()
    {
        float[] angles = { -10f, 0f, 10f };

        foreach (float angle in angles)
        {
            Quaternion rot = firePoint.rotation * Quaternion.Euler(0f, angle, 0f);
            SpawnProjectile(rot);
        }
    }

    private void SpawnProjectile(Quaternion rotation)
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            rotation);

        Spawn(projectile);
    }
}
