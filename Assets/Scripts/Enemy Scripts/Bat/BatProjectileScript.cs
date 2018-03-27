using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatProjectileScript : MonoBehaviour
{
	private Rigidbody projectileRigidbody;
	private bool projectileEnabled;
	private string playerTag;
	private int environmentLayerMask;
	
	private float projectileSpeed;
	private float projectileStrengh;

	void Awake()
	{
		projectileRigidbody = GetComponent<Rigidbody>();
	}

	void Start ()
	{
		playerTag = GlobalData.PlayerTag;
		environmentLayerMask = (int) Mathf.Log(GlobalData.EnvironmentLayerMask.value,2);
	}
	
	public void Shoot(float projectileLifeTime, float projectileSpeed, float projectileStrengh)
	{
		projectileEnabled = true;

		this.projectileSpeed = projectileSpeed;
		this.projectileStrengh = projectileStrengh;

		Invoke("DisableProjectile",projectileLifeTime*0.9f);
		Disappear(projectileLifeTime);
	}

	void FixedUpdate () 
	{
		projectileRigidbody.MovePosition(projectileRigidbody.position + transform.forward* Time.fixedDeltaTime*projectileSpeed);
		
		if (GlobalData.PlayerDeath && projectileEnabled)
		{
			DisableProjectile();
			Disappear(0.25f);
		}

	}

	void Disappear(float seconds)
	{		
		Destroy(this.gameObject,seconds);
	}

	void DisableProjectile()
	{		
		transform.GetComponent<ParticleSystem>().Stop();
		projectileEnabled = false;
	}

	void OnTriggerEnter(Collider other)
	{
		if (projectileEnabled)
		{
			if (LayerMask.Equals(environmentLayerMask,other.gameObject.layer))
			{
				
				DisableProjectile();

			}
			else if (other.CompareTag(playerTag))
			{
				DisableProjectile();
				GlobalData.PlayerMovementScript.Push(transform.forward,projectileStrengh);

				// This line is necessary because the player collider can't detect triggers as collisions.
				GlobalData.PlayerHealthScript.TakeDamage(1);
			}
		}
		
		
	}
}
