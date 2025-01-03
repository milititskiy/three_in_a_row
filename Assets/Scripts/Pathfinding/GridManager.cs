using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject quadPrefab;
    public GameObject playerPrefab;
    private GameObject playerInstance;
    private static int gridSize = 10;

    public GameObject spritePrefab;
    public Sprite enemySprite;
    public Sprite coinSprite;
    public Sprite playerSprite;



    public Material swordMat;
    public Material flaskMat;
    public Material shieldMat;
    public Material unwalkableMat;
    public Material enemyMat;
    public Material coinMat;
    public Material playerMat;
    public Material clearQuadMat;
    //public Material wallMat;
    //public Material pitMat;
    //public Material doorMat;
    QuadType[,] gridArray = new QuadType[gridSize, gridSize];
    // private Pathfinding pathfinding = new Pathfinding();
    List<GameObject> instances = new List<GameObject>();

    private PathManager pathManager;
    private Quad quad;




    void Start()
    {
        quad = GetComponent<Quad>();

        GenerateGrid();
        // FindObjectAtGridPosition(0, 0);
        PlayerInstanceSet();


    }

    void GenerateGrid()
    {
        // QuadType[,] gridArray = new QuadType[gridSize, gridSize];
        GameObject grid = GameObject.FindGameObjectWithTag("Grid");
        Quaternion newRotation = grid.transform.rotation * Quaternion.Euler(0, 90, 0);
        grid.transform.rotation = newRotation;



        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                GameObject quadInstance = Instantiate(quadPrefab, new Vector3(x, z, 0), Quaternion.Euler(0, 0, 0));
                // GameObject spriteInstance = Instantiate(spritePrefab, new Vector3(x, z, 0), Quaternion.Euler(0, 0, 0));
                // instances.Add(quadInstance);
                // Quad quad = quadInstance.GetComponent<Quad>();
                QuadType quadType = QuadTypeManager.GetRandomQuadType();
                gridArray[x, z] = quadType;
                quadInstance.transform.SetParent(grid.transform);
                // switch (quadType)
                // {
                //     case QuadType.Sword:
                //         quadInstance.GetComponent<Renderer>().material = swordMat;
                //         Node.nodes.Add(new Node(new Vector3(x, z, 0.5f), true, QuadType.Sword, quadInstance));
                //         quad.type = QuadType.Sword;
                //         break;

                //     case QuadType.Flask:
                //         quadInstance.GetComponent<Renderer>().material = flaskMat;
                //         Node.nodes.Add(new Node(new Vector3(x, z, 0.5f), true, QuadType.Flask, quadInstance)); // Assuming Flask is walkable
                //         quad.type = QuadType.Flask;
                //         break;

                //     case QuadType.Shield:
                //         quadInstance.GetComponent<Renderer>().material = shieldMat;
                //         Node.nodes.Add(new Node(new Vector3(x, z, 0.5f), true, QuadType.Shield, quadInstance));
                //         quad.type = QuadType.Shield;
                //         break;
                //     case QuadType.Coin:
                //         quadInstance.GetComponent<Renderer>().material = coinMat;
                //         Node.nodes.Add(new Node(new Vector3(x, z, 0.5f), true, QuadType.Coin, quadInstance));
                //         // quad.type = QuadType.Coin;
                //         break;
                //     case QuadType.Enemy:
                //         quadInstance.GetComponent<Renderer>().material = enemyMat;
                //         Node.nodes.Add(new Node(new Vector3(x, z, 0.5f), false, QuadType.Enemy, quadInstance));
                //         quad.type = QuadType.Enemy;
                //         break;
                // case QuadType.Player:
                // quadInstance.GetComponent<Renderer>().material = playerMat;
                // Node.nodes.Add(new Node(new Vector3(x, 0.5f, z), false, QuadType.Player, playerInstance));
                // quad.type = QuadType.Player;
                // PlacePlayerToPosition(x, z);
                // break;

                //case QuadType.Wall:
                //    quadInstance.GetComponent<Renderer>().material = wallMat;
                //    Pathfinding.nodes.Add(new Node(new Vector3(x, 0.5f, z), false)); // Assuming Wall is not walkable
                //    break;

                //case QuadType.Pit:
                //    quadInstance.GetComponent<Renderer>().material = pitMat;
                //    Pathfinding.nodes.Add(new Node(new Vector3(x, 0.5f, z), false)); // Assuming Pit is not walkable
                //    break;

                //case QuadType.Door:
                //    quadInstance.GetComponent<Renderer>().material = doorMat;
                //    Pathfinding.nodes.Add(new Node(new Vector3(x, 0.5f, z), true)); // Assuming Door is walkable
                //    break;

                //default: // QuadType.Normal
                //    if (Random.value < 0.2f)
                //    {
                //        quadInstance.GetComponent<Renderer>().material = unwalkableMat;
                //        Pathfinding.nodes.Add(new Node(new Vector3(x, 0.5f, z), false));
                //    }
                //    else
                //    {
                //        Pathfinding.nodes.Add(new Node(new Vector3(x, 0.5f, z), true));
                //    }
                //    break;
                // }


            }

        }
        // QuadType quadType_2 = QuadTypeManager.GetRandomQuadType();
        // gridArray[9,9] = quadType_2;
        // Debug.Log(quadType_2 + " this is the type");
        // ClearAllQuads();

    }


    // void HighlightPlayerPrefab(GameObject playerInstance)
    // {
    //     HighlightPlayer highlightScript = playerInstance.AddComponent<HighlightPlayer>();
    //     highlightScript.playerTag = "Player"; // Set the tag of the player GameObject to be highlighted
    //     highlightScript.highlightColor = Color.red; // Set the highlight color to red
    // }

    void FindObjectAtGridPosition(int x, int z)
    {
        List<Node> nodes = Node.nodes;
        Pathfinding pathfinding = new Pathfinding();
        Pathfinding pathfindingScript = GetComponent<Pathfinding>();
        foreach (Node n in nodes)
        {
            Vector3 nodePosition = n.position;
            if (nodePosition.x == x && nodePosition.z == z)
            {
                Quad quad = pathfinding.GetGridQuadAtPosition(new Vector3(x, 0, z));
                GameObject gameObject_1 = quad.gameObject;
                Destroy(gameObject_1);
                playerInstance = Instantiate(playerPrefab, new Vector3(x, 0.2f, z), Quaternion.Euler(90, 0, 0));
                // HighlightPlayerPrefab(playerInstance);
                pathManager?.SetPlayerInstance(playerInstance);
            }
        }
    }

    void PlayerInstanceSet()
    {
        if (playerPrefab != null)
        {
            playerInstance = Instantiate(playerPrefab, Vector3.zero, Quaternion.Euler(0, 0, 0));
            // HighlightPlayerPrefab(playerInstance);
            pathManager?.SetPlayerInstance(playerInstance);
        }
        else
        {
            Debug.LogError("PlayerPrefab is null. Please assign a valid GameObject to playerPrefab.");
        }
    }

    void ClearAllQuads()
    {
        // for (int i = 0; i < gridSize; i++)
        // {
        //     for (int j = 0; j < gridSize; j++)
        //     {
        //         // Access and update elements in the 2D array
        //         gridArray[i, j] = QuadType.Normal;
        //     }
        // }
        GameObject parentObject = GameObject.FindGameObjectWithTag("Grid");
        if (parentObject != null)
        {
            Transform parentTransform = parentObject.transform;
            foreach (Transform child in parentTransform)
            {
                Renderer renderer = child.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = clearQuadMat;
                }
                Quad quad = child.GetComponent<Quad>();
                if (quad != null)
                {
                    Debug.Log(" qad");
                }
            }
        }

    }






}

