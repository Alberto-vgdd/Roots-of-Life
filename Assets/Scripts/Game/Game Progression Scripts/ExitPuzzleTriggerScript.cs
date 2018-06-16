using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPuzzleTriggerScript : MonoBehaviour 
{
	[Header("Name of the target scene")]
	public string sceneName;
	private string playerTag;

	void Start()
	{
		playerTag = GlobalData.PlayerTag;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals(playerTag))
		{
			GlobalData.GameManager.ChangeScene(sceneName);
		}
	}
}
