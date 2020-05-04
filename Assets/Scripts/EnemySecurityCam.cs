using System.Collections;
using UnityEngine;

public class EnemySecurityCam : MonoBehaviour
{
    [SerializeField]
    public Transform player;
    [SerializeField]
    private float maxAngle = 45f;
    [SerializeField]
    private float maxRadius = 10f;
    [SerializeField]
    private float maxLookAngle = 40;
    private bool isInFOV;

    public bool IsInFOV { get => isInFOV; private set => isInFOV = value; }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRadius);

        //Rotate forward along Y
        Vector3 fovline1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
        Vector3 fovline2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovline1);
        Gizmos.DrawRay(transform.position, fovline2);


        if (!IsInFOV)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;
        if(player != null)
            Gizmos.DrawRay(transform.position, (player.position - transform.position).normalized * maxRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * maxRadius);
    }

    public static bool InFOV(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
    {
        Collider[] overlaps = new Collider[10];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

        for (int i = 0; i < count; i++)
        {
            if (overlaps[i] != null)
            {
                if (overlaps[i].transform == target)
                {
                    Vector3 directionBetween = (target.position - checkingObject.position).normalized;
                    //directionBetween.y *= 0;

                    float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                    if (angle <= maxAngle)
                    {
                        Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, maxRadius))
                        {
                            if (hit.transform == target)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
    private void Update()
    {
        IsInFOV = InFOV(transform, player, maxAngle, maxRadius);
        if (isInFOV)
        {
            Debug.Log("I See You");
        }
    }

    private void Start()
    {
        if(player == null)
        {
            //Change to your script
            player = FindObjectOfType<Player>().transform;
        }
        StartCoroutine(LookAround());
    }
    private IEnumerator LookAround()
    {
       
        // Alternativ start at one LookDirection
        while (true)
        {
            float count = 0;
            Quaternion rightLook = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, maxLookAngle, 0));
            Quaternion leftLook = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, -maxLookAngle, 0));
            Quaternion startRot = transform.rotation;

            while (count < 1)
            {
                transform.rotation = Quaternion.Lerp(startRot, rightLook, count);
                count += Time.deltaTime;
                yield return null;

            }
            count = 0;
            while (count < 1)
            {
                transform.rotation = Quaternion.Lerp(rightLook, leftLook, count);
                count += Time.deltaTime / 2; //Doubled distance
                yield return null;

            }
            count = 0;
            while (count < 1)
            {
                transform.rotation = Quaternion.Lerp(leftLook, startRot, count);
                count += Time.deltaTime;
                yield return null;

            }
            yield return null;
        }


    }
}
