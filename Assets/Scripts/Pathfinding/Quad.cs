using System.Collections.Generic;
using UnityEngine;

public class Quad : MonoBehaviour
{
    //public Material walkableMat;
    //public Material unwalkableMat;
    //public bool isWalkable;
    public Material originalMaterial;
    public Material highlightMaterial;
    private MeshRenderer meshRenderer;

    public QuadType type;




    public bool IsWalkable { get; private set; }




    public void SetHighlightMaterial()
    {
        if (meshRenderer && highlightMaterial)
        {
            meshRenderer.material = highlightMaterial;
        }
    }

    public void ResetOriginalMaterial()
    {
        if (meshRenderer && originalMaterial)
        {
            meshRenderer.material = originalMaterial;
        }
    }

    public void HighlightQuad(GameObject quad)
    {
        Renderer quadRenderer = quad.GetComponent<Renderer>();
        if (quadRenderer != null)
        {
            // Set your highlight material or change the shader properties here.
            // For example, you can change the material color.
            quadRenderer.material.color = Color.green;
        }
    }

    public void DimQuad(GameObject quad)
    {
        Renderer quadRenderer = quad.GetComponent<Renderer>();
        if (quadRenderer != null)
        {
            // Set your highlight material or change the shader properties here.
            // For example, you can change the material color.
            quadRenderer.material.color = Color.white;
        }
    }

    // public Quad GetGridQuadAtPosition(Vector3 position)
    // {
    //     Vector3 rayStartPos = position + Vector3.up * 5.0f;  // Start ray above the position
    //     RaycastHit hit;

    //     if (Physics.Raycast(rayStartPos, Vector3.down, out hit, 10.0f)) // Cast ray downward
    //     {
    //         return hit.transform.GetComponent<Quad>();
    //     }

    //     return null; // No Quad found at the position
    // }





}

