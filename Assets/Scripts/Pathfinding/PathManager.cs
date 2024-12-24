using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    List<Node> nodes;
    Node node;
    LineRendererManager lineRendererManager;
    Pathfinding pathfinding;
    Vector3 playerLocation;
    public void MoveTo(Vector3 targetPosition)
    {
        //Ensure nodes list is initialized and doesn't contain null elements.
        if (nodes == null || nodes.Contains(null))
        {
            Debug.LogError("Nodes list is either null or contains a null element.");
            return;
        }

        Node startNode = nodes.Find(n => n.position == transform.position);
        Node endNode = node.ClosestNodeTo(targetPosition);


        if (startNode == null)
        {
            Debug.LogError("Start node not found based on transform's position.");
            return;
        }

        if (lineRendererManager.lineRenderer)
        {
            lineRendererManager.lineRenderer.positionCount = 0;
        }

        // SetLinePoints(transform.position, targetPosition);

        if (endNode != null && endNode.walkable)  // Ensure that the end node is walkable
        {
            pathfinding.currentPath = FindPath(startNode.position, endNode.position);
            if (pathfinding.currentPath.Count > 0)  // If there's a valid path
            {
                pathfinding.currentPathIndex = 0;  // Start at the beginning of the path
                StopCoroutine("FollowPath");  // Stop the previous coroutine if it's running
                StartCoroutine("FollowPath");
            }
        }

    }

    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = nodes.Find(n => n.position == startPos);
        Node targetNode = nodes.Find(n => n.position == targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                   (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in currentNode.GetNeighbors())
            {
                if (closedSet.Contains(neighbor))
                    continue;

                int newMovementCost = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCost < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCost;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return new List<Node>();  // No path found
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs((int)nodeA.position.x - (int)nodeB.position.x);
        int dstZ = Mathf.Abs((int)nodeA.position.z - (int)nodeB.position.z);

        if (dstX > dstZ)
            return 14 * dstZ + 10 * (dstX - dstZ);
        return 14 * dstX + 10 * (dstZ - dstX);
    }

    public void SetPlayerInstance(GameObject player)
    {
        pathfinding.playerInstance = player;
        lineRendererManager.lineRenderer = pathfinding.playerInstance.GetComponent<LineRenderer>();
    }

    private void SetLinePoints(Vector3 start, Vector3 end)
    {
        lineRendererManager.lineRenderer.positionCount = 2;
        lineRendererManager.lineRenderer.SetPosition(0, start);
        lineRendererManager.lineRenderer.SetPosition(1, end);
    }



}
