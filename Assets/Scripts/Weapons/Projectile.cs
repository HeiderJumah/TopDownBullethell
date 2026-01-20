using FishNet.Object;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private int damage = 10;

    private Vector3 moveDirection;

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
    }

    private void DespawnSelf()
    {
        Despawn(gameObject);
    }
}


