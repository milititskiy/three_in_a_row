using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererManager
{
    public LineRenderer lineRenderer;
    public List<Vector3> linePositions = new List<Vector3>();
    private Pathfinding pathfinding;
    private Quad quad;


    public LineRendererManager(Pathfinding pathfinding, Quad quad)
    {
        this.pathfinding = pathfinding;
        this.quad = quad;
    }


    public void AddToLinePositions(Vector3 position)
    {
        pathfinding.GetGridQuadAtPosition(position);
        QuadType type = quad.type;
        if (type != QuadType.Enemy)
        {
            linePositions.Add(position);
        }
        UpdateLineRenderer();
    }

    public void RemoveFromLinePositions(Vector3 position)
    {
        pathfinding.GetGridQuadAtPosition(position);
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

    public void SetPositions(List<GameObject> selectedElements)
    {
        this.lineRenderer.positionCount = selectedElements.Count;
        for (int i = 0; i < selectedElements.Count; i++)
        {
            this.lineRenderer.SetPosition(i, selectedElements[i].transform.position);
        }
    }
}
