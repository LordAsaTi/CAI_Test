using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class DoorBehaviour : MonoBehaviour
{
	private bool open = false;

	public int opentag = 4;
	public int closedtag = 5;
	public bool updateGraphsWithGUO = true;
	public float yOffset = 5;

	Bounds bounds;

	public void Start()
	{
		// Capture the bounds of the collider while it is closed
		bounds = GetComponent<Collider>().bounds;

		// Initially open the door
		SetState(open);
	}
	public void SetState(bool open)
	{
		this.open = open;

		if (updateGraphsWithGUO)
		{
			// Update the graph below the door
			// Set the tag of the nodes below the door
			// To something indicating that the door is open or closed
			GraphUpdateObject guo = new GraphUpdateObject(bounds);
			int tag = open ? opentag : closedtag;

			// There are only 32 tags
			if (tag > 31) { Debug.LogError("tag > 31"); return; }

			guo.modifyTag = true;
			guo.setTag = tag;
			guo.updatePhysics = false;

			AstarPath.active.UpdateGraphs(guo);
		}

		// Play door animations
		if (open)
		{
			StartCoroutine(MoveDoorOpen());
		}
		else
		{
			//GetComponent<Animation>().Play("Close");
		}
	}


	IEnumerator MoveDoorOpen()
    {
        //Use Animaton is better
        Vector3 startPosi = transform.position;
        Vector3 endPosi = new Vector3(transform.position.x, -3, transform.position.z);
        float count = 0;
        while (count < 1)
        {
            transform.position = Vector3.Slerp(startPosi, endPosi, count);
            count += Time.deltaTime;
            yield return null;

        }
    }
}
