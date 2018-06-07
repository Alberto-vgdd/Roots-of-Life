using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotZoneScript : MonoBehaviour 
{
	[Header("Hotzone Parameters")]
	public float timeBetweenAttacks;
	public int damage;

	private float timer = 0f;
	private string playerTag;
	private bool playerInside = false;

	void Start()
	{
		playerTag = GlobalData.PlayerTag;
	}

	void FixedUpdate()
	{
		// Recovery time. The player can't get hit during this period of time
		if (playerInside)
		{
			timer += Time.fixedDeltaTime;

			if (timer >= timeBetweenAttacks)
			{
				timer = 0f;

				GlobalData.PlayerHealthScript.TakeDamage(damage);
				// Test camera shake 
				GlobalData.CameraShakeScript.ShakeCamera(0.2f,0.25f);
			}
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if ( other.CompareTag(playerTag))
		{
			playerInside = true;
			timer = 0f;
		}
	}
	void OnTriggerExit(Collider other)
	{
		if ( other.CompareTag(playerTag))
		{
			playerInside = false;
		}
	}
}
