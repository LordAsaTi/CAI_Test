using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CAIMovement : MonoBehaviour
{
    private IAstarAI ai;

    private bool followPlayer;
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
                //ignoring Height ATM
                ai.destination = followTarget ? new Vector3(target.position.x,ai.position.y,target.position.z ): destination;

            }
            if(ai.reachedDestination)
            {
                //To fast atm...
                /*
                Debug.Log("fix");
                if (!heightChangeActive)
                    StartCoroutine(ResetHeight());
                    */
            }
        }
    }
    public void StopMovement()
    {
        ai.isStopped = true;
        //stop velocity
        
    }
    public bool ReachedDestination()
    {
        return ai.reachedDestination;
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

    private IEnumerator ResetHeight()
    {
        heightChangeActive = true;
        //yield return new WaitForSeconds(3f);
        float count = 0;
        float start = caiMainBody.position.y;

        while (count < 1)
        {
            caiMainBody.transform.position = Vector3.Lerp(new Vector3(caiMainBody.position.x, start, caiMainBody.position.z),
                new Vector3(caiMainBody.position.x, startHeight, caiMainBody.position.z),
                count);
            count += Time.deltaTime;
            yield return null;
        }
        heightChangeActive = false;
    }
    private IEnumerator CorrectHeight(float height)
    {
        heightChangeActive = true;

        //GetTime to get Position - GetTime to get to height;
        while(GetRemainingDistanceHeightless() > 5)
        {
            yield return null;
        }
        //still need a better way... Reset other Method?
        //yield return new WaitForSeconds(1f);
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
        StartCoroutine(ResetHeight());
    }
    public void SetTarget(Transform targetpoint)
    {
        target = targetpoint;

        //Interaction Point is a child of the Interactable
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
    public void FollowPlayer(bool follow)
    {
        followPlayer = follow;
        if (follow)
        {
            if (lastInteractive != null && lastInteractive.GetComponentInParent<MeshHighlighter>())
                lastInteractive.GetComponentInParent<MeshHighlighter>().CAIHighlight(false);
            StartCoroutine(CorrectHeight(player.transform.position.y));
        }
    }

}
