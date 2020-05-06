using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.AI;

public class CAIInteract : MonoBehaviour
{
    Vector3 rayOrigin = new Vector3(0.5f, 0.5f, 0f);//center of Screen
    float rayLength = 500f;
    // NavMeshAgent agent;
    CAIMovement movement;
    public GameObject pickUp;
    private Transform caiMainBody;

    [SerializeField]
    private float maxHeight = 2f;
    [SerializeField]
    private float minHeight = 1f;
    [SerializeField]
    private float stoppingDistance = 1f;
    [SerializeField]
    GameObject pingPoint = null;

    private void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        movement = GetComponent<CAIMovement>();
        caiMainBody = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ViewportPointToRay(rayOrigin);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, rayLength))
            {
                pingPoint.transform.position = hit.point;
                Ping(hit.point, hit.collider.gameObject);
            }
        }
    }
    private void Ping(Vector3 point, GameObject obj)
    {
        if (pickUp)
        {
            movement.SetDestination(point);
            StartCoroutine(InteractWithWhile(obj, point));
        }
        else
        {
            PingType type; 
            if (obj.CompareTag("Interactable"))
            {
                type = PingType.Interact;
            }
            else if (obj.CompareTag("Enemy"))
            {
                type = PingType.Enemy;
            }
            else
            {
                type = PingType.GoPoint;
            }
            switch (type)
            {
                case PingType.GoPoint:
                    movement.SetDestination(point);
                    //agent.SetDestination(point);
                    break;
                case PingType.Interact:
                    //All Shitty ATM
                    movement.SetTarget(obj.GetComponent<Interactable>().GetInteractionPoint());
                    StartCoroutine(CorrectHeight(obj.GetComponent<Interactable>().GetInteractionPoint()));
                    //agent.SetDestination(point);
                    Debug.Log("Interact with " + obj.name);
                    StartCoroutine(InteractWith(obj));
                    break;
                case PingType.Enemy:
                    movement.SetTarget(obj.transform);
                    break;
            }

        }

    }
    IEnumerator EnemyInteract(GameObject enemy)
    {
        yield return null;
        while(movement.GetRemainingDistance() > stoppingDistance)
        {
            yield return null;
        }
        enemy.GetComponent<EnemyAStarPatrol>().Schock();
    }
    IEnumerator InteractWith(GameObject obj)
    {
        yield return null;//Destination Update wait
        while(movement.GetRemainingDistance() > stoppingDistance +3)
        {
            yield return null;
        }
        obj.GetComponent<IInteractable>().Interact();
    }
    IEnumerator InteractWithWhile(GameObject obj, Vector3 point)
    {
        yield return null;
        while (movement.GetRemainingDistance() > stoppingDistance)
        {
            yield return null;
        }
        movement.StopMovement();
        if (pickUp)
        {
            pickUp.transform.SetParent(null);
            pickUp.transform.position = point;
            pickUp = null;
        }
    }
    IEnumerator CorrectHeight(Transform point)
    {
        float start = caiMainBody.position.y;

        float count = 0;

        while(count < 1)
        {
            caiMainBody.transform.position = Vector3.Lerp(new Vector3(caiMainBody.position.x, start, caiMainBody.position.z),
                new Vector3(caiMainBody.position.x, point.position.y, caiMainBody.position.z),
                count);
            count += Time.deltaTime;
            yield return null;
        }

        //Vector3 endPoint = obj.transform.position.y > maxHeight ? new Vector3(obj.transform.position.x, maxHeight);
        yield return null;
    }

}
public enum PingType
{
    GoPoint,
    Interact,
    Enemy
}
