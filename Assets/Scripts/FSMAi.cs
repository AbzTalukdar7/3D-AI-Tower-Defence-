using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMAi : MonoBehaviour
{
    public BuildManager buildManager;
    public PlayerStats playerStats;
    public WaveSpawner waveSpawner;

    public Transform nodeParent;

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
    enum AIState { Idle, PlaceTurret, UpgradeTurret, AnalyzeThreat }
    AIState currentState = AIState.Idle;

    void Update()
    {
        switch (currentState)
        {
            case AIState.Idle:
                // Implement Idle behavior
                CheckForResources();
                break;
            case AIState.PlaceTurret:
                // Implement turret placement logic
                PlaceTurret();
                currentState = AIState.Idle; // Transition back to Idle or another state as needed
                break;
        }
    }

    void CheckForResources()
    {
        if (playerStats.Money >= buildManager.turretToBuild.cost)
        {
            currentState = AIState.PlaceTurret;
        }
        else
        {
            currentState = AIState.Idle;
        }
    }

    void PlaceTurret()
    {
        Node highestValueNode = null;
        float highestValue = int.MinValue;

        // Iterate through all nodes to find the one with the highest value
        foreach (Node node in nodes)
        {
            // Calculate the value of the node (you should replace this with your own logic)
            float nodeValue = node.weight;

            // Update highestValueNode if the current node has a higher value
            if (nodeValue > highestValue)
            {
                highestValue = nodeValue;
                highestValueNode = node;
            }
        }

        // Check if a valid node with the highest value was found
        if (highestValueNode != null)
        {
            // Place a turret on the node with the highest value
            buildManager.BuildTurretOn(highestValueNode);
        }
        else
        {
            Debug.LogWarning("No valid node found to place turret.");
        }
    }
}

