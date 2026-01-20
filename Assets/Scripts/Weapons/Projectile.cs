using FishNet.Object;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private int damage = 10;

    public override void OnStartServer()
    {
        base.OnStartServer();
        Invoke(nameof(DespawnSelf), lifetime);
    }

    private void Update()
    {
        if (!IsServer)
            return;

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer)
            return;

        if (other.TryGetComponent<PlayerStats>(out var stats))
        {
            stats.TakeDamageServerRpc(damage);
            DespawnSelf();
        }
    }

    private void DespawnSelf()
    {
        Despawn(gameObject);
    }
}

