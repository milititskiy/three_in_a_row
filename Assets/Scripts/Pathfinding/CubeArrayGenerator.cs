using UnityEngine;

public class CubeArrayGenerator : MonoBehaviour
{
    public GameObject cubePrefab;
    public int gridSize = 5; // Adjust as needed
    public float spacing = 1.0f; // Adjust as needed
    public float radius = 5.0f;

    public int triangleSize = 5;
    public int crossSize = 5; 
    public int numPoints = 5;

    void Start()
    {
        // GenerateCubeArray();
        // GeneratTriangle();
        // GenerateCross();
    }
    

    void GenerateCubeArray()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (x == gridSize / 2 || y == gridSize / 2)
                {
                    // Create a cube at the cross intersection
                    Vector3 position = new Vector3(x * spacing, 0, y * spacing);
                    Instantiate(cubePrefab, position, Quaternion.identity);
                }
            }
        }
    }

    void GeneratTriangle()
    {
        for (int row = 0; row < triangleSize; row++)
        {
            for (int col = 0; col <= row; col++)
            {
                float x = col * spacing;
                float y = row * spacing;
                Vector3 position = new Vector3(x, 0, y);
                Instantiate(cubePrefab, position, Quaternion.identity);
            }
        }
    }

    void GenerateCross()
    {
       if (numPoints < 5)
        {
            Debug.LogWarning("A star must have at least 5 points.");
            return;
        }

        for (int i = 0; i < numPoints; i++)
        {
            float angle = (360f / numPoints) * i;
            Vector3 position = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * radius, 0, Mathf.Sin(Mathf.Deg2Rad * angle) * radius);
            Instantiate(cubePrefab, position, Quaternion.identity);
        }
    }

    


}
    
    

