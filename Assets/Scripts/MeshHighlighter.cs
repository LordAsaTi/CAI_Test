using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshHighlighter : MonoBehaviour
{

    private MeshRenderer meshRenderer;
    private Material oldMat;
    [SerializeField]
    private Material highlightMat;

    private bool keepHighlighted;

    public void CAIHighlight(bool on)
    {
        EnableHighlight(on);
        keepHighlighted = on;
    }
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        oldMat = meshRenderer.material;
    }

    private void EnableHighlight(bool on)
    {
        if(highlightMat!= null)// && !keepHighlighted)
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
        if(!keepHighlighted)
            EnableHighlight(false);
    }
}
