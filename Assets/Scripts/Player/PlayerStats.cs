using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    // SyncVars als Wrapper
    public readonly SyncVar<int> Health = new SyncVar<int>();

    [SerializeField] private int maxHealth = 100;

    public readonly SyncVar<Color> PlayerColor = new SyncVar<Color>();

    public static System.Action<PlayerStats> OnLocalPlayerSpawned;

    public override void OnStartServer()
    {
        base.OnStartServer();

        Health.Value = maxHealth;
        PlayerColor.Value = Random.ColorHSV();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsOwner)
            OnLocalPlayerSpawned?.Invoke(this);

        // Listener registrieren
        Health.OnChange += OnHealthChanged;
        PlayerColor.OnChange += OnColorChanged;

        // Initial anwenden
        ApplyColor(PlayerColor.Value);
    }

    private void OnHealthChanged(int oldValue, int newValue, bool asServer)
    {
        Debug.Log($"Health changed: {oldValue} -> {newValue}");
    }

    private void OnColorChanged(Color oldColor, Color newColor, bool asServer)
    {
        ApplyColor(newColor);
    }

    private void ApplyColor(Color color)
    {
        var renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
            renderer.material.color = color;
    }

    public void TakeDamage(int damage)
    {
        if (!IsServerInitialized)
            return;

        Health.Value = Mathf.Max(Health.Value - damage, 0);
        Debug.Log($"Player took damage. HP: {Health.Value}");

        if (Health.Value <= 0)
            Die();

        if (GameManager.Instance.CurrentState != GameState.Playing)
            return;
    }

    private void Die()
    {
        Debug.Log("Player died");
        GameManager.Instance.PlayerDied();
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int damage)
    {
        TakeDamage(damage);
    }

}
