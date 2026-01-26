using FishNet.Object;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private int currentWave = 1;
    private int aliveEnemies = 0;
    private bool wavesStarted = false;

    [System.Serializable]
    public struct WaveData
    {
        public int enemyCount;
        public int enemyHealth;
    }
    
    [SerializeField] private WaveData[] waves;


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

    private int currentWaveIndex = 0;

    [Server]
    private void SpawnWave()
    {
        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("All WAVES CLEAR");
            GameManager.Instance.Victory();
            return;
        }

        Debug.Log($"WAVE {currentWaveIndex + 1} START");

        WaveData wave = waves[currentWaveIndex];
        aliveEnemies = wave.enemyCount;

        for (int i = 0; i < wave.enemyCount; i++)
        {
            Transform point = spawnPoints[i % spawnPoints.Length];

            GameObject enemy = Instantiate(
                enemyPrefab,
                point.position,
                Quaternion.identity);

            Spawn(enemy);

            EnemyStats stats = enemy.GetComponent<EnemyStats>();
            stats.Initialize(wave.enemyHealth);
        }

        currentWaveIndex++;
    }

    private void HandleEnemyDeath(EnemyStats enemy)
    {
        {
            if (!IsServerInitialized)
                return;

            aliveEnemies--;

            if (aliveEnemies <= 0)
            {
                Debug.Log($"WAVE {currentWaveIndex} CLEAR");
                SpawnWave();
            }
        }

    }


    public override void OnStopServer()
    {
        base.OnStopServer();
        EnemyStats.OnEnemyDied -= HandleEnemyDeath;
    }

}


