using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Copy_Pathfinding : MonoBehaviour
{
    public float moveSpeed = 5.0f;  // units per second
    List<Node> currentPath = new List<Node>();
    int currentPathIndex = 0;
    public GameObject playerInstance;
    private LineRenderer lineRenderer;

    public static List<Node> nodes = new List<Node>();

    public GameObject redCubePrefab;
    private bool isMouseButtonDown = false;

    public string quadType; // Type of the selected quad
    public List<GameObject> selectedQuads = new List<GameObject>();
    private List<Vector3> linePositions = new List<Vector3>();

   






    private void Start()
    {
        InitializeNodes();
        lineRenderer = playerInstance.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }



    void InitializeNodes()
    {   
        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                Vector3 nodePos = new Vector3(x, 0.5f, z);
                Quad quad = GetGridQuadAtPosition(nodePos);

                bool isNodeWalkable = true;
                if (quad != null)
                {
                    isNodeWalkable = quad.IsWalkable;
                }
                nodes.Add(new Node(nodePos, isNodeWalkable, QuadType.Normal,gameObject));
            }
        }
    }

    private void Update()
    {
        //CheckForMouseHover();
        // CheckForMouseClick();
        Light();

        if (Input.GetMouseButtonDown(0))
        {
            isMouseButtonDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMouseButtonDown = false;
            ClearSelection();
        }

        if (isMouseButtonDown)
        {
            CheckMouseOverQuad();
        }
        else if (!isMouseButtonDown && linePositions.Count > 0)
        {
            // Clear the line renderer when the mouse button is released
            ClearLineRenderer();
        }

    }
    


    void CheckForMouseClick()
    {
        if (Input.GetMouseButtonDown(1))  // 0 = left mouse button
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("GridQuad"))  // Make sure you set the tag for your quads!
                {
                    //SetMoveTarget(hit.point);
                    MoveTo(hit.point);
                }
            }
        }
    }

    Quad GetGridQuadAtPosition(Vector3 position)
    {
        Vector3 rayStartPos = position + Vector3.up * 5.0f;  // Start ray above the position
        RaycastHit hit;

        if (Physics.Raycast(rayStartPos, Vector3.down, out hit, 10.0f)) // Cast ray downward
        {
            return hit.transform.GetComponent<Quad>();
        }

        return null; // No Quad found at the position
    }





    public void MoveTo(Vector3 targetPosition)
    {
        //Ensure nodes list is initialized and doesn't contain null elements.
        if (nodes == null || nodes.Contains(null))
        {
            Debug.LogError("Nodes list is either null or contains a null element.");
            return;
        }

        Node startNode = nodes.Find(n => n.position == transform.position);
        Node endNode = ClosestNodeTo(targetPosition);


        if (startNode == null)
        {
            Debug.LogError("Start node not found based on transform's position.");
            return;
        }

        if (lineRenderer)
        {
            lineRenderer.positionCount = 0;
        }

        // SetLinePoints(transform.position, targetPosition);

        if (endNode != null && endNode.walkable)  // Ensure that the end node is walkable
        {
            currentPath = FindPath(startNode.position, endNode.position);
            if (currentPath.Count > 0)  // If there's a valid path
            {
                currentPathIndex = 0;  // Start at the beginning of the path
                StopCoroutine("FollowPath");  // Stop the previous coroutine if it's running
                StartCoroutine("FollowPath");
            }
        }

    }

    IEnumerator FollowPath()
    {
        if (lineRenderer)
        {
            // SetCurvedPath(currentPath);
        }
        Vector3 currentWaypoint = currentPath[currentPathIndex].position;
        //lineRenderer.enabled = true;
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                currentPathIndex++;
                if (currentPathIndex >= currentPath.Count)
                {
                    if (lineRenderer)
                    {
                        lineRenderer.positionCount = 0;
                    }
                    //lineRenderer.enabled = false;
                    yield break;  // Exit the coroutine
                }

                currentWaypoint = currentPath[currentPathIndex].position;

            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);
            yield return null;  // Wait for the next frame

        }

    }

    Node ClosestNodeTo(Vector3 target)
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
        playerInstance = player;
        lineRenderer = playerInstance.GetComponent<LineRenderer>();
    }

    private void SetLinePoints(Vector3 start, Vector3 end)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }


    private Vector3 QuadraticBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        // B(t) = (1 - t) * ((1 - t) * p0 + t * p1) + t * ((1 - t) * p1 + t * p2)
        return (1 - t) * ((1 - t) * p0 + t * p1) + t * ((1 - t) * p1 + t * p2);
    }

    private void SetCurvedPath(List<Node> path)
    {
        if (path.Count < 2 || !lineRenderer)
            return;

        List<Vector3> bezierPath = new List<Vector3>();
        bezierPath.Add(path[0].position);

        for (int i = 0; i < path.Count - 2; i += 2)
        {
            Vector3 startPoint = path[i].position;
            Vector3 middlePoint = path[i + 1].position;
            Vector3 endPoint = path[i + 2].position;

            for (float t = 0; t < 1; t += 0.1f)
            {
                bezierPath.Add(QuadraticBezierCurve(startPoint, middlePoint, endPoint, t));
            }
        }
        bezierPath.Add(path[path.Count - 1].position);

        lineRenderer.positionCount = bezierPath.Count;
        lineRenderer.SetPositions(bezierPath.ToArray());
    }

    private GameObject lastHighlightedObject; // Add this variable to keep track of the last highlighted object.

    void CheckForMouseHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("GridQuad"))
            {
                Node hoveredNode = nodes.Find(n => n.position == hit.point);
                GameObject go = hit.transform.gameObject;
                QuadType type = go.GetComponent<Quad>().type;

                // Check if the hovered object is of the same type as the last highlighted object.
                if (lastHighlightedObject != null && lastHighlightedObject.GetComponent<Quad>().type != type)
                {
                    // Reset the last highlighted object to its default color.
                    ResetObjectColor(lastHighlightedObject);
                }

                // Highlight the current hovered object.
                HighlightObject(go);

                // Update the last highlighted object.
                lastHighlightedObject = go;
            }
        }
        else
        {
            // No object under the mouse pointer, reset the last highlighted object and all objects to their default color.
            if (lastHighlightedObject != null)
            {
                ResetObjectColor(lastHighlightedObject);
                lastHighlightedObject = null;
            }
            ResetAllObjectsColor();
        }
    }

    void ResetObjectColor(GameObject obj)
    {
        obj.GetComponent<Renderer>().material.color = Color.white; // Change this to whatever the default color is.
    }

    void ResetAllObjectsColor()
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("GridQuad");

        foreach (GameObject obj in allObjects)
        {
            ResetObjectColor(obj);
        }
    }

    void HighlightObject(GameObject obj)
    {
        obj.GetComponent<Renderer>().material.color = Color.green; // Change this to the highlight color you want.
    }




    void HighlightNode(Node node)
    {
        Quad quad = GetGridQuadAtPosition(node.position);
        if (quad)
        {
            // Assuming you've a function in Quad to set a highlight shader/material.
            quad.SetHighlightMaterial();
        }
    }

    void ResetNodeHighlight(Node node)
    {
        Quad quad = GetGridQuadAtPosition(node.position);
        if (quad)
        {
            // Assuming you've a function in Quad to reset to the original material.
            quad.ResetOriginalMaterial();
        }
    }



    private void Light()
    {

        if (Input.GetMouseButtonDown(0))
        {
            isMouseButtonDown = true;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;

                if (hitObject.CompareTag("GridQuad"))
                {
                    string hitQuadType = hitObject.GetComponent<Quad>().type.ToString(); // Assuming you have a script that holds the quad type
                    if (selectedQuads.Count == 0 || hitQuadType == quadType)
                    {
                        quadType = hitQuadType;
                        selectedQuads.Add(hitObject);
                        HighlightQuad(hitObject);
                    }
                    else
                    {
                        ClearSelection();
                    }
                }
                
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            MoveToLastSelectedQuad();
            // Move to the last selected quad.
            if (selectedQuads.Count > 0)
            {
                Vector3 lastQuadPosition = selectedQuads[selectedQuads.Count - 1].transform.position;
                DrawLineFromPlayerToPosition(lastQuadPosition);
            }

        }
        
    }

    private void DrawLineFromPlayerToPosition(Vector3 targetPosition)
    {
       
         if (lineRenderer != null && selectedQuads != null && selectedQuads.Count > 0)
    {
        lineRenderer.positionCount = selectedQuads.Count + 1; // +1 for the player's position

        // Set the first position to the player's position
        lineRenderer.SetPosition(0, playerInstance.transform.position);

        // Set positions from 1 to n (where n is the number of positions in the list)
        for (int i = 0; i < selectedQuads.Count; i++)
        {
            lineRenderer.SetPosition(i + 1, selectedQuads[i].transform.position);
        }
    }
    }

    private void MoveToLastSelectedQuad()
    {   
        if (selectedQuads.Count > 0)
        {
            // Clear the current path
            currentPath.Clear();

            // Extract the positions from selectedQuads and add them to currentPath
            foreach (GameObject quad in selectedQuads)
            {
                currentPath.Add(new Node(quad.transform.position, true, QuadType.Normal,gameObject));
            }

            // Start moving to the positions in currentPath
            currentPathIndex = 0;
            StartCoroutine("FollowPath");
        }
    }






    private void HighlightQuad(GameObject quad)
    {
        Renderer quadRenderer = quad.GetComponent<Renderer>();
        if (quadRenderer != null)
        {
            // Set your highlight material or change the shader properties here.
            // For example, you can change the material color.
            quadRenderer.material.color = Color.green;
        }
    }

    private void ClearSelection()
    {
        foreach (GameObject selectedQuad in selectedQuads)
        {
            Renderer quadRenderer = selectedQuad.GetComponent<Renderer>();
            if (quadRenderer != null)
            {
                // Reset the material or shader properties to their original values.
                quadRenderer.material.color = Color.white;
            }
        }
        selectedQuads.Clear();
    }

    private void CheckMouseOverQuad()
    {
        // RaycastHit hit;
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // if (Physics.Raycast(ray, out hit))
        // {
        //     GameObject hitObject = hit.collider.gameObject;

        //     if (hitObject.CompareTag("GridQuad"))
        //     {
        //         string hitQuadType = hitObject.GetComponent<Quad>().type.ToString(); // Assuming you have a script that holds the quad type
        //         if (selectedQuads.Count == 0 || hitQuadType == quadType)
        //         {
        //             quadType = hitQuadType;
        //             if (!selectedQuads.Contains(hitObject))
        //             {
        //                 selectedQuads.Add(hitObject);
        //                 HighlightQuad(hitObject);
        //             }
        //         }
        //     }
        // }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("GridQuad"))
            {
                 string hitQuadType = hitObject.GetComponent<Quad>().type.ToString(); 
                if (selectedQuads.Count == 0 || hitQuadType == quadType)
                {
                    quadType = hitQuadType;
                    if (!selectedQuads.Contains(hitObject))
                    {
                        selectedQuads.Add(hitObject);
                        HighlightQuad(hitObject);
                    }
                }
                // Add the position of the hovered quad to the line positions
                Vector3 quadPosition = hitObject.transform.position;
                AddToLinePositions(quadPosition);
            }
        }

        
    }

    private void AddToLinePositions(Vector3 position)
    {
        linePositions.Add(position);

        // Update the line renderer with the new positions
        lineRenderer.positionCount = linePositions.Count;
        lineRenderer.SetPositions(linePositions.ToArray());
    }

    private void ClearLineRenderer()
    {
        // Clear the line renderer
        linePositions.Clear();
        lineRenderer.positionCount = 0;
    }

}
