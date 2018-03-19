using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAIScript : MonoBehaviour 
{
	public GameObject projectile;
	private Transform playerTransform;

	private float timeBetweenProjectiles = 2f;
	private float projectileTimer;

	// Variables to look at the character smoothly
	private Vector3 currentVelocity;
	private float turnSpeed = 0.5f;

	// Variables to shoot at the character's predicted position.
	private Rigidbody playerRigidbody;
	private float projectileDirectionOffset = 4f;
	private float targetDistance;
	private Vector3 targetPosition;


	void Start () 
	{
		playerTransform = GlobalData.PlayerTransform;
		playerRigidbody = playerTransform.GetComponent<Rigidbody>();

		projectileTimer = 0f;
	}
	
	void Update () 
	{
		LookAtTarget();
		Blast();
		Debug.DrawRay(transform.position,transform.forward,Color.blue);
	}

	void LookAtTarget()
	{
		targetDistance = (playerTransform.position - transform.position).magnitude/10;
		targetPosition = playerTransform.position + playerTransform.forward*(playerRigidbody.velocity.magnitude*targetDistance); //Distance, speed
		transform.forward = Vector3.SmoothDamp(transform.forward,Vector3.Scale(new Vector3(1f,0,1f),targetPosition- transform.position),ref currentVelocity, turnSpeed);
	}

	void Blast()
	{
		projectileTimer += Time.deltaTime;

		if (projectileTimer >= timeBetweenProjectiles)
		{
			projectileTimer = 0f;
			Instantiate(projectile,transform.position + transform.forward,Quaternion.LookRotation(targetPosition - transform.position));
		}
	}
}
