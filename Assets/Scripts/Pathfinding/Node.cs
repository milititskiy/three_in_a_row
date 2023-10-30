using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node
{
    public Vector3 position;
    public bool walkable;
    public Node parent; // For path reconstruction
    public int gCost; // Cost from the start node to this node
    public int hCost; // Heuristic cost from this node to the end node
    public int fCost => gCost + hCost; // Total cost
    public QuadType quadType;
    public static List<Node> nodes = new List<Node>();
    public static GameObject gameObject;

    public Node(Vector3 _position, bool _walkable, QuadType _quadType, GameObject _gameObject)
    {
        position = _position;
        walkable = _walkable;
        quadType = _quadType;
        gameObject = _gameObject;
    }

    public Node()
    {
    }

    public List<Node> GetNeighbors()
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0)
                    continue; // Skip the current node itself

                int checkX = (int)position.x + x;
                int checkZ = (int)position.z + z;

                if (checkX >= 0 && checkX < 10 && checkZ >= 0 && checkZ < 10) // Ensure within grid bounds
                {
                    Node neighbor = nodes.Find(n => n.position == new Vector3(checkX, 0.5f, checkZ));
                    if (neighbor != null && neighbor.walkable)
                    {
                        neighbors.Add(neighbor);
                    }

                }
            }
        }



        return neighbors;
    }

    public Node ClosestNodeTo(Vector3 target)
    {
        float minDistance = float.MaxValue;
        Node closest = null;

        foreach (var node in nodes)
        {
            if (!node.walkable)
                continue;  // skip unwalkable nodes

            float distance = Vector3.Distance(target, node.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = node;
            }
        }
        return closest;
    }
}
