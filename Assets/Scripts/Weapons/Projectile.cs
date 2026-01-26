using FishNet.Object;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifetime = 15f;
    [SerializeField] private int damage = 10;

    private Vector3 moveDirection;

    // Wird vom Server beim Spawn gesetzt 
    // Speed & Direction serverseitig initialisieren 
    // damit Clients die Werte nicht manipulieren können und Cheats vermeiden
    public void Initialize(Vector3 direction, float projectileSpeed)
    {
        moveDirection = direction.normalized;
        speed = projectileSpeed;
    }
    public override void OnStartServer()
    {
        base.OnStartServer();

        // Richtung EINMAL festlegen
        moveDirection = transform.forward;

        Invoke(nameof(DespawnSelf), lifetime);
    }

    private void Update()
    {
        if (!IsServerInitialized)
            return;

        transform.position += moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServerInitialized)
            return;

        if (other.TryGetComponent<PlayerStats>(out var stats))
        {
            stats.TakeDamage(damage);
            Debug.Log("Projectile hit player");
            DespawnSelf();
        }

        if (other.TryGetComponent<EnemyStats>(out var enemy))
        {
            enemy.TakeDamage(damage);
            DespawnSelf();
        }
    }

    private void DespawnSelf()
    {
        Despawn(gameObject);
    }
}


