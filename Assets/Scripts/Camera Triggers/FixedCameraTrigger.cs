using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCameraTrigger : MonoBehaviour
{
	private FixedCameraMovementScript Camera2D;
	private FreeCameraMovementScript Camera3D;
	private string playerTag;

	[Header("Camera Parameters")]
	public float targetDistance;
	public float targetHeight = 1f;
	public float targetHoriontalAngle;
	public float targetVerticalAngle;

	[Header("Camera Movement Parameters")]
	public float cameraFollowSpeedMultiplier = 5f;
	public float cameraTransitionTime = 2f;

	[Header("Clipping Parameters")]
	public float cameraClippingOffset = 0.05f;

	void Start () 
	{
		playerTag = GlobalData.PlayerTag;
	}
	
	void OnTriggerEnter(Collider other)
	{
		Camera2D =  GlobalData.FixedCameraMovementScript;
		Camera3D =  GlobalData.FreeCameraMovementScript;

		if (other.CompareTag(playerTag))
		{
			if (!Camera2D.enabled)
			{
				Camera3D.enabled = false;
				
				Camera2D.enabled = true;
				Camera2D.SetUp(targetDistance,targetHeight,targetHoriontalAngle,targetVerticalAngle,cameraFollowSpeedMultiplier,cameraTransitionTime,cameraClippingOffset);
				Camera2D.StartCameraTransition();
			}
			else
			{
				bool sameConfig = Camera2D.EqualsTo(targetDistance,targetHeight,targetHoriontalAngle,targetVerticalAngle,cameraFollowSpeedMultiplier,cameraTransitionTime,cameraClippingOffset);
			
				if (!sameConfig )
				{
					Camera2D.SetUp(targetDistance,targetHeight,targetHoriontalAngle,targetVerticalAngle,cameraFollowSpeedMultiplier,cameraTransitionTime,cameraClippingOffset);
					Camera2D.StartCameraTransition();
				}
			}
		}
		
	}
}
