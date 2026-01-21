using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : NetworkBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireCooldown = 0.4f;

    [Header("Patterns")]
    [SerializeField] private BulletPatternType currentPattern = BulletPatternType.Straight;
    [SerializeField] private int spreadBulletCount = 5;
    [SerializeField] private float spreadAngle = 30f;


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

        // Pattern wechseln
        if (keyboard.digit1Key.wasPressedThisFrame)
            currentPattern = BulletPatternType.Straight;

        if (keyboard.digit2Key.wasPressedThisFrame)
            currentPattern = BulletPatternType.Spread;

        // Schießen
        if (mouse.leftButton.isPressed && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireCooldown;
            ShootServerRpc(currentPattern);
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
        float step = spreadAngle / (spreadBulletCount - 1);
        float startAngle = -spreadAngle / 2f;

        for (int i = 0; i < spreadBulletCount; i++)
        {
            float angle = startAngle + step * i;
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
