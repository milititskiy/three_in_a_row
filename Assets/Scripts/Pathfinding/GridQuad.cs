using UnityEngine;

public class GridQuad : MonoBehaviour
{
    //public Material walkableMat;
    //public Material unwalkableMat;
    public bool isWalkable;
    public Material originalMaterial;
    public Material highlightMaterial;

    private MeshRenderer meshRenderer;


    public bool IsWalkable { get; private set; }

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalMaterial = meshRenderer.material;
    }

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



    
}

