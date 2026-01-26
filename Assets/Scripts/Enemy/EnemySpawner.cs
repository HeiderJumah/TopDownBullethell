using FishNet.Object;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private int currentWave = 1;
    private int aliveEnemies = 0;
    private bool wavesStarted = false;

    public override void OnStartServer()
    {
        base.OnStartServer();
        EnemyStats.OnEnemyDied += HandleEnemyDeath;
    }

    [Server]
    public void StartWaves()
    {
        if (wavesStarted)
            return;

        wavesStarted = true;
        SpawnWave();
    }

    [Server]
    private void SpawnWave()
    {
        int enemyCount = currentWave * 2;
        aliveEnemies = enemyCount;

        for (int i = 0; i < enemyCount; i++)
        {
            Transform point = spawnPoints[i % spawnPoints.Length];

            GameObject enemy = Instantiate(
                enemyPrefab,
                point.position,
                Quaternion.identity);

            Spawn(enemy);
        }

        currentWave++;
    }

    private void HandleEnemyDeath(EnemyStats enemy)
    {
        if (!IsServerInitialized)
            return;

        aliveEnemies--;

        if (aliveEnemies <= 0)
        {
            SpawnWave();
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        EnemyStats.OnEnemyDied -= HandleEnemyDeath;
    }

}


