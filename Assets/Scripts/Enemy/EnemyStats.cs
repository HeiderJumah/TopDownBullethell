using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class EnemyStats : NetworkBehaviour
{
    public static System.Action<EnemyStats> OnEnemyDied;

    public readonly SyncVar<int> Health = new SyncVar<int>();

    private Renderer enemyRenderer;
    private Color originalColor;

    public override void OnStartClient()
    {
        base.OnStartClient();

        enemyRenderer = GetComponentInChildren<Renderer>();
        if (enemyRenderer != null)
            originalColor = enemyRenderer.material.color;
    }

    [Server]
    public void Initialize(int maxHealth)
    {
        Health.Value = maxHealth;
    }

    [Server]
    public void TakeDamage(int damage)
    {
        Health.Value -= damage;

        FlashObserversRpc();

        if (Health.Value <= 0)
            Die();
    }

    [ObserversRpc]
    private void FlashObserversRpc()
    {
        if (enemyRenderer == null)
            return;

        enemyRenderer.material.color = Color.red;
        Invoke(nameof(ResetColor), 0.1f);
    }

    private void ResetColor()
    {
        if (enemyRenderer != null)
            enemyRenderer.material.color = originalColor;
    }

    [Server]
    private void Die()
    {
        OnEnemyDied?.Invoke(this);
        ScoreManager.Instance.AddScore(1);
        Despawn(gameObject);
    }
}


