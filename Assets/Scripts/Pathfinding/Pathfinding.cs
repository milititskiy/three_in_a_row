using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class Pathfinding : MonoBehaviour
{
    public float moveSpeed;  // units per second
    public List<Node> currentPath;
    public int currentPathIndex;
    public GameObject playerInstance;

    public List<Node> nodes;
    private bool isMouseButtonDown;

    public string quadType; // Type of the selected quad
    public List<GameObject> selectedQuads;
    private Node node;
    private LineRendererManager lineRendererManager;
    private Quad quad;

    private GridManager gridManager;

    private GameObject hoverObject;

    private Pathfinding pathfindingInstance;







    private void Awake()
    {
        InitializeVariables();

    }

    private void Start()
    {
        InitializeNodes();
        // lineRendererManager.lineRenderer = playerInstance.GetComponent<LineRenderer>();
        lineRendererManager.lineRenderer = GetComponent<LineRenderer>();
        lineRendererManager.lineRenderer.positionCount = 0;
    }

    private void InitializeVariables()
    {
        selectedQuads = new List<GameObject>();
        currentPath = new List<Node>();
        node = new Node();
        nodes = Node.nodes;
        currentPathIndex = 0;
        moveSpeed = 5.0f;
        isMouseButtonDown = false;
        GameObject gameObject = GameObject.FindGameObjectWithTag("GridQuad");
        quad = gameObject.GetComponent<Quad>();
        gridManager = GetComponent<GridManager>();
        pathfindingInstance = playerInstance.GetComponent<Pathfinding>();
        lineRendererManager = new LineRendererManager(pathfindingInstance, quad);
    }




    void InitializeNodes()
    {
        //  Node node = new Node();
        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                Vector3 nodePos = new Vector3(x, 0.5f, z);
                GetGridQuadAtPosition(nodePos);
                // Quad quad = GetGridQuadAtPosition(nodePos);

                bool isNodeWalkable = true;
                if (quad != null)
                {
                    isNodeWalkable = quad.IsWalkable;
                }
                nodes.Add(new Node(nodePos, isNodeWalkable, QuadType.Normal, gameObject));
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMouseButtonDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            MoveToLastSelectedQuad();
            isMouseButtonDown = false;
            ClearSelection();
        }

        if (isMouseButtonDown)
        {
            CheckMouseOverQuad();
            // CheckForMouseHover();
        }
        else if (!isMouseButtonDown && lineRendererManager.linePositions.Count > 0)
        {
            // Clear the line renderer when the mouse button is released
            lineRendererManager.ClearLineRenderer();
        }

    }

    private bool ArePositionsNeighbours(GameObject hoverObject)
    {
        float threshold = 1.6f;
        Vector3 playerPosition = playerInstance.transform.position;
        Vector3 mousePosition = hoverObject.transform.position;
        // Debug.Log(playerPosition + " playerPosition");
        float distance = Vector3.Distance(mousePosition, playerPosition);
        Vector3 difference = mousePosition - playerPosition;


        int x = (int)difference.x;
        int y = (int)difference.y;

        bool isNeighbour = false;

        switch (x)
        {
            case -1:
            case 0:
            case 1:
                switch (y)
                {
                    case -1:
                    case 0:
                    case 1:
                        isNeighbour = true;
                        // Debug.Log(" is Neighbour");
                        break;

                    default:
                        // Debug.Log(" not a Neighbour");
                        break;
                }
                break;

            default:
                // Debug.Log(" not a Neighbour");
                break;
        }

        return isNeighbour && distance < threshold;
    }





    public Quad GetGridQuadAtPosition(Vector3 position)
    {
        Vector3 rayStartPos = position + Vector3.up * 5.0f;  // Start ray above the position
        RaycastHit hit;

        if (Physics.Raycast(rayStartPos, Vector3.down, out hit, 10.0f)) // Cast ray downward
        {
            return hit.transform.GetComponent<Quad>();
        }

        return null; // No Quad found at the position
    }




    IEnumerator FollowPath()
    {
        if (lineRendererManager.lineRenderer)
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
                    if (lineRendererManager.lineRenderer)
                    {
                        lineRendererManager.lineRenderer.positionCount = 0;
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






    // Add this variable to keep track of the last highlighted object.



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



    private void MoveToLastSelectedQuad()
    {
        if (selectedQuads.Count > 0)
        {
            // Clear the current path
            currentPath.Clear();
            // Extract the positions from selectedQuads and add them to currentPath
            foreach (GameObject quad in selectedQuads)
            {
                currentPath.Add(new Node(quad.transform.position, true, QuadType.Normal, gameObject));
            }
            currentPathIndex = 0;
            if (selectedQuads.Count > 2)
            {
                StartCoroutine("FollowPath");
            }
            else
            {
                ClearSelection();
            }

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        isMouseButtonDown = true;

        if (Physics.Raycast(ray, out hit))
        {
            hoverObject = hit.collider.gameObject;
            Vector3 quadPosition = hoverObject.transform.position;
            GameObject preLastObject;

            if (hoverObject.CompareTag("GridQuad"))
            {

                string hitQuadType = hoverObject.GetComponent<Quad>().type.ToString();
                if (selectedQuads.Count == 0 || hitQuadType == quadType)
                {
                    quadType = hitQuadType;
                    if (!selectedQuads.Contains(hoverObject))
                    {
                        selectedQuads.Add(hoverObject);
                        if (ArePositionsNeighbours(selectedQuads[0]))
                        {
                            quad.HighlightQuad(hoverObject);
                        }
                        else
                        {
                            quad.DimQuad(selectedQuads[0]);
                            selectedQuads.Clear();
                        }
                    }
                    // Debug.Log(quadPosition + " position");
                    // lineRendererManager.AddToLinePositions(quadPosition);
                    lineRendererManager.SetPositions(selectedQuads);
                    int lastObjectIndex = selectedQuads.Count - 1;
                    int preLastObjectIndex = selectedQuads.Count - 2;
                    // GameObject lastObject = selectedQuads[lastObjectIndex];
                    if (selectedQuads.Count > 1)
                    {
                        preLastObject = selectedQuads[preLastObjectIndex];
                        if (hoverObject.transform.position == preLastObject.transform.position)
                        {
                            // Vector3 rendererPoint = hoverObject.transform.position;
                            GameObject go = selectedQuads[lastObjectIndex].gameObject;
                            Renderer renderer = go.GetComponent<Renderer>();
                            renderer.material.color = Color.white;
                            selectedQuads.RemoveAt(lastObjectIndex);
                        }
                    }
                }

            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            MoveToLastSelectedQuad();
        }
    }





}
