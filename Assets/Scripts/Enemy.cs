using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public PlayerStats playerStats;
    public AgentController aiController;
    public Waypoints waypoints;
    public WaveSpawner waveSpawner;

    public float speed = 10f;
    public int health = 100;
    public int value = 10;
    public GameObject deathEffect;

    private Transform target;
    private int waypointIndex = 0;

    public float reachThreshold = 0.2f;

    void Start()
    { 
        if (waypoints != null && waypoints.path.Length > 0)
        {
            target = waypoints.path[0];
        }
    }

    void Update()
    {
        if (target == null) return;

        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= reachThreshold)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if (waypointIndex >= waypoints.path.Length - 1)
        {
            EndPath();
            return;
        }

        waypointIndex++;
        target = waypoints.path[waypointIndex];
    }

    void EndPath()
    {
        playerStats.Lives--;
        waveSpawner.OnEnemyDeath(this);
        aiController.OnLifeLost();
        Destroy(gameObject);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        playerStats.Money += value;
        playerStats.kills += 1;
        aiController.OnEnemyDefeated();

        // GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        // Destroy(effect, 5f);

        waveSpawner.OnEnemyDeath(this);
        Destroy(gameObject);
    }

    // Assigning scripts to the enemy (for parallel training)
    public void SetPlayerStats(PlayerStats stats)
    {
        playerStats = stats;
    }

    public void SetAIController(AgentController controller)
    {
        aiController = controller;
    }

    public void SetWaveSpawner(WaveSpawner wave)
    {
        waveSpawner = wave;
    }

    public void SetWaypoints(Waypoints points)
    {
        waypoints = points;
        if (waypoints != null && waypoints.path.Length > 0)
        {
            target = waypoints.path[0];
        }
    }
}
