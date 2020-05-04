using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CAIMovement : MonoBehaviour
{
    private IAstarAI ai;

    private Transform target;
    private Vector3 destination;// { private get; set; }
    public bool followTarget;

    #region EnDisAble
    private void OnEnable()
    {
        ai = GetComponent<IAstarAI>();

        if(ai != null)
        {
            ai.onSearchPath += Update;
        }
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
            ai.destination = followTarget ? target.position : destination;
            
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
    #region Setter
    public void SetDestination(Vector3 point)
    {
        destination = point;
        followTarget = false;
        Debug.Log("Destination Set");
    }
    public void SetTarget(Transform targetpoint)
    {
        target = targetpoint;
        followTarget = true;
        Debug.Log("Target Set");
    }
    #endregion


}
