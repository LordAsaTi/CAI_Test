using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshHighlighter : MonoBehaviour
{

    private MeshRenderer meshRenderer;
    private Material oldMat;
    [SerializeField]
    private Material highlightMat;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        oldMat = meshRenderer.material;
    }

    public void EnableHighlight(bool on)
    {
        if(highlightMat!= null)
        {
            meshRenderer.material = on ? highlightMat : oldMat;
        }
    }

    private void OnMouseEnter()
    {
        EnableHighlight(true);
    }
    private void OnMouseExit()
    {
        EnableHighlight(false);
    }
}
