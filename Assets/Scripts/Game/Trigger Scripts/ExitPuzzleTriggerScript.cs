using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPuzzleTriggerScript : MonoBehaviour 
{
	[Header("Name of the target scene")]
	public string sceneName;
	public float timeBeforeTeleport = 5f;
	private string playerTag;

	void Start()
	{
		playerTag = GlobalData.PlayerTag;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals(playerTag))
		{
			GlobalData.GameManager.DisableInput();
			Invoke ("ExitPuzzle",timeBeforeTeleport);
		}
	}

	void ExitPuzzle()
	{
		if (!GlobalData.GamePaused)
		{
			GlobalData.GameManager.EnableInput();
		}

		SceneManager.LoadScene(sceneName);
	}
}
