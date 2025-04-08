using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Google.Protobuf.Collections;

/*
    void TargetEnemies()
    {
       /* foreach (GameObject tower in towers)
        {
            Turret towerScript = tower.GetComponent<Turret>();
                if (towerScript != null)
                {
                    towerScript.ChangeTargetingType("Closest");
                }
        }
    }
}*/

public class AgentController : Agent
{
    public BuildManager buildManager;
    public PlayerStats playerStats;
    public WaveSpawner waveSpawner;

    public Transform nodeParent;
    public GameObject towerPrefab;

    Node[] nodes;

    private void Start()
    {
        // Initialize the nodes array based on the number of child GameObjects
        nodes = new Node[nodeParent.childCount];

        // Iterate over all child GameObjects and store their Node component
        for (int i = 0; i < nodeParent.childCount; i++)
        {
            Transform child = nodeParent.GetChild(i);
            nodes[i] = child.GetComponent<Node>();
        }
    }
    public override void OnEpisodeBegin(){
        // Reset environment for the new episode
        ResetEnvironment();
        waveSpawner.waveIndex = 0;
        //StartCoroutine(waveSpawner.SpawnWave());
        RequestDecision(); // Request a decision to place the turret
        
    }

    void ResetEnvironment()
    {
        playerStats.Reset();
        waveSpawner.waveIndex = 0;
        foreach (var node in nodes)
        {
            node.ResetNode(); 
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        foreach (var node in nodes)
        {
            sensor.AddObservation(node.nodeState());
        }

        sensor.AddObservation(playerStats.Money / 99999f);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        PlaceTurret(actionBuffers.DiscreteActions[0]);

    }

    void PlaceTurret(int actionIndex)
    {
        if (actionIndex >= 0 && actionIndex < nodes.Length)
        {
            Node selectedNode = nodes[actionIndex];
            if (selectedNode.nodeState() == 0 && playerStats.Money >= buildManager.turretToBuild.cost)
            {
                buildManager.BuildTurretOn(selectedNode);
                AddReward(0.5f);
            }

            else
            {
                AddReward(-0.5f); // Penalize if a turret is already placed here
            }
        }
        else
        {
            AddReward(-1f); // Penalize if the action is out of bounds
        }
    }

    public void OnAllLivesLost(){
        AddReward(-5f);
        EndEpisode();
    }

    public void OnEnemyDefeated(){
        AddReward(0.1f);
    }

    public void OnLifeLost()
    {
        AddReward(-0.2f); // Penalty for life lost
    }

    public void OnWaveCompleted(int waveNumber)
    {
        float completionReward = 1.0f + (waveNumber * 0.5f); // Reward for wave completion with scaling
        AddReward(completionReward);

        if (playerStats.Lives == playerStats.startLives) // Check if no lives were lost
        {
            AddReward(2.0f); // Reward for flawless round completion
        }
    }

    public void OnWaveEndedWithUnusedResources()
    {
        float unusedResources = playerStats.Money;
        float bonus = Mathf.Min(unusedResources * 0.05f, 1.0f); // Bonus for unused resources, capped
        AddReward(bonus);
    }

    public void OnWavesCompleted()
    {
        EndEpisode();
    }
}