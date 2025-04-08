using UnityEngine;

public class WRDAi : MonoBehaviour
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
    void Update()
    {
        // Example: Call ChooseNodeForTurret periodically or based on certain conditions
        if (ShouldPlaceTurret()) // You need to define this condition
        {
            Node chosenNode = ChooseNodeForTurret(nodes);
            PlaceTurretAtNode(chosenNode); // Implement this method based on how you handle turret placement
        }
    }

    Node ChooseNodeForTurret(Node[] nodes)
    {
        float totalWeight = 0;
        foreach (var node in nodes)
        {
            totalWeight += node.weight; // Make sure each node has a 'weight' property
        }

        float randomPoint = Random.value * totalWeight;
        float currentPoint = 0;

        foreach (var node in nodes)
        {
            currentPoint += node.weight;
            if (randomPoint <= currentPoint)
            {
                return node;
            }
        }

        return nodes[nodes.Length - 1]; // Fallback in case of rounding errors
    }

    void PlaceTurretAtNode(Node node)
    {
        if (node.nodeState() == 0 && playerStats.Money >= buildManager.turretToBuild.cost)
        {
            buildManager.BuildTurretOn(node);
        }
    }

    bool ShouldPlaceTurret()
    {
        // Implement the logic to decide when to place a turret
        // This could be based on time, game state, available resources, etc.
        return true; // Placeholder
    }
}
