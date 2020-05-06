using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PowerBoxInteractable : Interactable,IInteractable
{
    public DoorBehaviour doorBehaviour;

    public void Interact()
    {
        Debug.Log("Start");
        doorBehaviour.SetState(true);
    }

    
}
