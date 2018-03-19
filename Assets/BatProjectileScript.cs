using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatProjectileScript : MonoBehaviour
{
	private Rigidbody projectileRigidbody;
	private bool projectileEnabled;
	private string playerTag;
	private int environmentLayerMask;
	
	private float projectileLifeTime = 5f;
	private float projectileSpeed = 8f;
	private float projectileStrengh = 10f;

	void Awake()
	{
		projectileRigidbody = GetComponent<Rigidbody>();
	}

	void Start ()
	{
		playerTag = GlobalData.PlayerTag;
		environmentLayerMask = (int) Mathf.Log(GlobalData.EnvironmentLayerMask.value,2);
		projectileEnabled = true;

		Invoke("DisableProjectile",projectileLifeTime*0.9f);
		Disappear(projectileLifeTime);

	}
	
	void FixedUpdate () 
	{
		projectileRigidbody.MovePosition(projectileRigidbody.position + transform.forward* Time.fixedDeltaTime*projectileSpeed);
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
				//GlobalData.GameManager.StartGameOver();
			}
		}
		
		
	}
}
