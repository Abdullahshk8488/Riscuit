using UnityEngine;
using System.Collections.Generic;

public class AStarManager : MonoBehaviour
{
    public static AStarManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(Instance);
    }

    public List<Node> GeneratePath(Node start, Node end)
    {
        // Keep track of the current nodes
        List<Node> openSet = new List<Node>();

        // Looping through all nodes in the scene and we don't care about sorting
        foreach (Node n in NodesInScene())
        {
            n.gScore = float.MaxValue;
        }

        start.gScore = 0;
        start.hScore = Vector2.Distance(start.transform.position, end.transform.position);
        openSet.Add(start);

        while (openSet.Count > 0)
        {
            int lowestF = default;

            // Finding what node has the lowest FScore
            for (int i = 0; i < openSet.Count; ++i)
            {
                if (openSet[i].FScore() < openSet[lowestF].FScore())
                {
                    lowestF = i;
                }
            }

            Node currentNode = openSet[lowestF];
            openSet.Remove(currentNode);

            if (currentNode == end)
            {
                // If current node is at the end, we are returning the path
                List<Node> path = new List<Node>();

                // Backtracking to create the path from the end
                path.Insert(0, end);

                while (currentNode != start)
                {
                    currentNode = currentNode.cameFrom;
                    path.Add(currentNode);
                }

                path.Reverse();
                return path;
            }

            // Neighbor check
            foreach(Node connectedNode in currentNode.connections)
            {
                // To compare the gScores of the connected node and the current node
                float heldGScore = currentNode.gScore + Vector2.Distance(currentNode.transform.position, connectedNode.transform.position);

                if (heldGScore < connectedNode.gScore)
                {
                    // Updating the values
                    connectedNode.cameFrom = currentNode;
                    connectedNode.gScore = heldGScore;
                    connectedNode.hScore = Vector2.Distance(connectedNode.transform.position, end.transform.position);

                    // Checks if the open set contains the connected node
                    if (!openSet.Contains(connectedNode))
                    {
                        openSet.Add(connectedNode);
                    }
                }
            }
        }

        return null;
    }

    public Node FindNearestNode(Vector2 position)
    {
        Node foundNode = null;
        float minDistance = float.MaxValue;

        foreach (Node node in NodesInScene())
        {
            float currentDistance = Vector2.Distance(position, node.transform.position);
            if (currentDistance <  minDistance)
            {
                minDistance = currentDistance;
                foundNode = node;
            }
        }

        return foundNode;
    }

    public Node FindFurthestNode(Vector2 position)
    {
        Node foundNode = null;
        float maxDistance = 0;

        foreach (Node node in NodesInScene())
        {
            float currentDistance = Vector2.Distance(position, node.transform.position);
            if (currentDistance > maxDistance)
            {
                maxDistance = currentDistance;
                foundNode = node;
            }
        }

        return foundNode;
    }

    public Node[] NodesInScene()
    {
        return FindObjectsByType<Node>(FindObjectsSortMode.None);
    }
}
