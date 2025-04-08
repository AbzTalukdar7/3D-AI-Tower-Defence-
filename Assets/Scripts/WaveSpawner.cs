using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public GameManager gameManager;

    PlayerStats playerStats;
    AgentController agentController;
    Waypoints waypoints;

    public GameObject enemyPrefab;
    public Transform spawnPoint;

    public float timeBetweenWaves = 2f;
    public Text waveCountdownText;
    private float countdown = 2f;
    public int waveIndex = 0;
    public int wavesToSpawn = 15;

    private List<Enemy> spawnedEnemies = new List<Enemy>();

    private void Start()
    {
        playerStats = gameManager.playerStats;
        agentController = gameManager.aiController;
        waypoints = gameManager.waypoints;
    }

    void Update()
    {
        if (spawnedEnemies.Count > 0)
        {
            return;
        }

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        waveCountdownText.text = string.Format("{0:00.0}", countdown);
    }

    IEnumerator SpawnWave()
    {
        if (waveIndex >= wavesToSpawn)
        {
            agentController.OnWavesCompleted();
            Debug.Log("All waves completed!");
            yield break; // Exit the coroutine, preventing more waves from spawning.
        }
        waveIndex++;

        for (int i = 0; i < waveIndex; i++)
        {
            GameObject g = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            Enemy enemy = g.GetComponent<Enemy>();
            enemy.SetPlayerStats(playerStats);
            enemy.SetAIController(agentController);
            enemy.SetWaypoints(waypoints);
            enemy.SetWaveSpawner(this);
            spawnedEnemies.Add(enemy); // Add the enemy to the list of spawned enemies
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void OnEnemyDeath(Enemy enemy)
    {
        // Remove the enemy from the list of spawned enemies when it dies
        spawnedEnemies.Remove(enemy);
    }

    IEnumerator CheckWaveCompletion()
    {
        // Wait until all enemies from the current wave are defeated
        yield return new WaitUntil(() => spawnedEnemies.Count == 0);

        // Once all enemies are defeated, call OnWaveCompleted from the AgentController
        Debug.Log("Wave " + waveIndex + " completed");
        agentController.OnWaveCompleted(waveIndex);
        agentController.OnWaveEndedWithUnusedResources();
    }
}
