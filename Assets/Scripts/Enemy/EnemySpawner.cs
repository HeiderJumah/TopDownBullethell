using FishNet.Object;
using UnityEngine;
using System.Collections;

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
    [SerializeField] private float waveDelay = 2f;

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
        StartCoroutine(SpawnWaveRoutine());
    }

    [Server]
    private IEnumerator SpawnWaveRoutine()
    {
        if (currentWaveIndex >= waves.Length)
        {
            GameManager.Instance.Victory();
            yield break;
        }

        // UI-Event
        WaveUI.Instance.ShowWave(currentWaveIndex + 1);

        yield return new WaitForSeconds(waveDelay);

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
            StartCoroutine(SpawnWaveRoutine());
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        EnemyStats.OnEnemyDied -= HandleEnemyDeath;
    }
}




