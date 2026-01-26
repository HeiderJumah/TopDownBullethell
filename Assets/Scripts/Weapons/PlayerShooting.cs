using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : NetworkBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireCooldown = 0.5f;

    [Header("Patterns")]
    [SerializeField] private BulletPatternType currentPattern = BulletPatternType.Straight;
    [SerializeField] private int spreadBulletCount = 3;
    [SerializeField] private float spreadAngle = 20f;

    [Header("Projectile Settings")]
    [SerializeField] private float straightBulletSpeed = 30f;
    [SerializeField] private float spreadBulletSpeed = 20f;

    // Zeitstempel f¸r das n‰chste erlaubte Schieﬂen
    private float nextFireTime;

    private void Update()
    {
        if (!IsOwner)
            return;

        HandleInput();
    }

    // Eingabeverarbeitung f¸r das Schieﬂen
    private void HandleInput()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;

        //Sraight Schieﬂen mit linker Maustaste
        if (mouse.leftButton.isPressed && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireCooldown;
            ShootServerRpc(BulletPatternType.Straight);
        }

        // Spread Schieﬂen mit rechter Maustaste
        if (mouse.rightButton.isPressed && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireCooldown;
            ShootServerRpc(BulletPatternType.Spread);
        }
    }

    // ServerRpc zum Handhaben des Schieﬂens
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

    // Straight Schussmuster
    private void FireStraight()
    {
        SpawnProjectile(firePoint.rotation, straightBulletSpeed);
    }

    // Spread Schussmuster
    private void FireSpread()
    {
        float[] angles = { -10f, 0f, 10f };

        foreach (float angle in angles)
        {
            Quaternion rot = firePoint.rotation * Quaternion.Euler(0f, angle, 0f);
            SpawnProjectile(rot, spreadBulletSpeed);
        }
    }

    // Hilfsmethode zum Spawnen von Projektilen
    private void SpawnProjectile(Quaternion rotation, float speed)
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            rotation);

        var proj = projectile.GetComponent<Projectile>();
        proj.Initialize(rotation * Vector3.forward, speed);

        Spawn(projectile);
    }
}
