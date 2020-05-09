using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField]
    Transform interactionPoint =  null;

    public virtual Transform GetInteractionPoint()
    {
        return interactionPoint != null ? interactionPoint : this.transform;
    }
}
