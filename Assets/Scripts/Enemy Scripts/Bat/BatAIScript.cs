using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAIScript : MonoBehaviour 
{
	public GameObject projectile;
	private Transform playerTransform;
	private string playerTag;
	private bool playerInRange;

	// Variables to "randomly shoot"
	private float timeBetweenProjectiles = 2f;
	private float projectileTimer;

	// Variables to look at the character smoothly
	private Vector3 currentVelocity;
	private float turnSpeed = 0.5f;

	// Variables to shoot at the character's predicted position.
	private Rigidbody playerRigidbody;
	private float targetDistance;
	private Vector3 targetPosition;


	void Start () 
	{
		playerTransform = GlobalData.PlayerTransform;
		playerTag = GlobalData.PlayerTag;
		playerRigidbody = playerTransform.GetComponent<Rigidbody>();

		projectileTimer = (Random.value*2f) - 1;
	}
	
	void Update () 
	{
		LookAtTarget();
		Blast();
	}

	void LookAtTarget()
	{
		targetDistance = (playerTransform.position - transform.position).magnitude/10;
		targetPosition = playerTransform.position + playerRigidbody.velocity.normalized*(playerRigidbody.velocity.magnitude*targetDistance); //Distance, speed
		transform.forward = Vector3.SmoothDamp(transform.forward,Vector3.Scale(new Vector3(1f,0,1f),targetPosition- transform.position),ref currentVelocity, turnSpeed);
	}

	void Blast()
	{
		if (!GlobalData.PlayerDeath && playerInRange)
		{
			projectileTimer += Time.deltaTime;

			if (projectileTimer >= timeBetweenProjectiles)
			{
				projectileTimer = (Random.value*2f) - 1;
				Instantiate(projectile,transform.position + transform.forward,Quaternion.LookRotation(targetPosition - transform.position));
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(playerTag))
		{
			playerInRange = true;
		}
		
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag(playerTag))
		{
			playerInRange = false;
		}
	}
}
