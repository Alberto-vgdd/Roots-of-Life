using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAIScript : MonoBehaviour 
{
	// Variables to aim and shoot to the player.
	private Transform playerTransform;
	private string playerTag;
	private bool playerInRange;

	// Variables to look at the character smoothly
	private Transform batMesh;
	private Vector3 currentVelocity;
	private float turnSpeed = 0.5f;

	// Variables to predict the character's position.
	private Rigidbody playerRigidbody;
	private float targetDistance;
	private Vector3 targetPosition;


	[Header("Projectile Parameters")]
	public GameObject projectile;
	public float timeBetweenProjectiles = 3f;
	public float projectileLifeTime = 5f;
	public float projectileSpeed = 8f;
	public float projectileStrength = 10f;

	[Header("Bat Animator Parameters")]
	public Animator batAnimator;
	public float shootAnimationTime = 2f;
	private string shootTrigger = "Shoot";
	private bool batShooting;
	private float chargeTimer;




	void Awake()
	{
		batMesh = transform.GetChild(0);
	}

	void Start () 
	{
		playerTransform = GlobalData.PlayerTransform;
		playerTag = GlobalData.PlayerTag;
		playerRigidbody = playerTransform.GetComponent<Rigidbody>();

		chargeTimer = timeBetweenProjectiles + (Random.value*2f) - 1;
	}
	
	void Update () 
	{
		LookAtTarget();
		ChargeProjectile();
	}

	void LookAtTarget()
	{
		targetDistance = (playerTransform.position - batMesh.position).magnitude/10;
		targetPosition = playerTransform.position + playerRigidbody.velocity.normalized*(playerRigidbody.velocity.magnitude*targetDistance); //Distance, speed
		batMesh.forward = Vector3.SmoothDamp(batMesh.forward,Vector3.Scale(new Vector3(1f,0,1f),targetPosition- batMesh.position),ref currentVelocity, turnSpeed);
	}

	void ChargeProjectile()
	{
		if (!GlobalData.PlayerDeath && playerInRange && !batShooting)
		{
			chargeTimer -= Time.deltaTime;

			if (chargeTimer <= 0)
			{
				batShooting = true;
				batAnimator.SetTrigger(shootTrigger);
				Invoke(shootTrigger,shootAnimationTime);
			}
			
		}
	}

	void Shoot()
	{
		chargeTimer = timeBetweenProjectiles + (Random.value*2f) - 1;
		GameObject instantiatedProjectile = Instantiate(projectile,batMesh.position + batMesh.forward,Quaternion.LookRotation(targetPosition - batMesh.position));
		instantiatedProjectile.GetComponent<BatProjectileScript>().Shoot(projectileLifeTime,projectileSpeed,projectileStrength);
		batShooting = false;
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
