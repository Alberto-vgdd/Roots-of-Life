using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTriggerScript : MonoBehaviour, IInteractable
{
	[Header("Checkpoint Parameters")]
	public bool automaticCheckpoint = true;
	private string checkpointMessage = "Check Point!";
	private string playerTag;

	void Start()
	{
		playerTag = GlobalData.PlayerTag;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if ( automaticCheckpoint && GlobalData.currentCheckPoint != transform && other.tag.Equals(playerTag))
		{
			GlobalData.GameManager.UpdateCheckPoint(this.transform, GlobalData.FreeCameraMovementScript.enabled,GlobalData.FixedCameraMovementScript.enabled);
		}
	}

	public void OnPush()
	{
		if (!automaticCheckpoint && GlobalData.currentCheckPoint != transform)
		{
			GlobalData.GameManager.UpdateCheckPoint(this.transform, GlobalData.FreeCameraMovementScript.enabled,GlobalData.FixedCameraMovementScript.enabled);
			GlobalData.GameUIScript.DisplayMessage(checkpointMessage);
			GlobalData.GameManager.ShakeCamera(0.1f,0.2f);
		}
	}

	public void OnPull()
	{

	}
}
