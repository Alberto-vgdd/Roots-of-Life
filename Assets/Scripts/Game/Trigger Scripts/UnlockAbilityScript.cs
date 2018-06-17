using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockAbilityScript : MonoBehaviour 
{
	[Header("Ability to unlock")]
	public int ability;
	private string playerTag;

	void Start()
	{
		playerTag = GlobalData.PlayerTag;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals(playerTag))
		{
			GlobalData.GameManager.Unlock(ability);
		}
	}
}
