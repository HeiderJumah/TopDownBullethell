using FishNet.Object;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    public override void OnStartServer()
    {
        base.OnStartServer();
        SpawnWave(3);
    }

    private void SpawnWave(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Transform point = spawnPoints[i % spawnPoints.Length];

            GameObject enemy = Instantiate(
                enemyPrefab,
                point.position,
                Quaternion.identity);

            Spawn(enemy);
        }
    }
}

