using FishNet.Object;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private GameObject shooterEnemyPrefab;
    [SerializeField] private GameObject kamikazeEnemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [System.Serializable]
    public struct WaveData
    {
        public int enemyCount;
        public int enemyHealth;
    }

    [SerializeField] private WaveData[] waves;

    private int currentWaveIndex = 0;
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
        Debug.Log("START WAVES");

        SpawnWave();
    }

    [Server]
    private void SpawnWave()
    {
        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("ALL WAVES CLEARED");
            GameManager.Instance.Victory();
            return;
        }

        Debug.Log($"WAVE {currentWaveIndex + 1} START");

        WaveData wave = waves[currentWaveIndex];
        aliveEnemies = wave.enemyCount;

        for (int i = 0; i < wave.enemyCount; i++)
        {
            Transform point = spawnPoints[i % spawnPoints.Length];

            GameObject prefab =
                currentWaveIndex < 2 ? shooterEnemyPrefab : kamikazeEnemyPrefab;

            GameObject enemy = Instantiate(prefab, point.position, Quaternion.identity);
            Spawn(enemy);

            EnemyStats stats = enemy.GetComponent<EnemyStats>();
            stats.Initialize(wave.enemyHealth);
        }

        currentWaveIndex++;
    }

    [Server]
    private void HandleEnemyDeath(EnemyStats enemy)
    {
        aliveEnemies--;

        if (aliveEnemies <= 0)
        {
            Debug.Log($"WAVE {currentWaveIndex} CLEARED");
            SpawnWave();
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        EnemyStats.OnEnemyDied -= HandleEnemyDeath;
    }
}



