using FishNet.Object;
using UnityEngine;

public class KillZone : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!IsServerInitialized)
            return;

        EnemyStats enemy = other.GetComponent<EnemyStats>();
        if (enemy != null)
        {
            enemy.TakeDamage(100);
        }
    }
}

