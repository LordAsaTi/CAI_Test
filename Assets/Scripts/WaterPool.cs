using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPool : MonoBehaviour,IInteractable
{
    //Gameobject for flexiblility
    public List<GameObject> insideList; 

    private void Awake()
    {
        insideList = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<EnemyAStarPatrol>())
        {
            insideList.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (insideList.Contains(other.gameObject))
            insideList.Remove(other.gameObject);
    }
    public void Interact()
    {
        foreach (var item in insideList)
        {
            if (item.GetComponentInParent<EnemyAStarPatrol>())
                item.GetComponentInParent<EnemyAStarPatrol>().Schock();
        }
    }

    
}
