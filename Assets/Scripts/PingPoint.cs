using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPoint : MonoBehaviour
{
    MeshRenderer mRenderer;

    private void Start()
    {
        mRenderer = GetComponent<MeshRenderer>();
    }
    public void SetPoint(Vector3 posi)
    {
        mRenderer.enabled = true;
        transform.position = posi;
        StopCoroutine(TurnInvisible());
        StartCoroutine(TurnInvisible());
    }

    private IEnumerator TurnInvisible()
    {
        yield return new WaitForSeconds(1f);
        mRenderer.enabled = false;
    }
}
