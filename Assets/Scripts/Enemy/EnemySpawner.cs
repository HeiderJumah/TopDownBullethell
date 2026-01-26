using FishNet.Object;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private int currentWave = 1;
    private bool wavesStarted = false;

    public override void OnStartServer()
    {
        base.OnStartServer();
        // bewusst leer
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
}


