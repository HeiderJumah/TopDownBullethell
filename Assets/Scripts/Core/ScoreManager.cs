using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    public static ScoreManager Instance;

    public readonly SyncVar<int> Score = new SyncVar<int>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Score.Value = 0;
    }

    [Server]
    public void AddScore(int amount)
    {
        Score.Value += amount;
    }
}

