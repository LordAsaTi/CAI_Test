using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;

public class EnemyAStarPatrol : MonoBehaviour
{
	[SerializeField]
    private Transform[] waypoints = new Transform[0];
	[SerializeField]
    private int index = 0;
	[SerializeField]
	private bool lookAround = true;
	private bool playerSeen;
	private bool lookIsRunning;
	private bool schocked;

    private IAstarAI ai;
	private FOVDetection fOVDetection;
	private void OnEnable()
	{
		ai = GetComponent<IAstarAI>();
		fOVDetection = GetComponent<FOVDetection>();

		if (ai != null)
		{
			if (lookAround)
			{
				LookAroundStart();
				ai.onSearchPath += UpdateAI;
			}
			
			ai.onSearchPath += Update;
		}
		
	}

	private void OnDisable()
	{
		if (ai != null)
		{
			if (lookAround)
			{
				StopCoroutine(LookAround());
				ai.onSearchPath -= UpdateAI;
				lookIsRunning = false;
			}
			ai.onSearchPath -= Update;

		}
	}

	/// <summary>Updates the AI's destination every frame</summary>
	private void Update()
	{
		if (!schocked)
		{
			if (!playerSeen)
			{
				if (waypoints != null && ai != null && !lookAround)
				{
					if (Vector3.Distance(ai.destination, transform.position) > 1f)
					{
						ai.destination = waypoints[index].position;
						Debug.Log(Vector3.Distance(ai.destination, transform.position));
					}
					else
					{
						index = (index + 1) % (waypoints.Length);
						ai.destination = waypoints[index].position;
					}

				}
			}
			if (fOVDetection)
			{
				if (fOVDetection.IsInFOV)
				{
					if (playerSeen)
					{
						ai.destination = fOVDetection.player.position;
					}
					else
					{
						StopPatrol();
					}
				}
				/*
				else
				{
					playerSeen = false;
					if (lookAround)
						LookAroundStart();
				}
				*/
			}
		}
		
	}
	private void LookAroundStart()
	{
		if(!lookIsRunning)
			StartCoroutine(LookAround());
	}
	public void StopPatrol()
	{
		playerSeen = true;
		Debug.Log("Found you!");
		StopCoroutine(LookAround());
		lookIsRunning = false;
	}
	private IEnumerator LookAround()
	{
		lookIsRunning = true;
		while (true)
		{
			if (Vector3.Distance(ai.destination, transform.position) > 1f)
			{
				UpdateAI();
			}
			else
			{
				float count = 0;
				Quaternion rightLook = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 40, 0));
				Quaternion leftLook = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, -40, 0));
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
					count += Time.deltaTime/2;
					yield return null;

				}
				count = 0;
				while (count < 1)
				{
					transform.rotation = Quaternion.Lerp(leftLook, startRot, count);
					count += Time.deltaTime;
					yield return null;

				}
				index = (index + 1) % (waypoints.Length);
				UpdateAI();
			}

			yield return null;
		}
		
	}
	public void Schock()
	{
		schocked = true;
		ai.isStopped = true;
		StartCoroutine(Schocked());
	}
	IEnumerator Schocked()
	{
		yield return new WaitForSeconds(2f);
		ai.isStopped = false;
		schocked = false;
	}
	//Update ai inner Value;
	private void UpdateAI()
	{
		ai.destination = waypoints[index].position;
	}
}
