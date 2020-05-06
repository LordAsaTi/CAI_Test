using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ButtonInteractable : Interactable, IInteractable
{
    CAIInteract cai;
    private void Start()
    {
        cai = FindObjectOfType<CAIInteract>();
    }

    public void Interact()
    {
        Debug.Log("Domo, Kazuma desu");
        transform.SetParent(cai.transform);
        transform.position = cai.transform.GetChild(0).position + Vector3.up;
        cai.pickUp = this.gameObject;
    }

}
