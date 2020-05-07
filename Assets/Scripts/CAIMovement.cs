using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CAIMovement : MonoBehaviour
{
    private IAstarAI ai;

    public bool followPlayer;
    private GameObject player;

    [SerializeField]
    private float maxHeight = 3f;
    [SerializeField]
    private float minHeight = 0.5f;


    private Transform caiMainBody;
    private float startHeight;
    private bool heightChangeActive = false;

    private Transform target;
    private Vector3 destination;// { private get; set; }
    public bool followTarget;

    private Transform lastInteractive = null;

    #region EnDisAble
    private void OnEnable()
    {
        ai = GetComponent<IAstarAI>();

        if (ai != null)
        {
            ai.onSearchPath += Update;
        }

        caiMainBody = transform.GetChild(0);
        startHeight = caiMainBody.position.y;
        player = FindObjectOfType<Player>().gameObject;
    }
    private void OnDisable()
    {
        if(ai != null)
        {
            ai.onSearchPath -= Update;
        }
    }
    #endregion EnDisAble
    private void Update()
    {
        if(ai != null)
        {
            //Timing to activate?
            if (followPlayer)
            {
                ai.destination = player.transform.position - (player.transform.forward * 1.3f); //should work correctly in 3rdPerson
            }
            else
            {
                ai.destination = followTarget ? target.position : destination;

            }
            if(GetRemainingDistanceHeightless() < 1f)
            {
                //To fast atm...
                Debug.Log("fix");
                if (!heightChangeActive)
                    StartCoroutine(CorrectHeight(startHeight));
            }
        }
    }
    public void StopMovement()
    {
        ai.isStopped = true;
        //stop velocity
        
    }
    public float GetRemainingDistance()
    {
        return Vector3.Distance(transform.position, followTarget ? target.position : destination);
    }
    public float GetRemainingDistanceHeightless()
    {
        Vector3 posi = transform.position;
        posi.y *= 0;
        Vector3 end = followTarget ? target.position : destination;
        end.y *= 0;
        return Vector3.Distance(posi,end);
    }

    private IEnumerator CorrectHeight(float height)
    {
        heightChangeActive = true;
        //still need a better way... Reset other Method?
        yield return new WaitForSeconds(1f);
        float start = caiMainBody.position.y;

        height = Mathf.Clamp(height, minHeight, maxHeight);
        float count = 0;

        while (count < 1)
        {
            caiMainBody.transform.position = Vector3.Lerp(new Vector3(caiMainBody.position.x, start, caiMainBody.position.z),
                new Vector3(caiMainBody.position.x,  height, caiMainBody.position.z),
                count);
            count += Time.deltaTime;
            yield return null;
        }

        //Vector3 endPoint = obj.transform.position.y > maxHeight ? new Vector3(obj.transform.position.x, maxHeight);
        
        heightChangeActive = false;
    }
    #region Setter
    public void SetDestination(Vector3 point)
    {
        destination = point;
        followTarget = false;
        Debug.Log("Destination Set");
        if (lastInteractive != null && lastInteractive.GetComponentInParent<MeshHighlighter>())
            lastInteractive.GetComponentInParent<MeshHighlighter>().CAIHighlight(false);
    }
    public void SetTarget(Transform targetpoint)
    {
        target = targetpoint;
        Debug.Log(targetpoint.name);

        if (lastInteractive != null && lastInteractive.GetComponentInParent<MeshHighlighter>())
            lastInteractive.GetComponentInParent<MeshHighlighter>().CAIHighlight(false);
        if(target.GetComponentInParent<MeshHighlighter>())
            target.GetComponentInParent<MeshHighlighter>().CAIHighlight(true);

        lastInteractive = target;
        followTarget = true;
        Debug.Log("Target Set");
        StartCoroutine(CorrectHeight(targetpoint.position.y));
    }
    #endregion


}
