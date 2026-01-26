using FishNet.Object;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsServerInitialized)
        {
            enemySpawner.StartWaves();
        }
    }
}
