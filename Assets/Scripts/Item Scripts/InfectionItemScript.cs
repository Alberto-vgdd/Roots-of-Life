using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionItemScript : MonoBehaviour 
{
	private string playerTag;
	private bool infectionCollected;

	void Start () 
	{
		playerTag = GlobalData.PlayerTag;
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if (!infectionCollected && collision.collider.CompareTag(playerTag))
		{
			CollectInfection();
		}
	}

	void CollectInfection()
	{
		GlobalData.InfectionCount++;
		GlobalData.GameUIScript.UpdateInfectionCounter();
		gameObject.SetActive(false);
	}
}
