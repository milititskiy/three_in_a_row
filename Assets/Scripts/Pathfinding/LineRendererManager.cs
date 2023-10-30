using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererManager
{
    public LineRenderer lineRenderer;
    public List<Vector3> linePositions = new List<Vector3>();
    private Pathfinding pathfinding = new Pathfinding();

    public void AddToLinePositions(Vector3 position)
    {
        Quad quad = pathfinding.GetGridQuadAtPosition(position);
        QuadType type = quad.type;
        if (type != QuadType.Enemy)
        {
            linePositions.Add(position);
        }
        UpdateLineRenderer();
    }

    public void RemoveFromLinePositions(Vector3 position)
    {
        Quad quad = pathfinding.GetGridQuadAtPosition(position);
        QuadType type = quad.type;
        if (type != QuadType.Enemy)
        {
            linePositions.Remove(position);
        }
        UpdateLineRenderer();
    }


    public void RemovePosition(int index)
    {
        int newPositionCount = lineRenderer.positionCount - 1;
        Vector3[] newPositions = new Vector3[newPositionCount];

        for (int i = 0, j = 0; i < lineRenderer.positionCount; i++)
        {
            if (i != index)
            {
                newPositions[j] = lineRenderer.GetPosition(i);
                j++;
            }
        }

        lineRenderer.positionCount = newPositionCount;
        lineRenderer.SetPositions(newPositions);
    }

    public void ClearLineRenderer()
    {
        // Clear the line renderer
        linePositions.Clear();
        lineRenderer.positionCount = 0;
    }

    private void UpdateLineRenderer()
    {
        lineRenderer.positionCount = linePositions.Count;
        lineRenderer.SetPositions(linePositions.ToArray());
    }

}
