using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public enum GameState
{
    Playing,
    GameOver,
    Victory
}

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    [SerializeField] private EnemySpawner enemySpawner;

    public readonly SyncVar<GameState> CurrentState = new SyncVar<GameState>();

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

        Debug.Log("SERVER STARTED");

        CurrentState.Value = GameState.Playing;
        Debug.Log("GAME START");

        if (enemySpawner == null)
        {
            Debug.LogError("EnemySpawner NOT assigned in GameManager!");
            return;
        }

        enemySpawner.StartWaves();
    }

    [Server]
    public void PlayerDied()
    {
        if (CurrentState.Value != GameState.Playing)
            return;

        CurrentState.Value = GameState.GameOver;
        Debug.Log("GAME OVER");
    }

    [Server]
    public void Victory()
    {
        if (CurrentState.Value != GameState.Playing)
            return;

        CurrentState.Value = GameState.Victory;
        Debug.Log("VICTORY");
    }
}

