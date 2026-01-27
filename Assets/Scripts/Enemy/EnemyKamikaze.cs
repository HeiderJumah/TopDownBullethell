using FishNet.Object;
using UnityEngine;

public class EnemyKamikaze : NetworkBehaviour
{
    [SerializeField] private int collisionDamage = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServerInitialized)
            return;

        if (other.TryGetComponent<PlayerStats>(out var player))
        {
            player.TakeDamageServerRpc(collisionDamage);
            Despawn(gameObject);
        }
    }
}
