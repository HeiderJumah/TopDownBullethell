using FishNet.Object;
using UnityEngine;

public enum GameState
{
    Playing,
    GameOver,
    Victory
}

public class GameManager : NetworkBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    public override void OnStartClient()
    {
        base.OnStartServer();
        StartGame();
        enemySpawner.StartWaves();
    }

    public static GameManager Instance;

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        CurrentState = GameState.Playing;
    }

    public void StartGame()
    {
        CurrentState = GameState.Playing;
        Debug.Log("GAME START");
    }

    public void PlayerDied()
    {
        if (CurrentState != GameState.Playing)
            return;

        CurrentState = GameState.GameOver;
        Debug.Log("GAME OVER");
    }

    public void Victory()
    {
        if (CurrentState != GameState.Playing)
            return;

        CurrentState = GameState.Victory;
        Debug.Log("VICTORY");
    }
}
