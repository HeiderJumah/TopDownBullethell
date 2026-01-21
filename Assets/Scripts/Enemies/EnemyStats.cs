using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class EnemyStats : NetworkBehaviour
{
    public readonly SyncVar<int> Health = new SyncVar<int>();

    public override void OnStartServer()
    {
        base.OnStartServer();
        Health.Value = 30;
    }

    public void TakeDamage(int damage)
    {
        if (!IsServerInitialized)
            return;

        Health.Value -= damage;

        if (Health.Value <= 0)
            Die();
    }

    private void Die()
    {
        Despawn(gameObject);
    }
}

