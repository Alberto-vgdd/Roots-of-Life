using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlBossTrigger : MonoBehaviour 
{
	public GameObject owlBossGameObject;
	public GameObject doorGameObject;

	private string playerTag;


	void Start () 
	{
		playerTag = GlobalData.PlayerTag;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(playerTag))
		{
			owlBossGameObject.SetActive(true);
			doorGameObject.SetActive(true);
			this.gameObject.SetActive(false);
		}
	}
}
