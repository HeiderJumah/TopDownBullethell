using FishNet.Object;
using UnityEngine;

public class EnemyShooter : NetworkBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireCooldown = 1.5f;

    private float fireTimer;

    public override void OnStartServer()
    {
        base.OnStartServer();
        fireTimer = fireCooldown;
    }

    private void Update()
    {
        if (!IsServerInitialized)
            return;

        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireCooldown;
        }
    }

    [Server]
    private void Shoot()
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation);

        Spawn(projectile);
    }
}

