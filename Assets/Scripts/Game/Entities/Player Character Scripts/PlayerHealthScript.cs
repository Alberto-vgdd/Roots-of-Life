using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthScript : MonoBehaviour
{

	private float recoveryTime;
	private float recoveryTimer = -1f;
	private int enemiesLayerMask;


	void Start()
	{
		enemiesLayerMask = (int) Mathf.Log(GlobalData.EnemiesLayerMask.value,2);
		recoveryTime = GlobalData.playerRecoveryTime;
	}

	void FixedUpdate()
	{
		// Recovery time. The player can't get hit during this period of time
		if (recoveryTimer >= 0)
		{
			recoveryTimer += Time.fixedDeltaTime;

			if (recoveryTimer >= recoveryTime)
			{
				recoveryTimer = -1f;
			}
		}
	}

	// Function for damage from enemies.
    public void TakeDamage(int damageAmount)
    {
        GlobalData.AcornCount -= damageAmount;
		GlobalData.AcornCount = Mathf.Max(0,GlobalData.AcornCount);
		GlobalData.GameUIScript.UpdateAcornCounter();
        
		if(GlobalData.AcornCount <= 0)
		{
			GlobalData.GameManager.StartGameOver();
		}
	}

	//function to detect collision from the enemy 
    void OnCollisionStay(Collision collision)
    {
        if (LayerMask.Equals(enemiesLayerMask,collision.gameObject.layer)&& recoveryTimer < 0)
        {
			recoveryTimer = 0f;
        	TakeDamage(3);

			// Test camera shake 
			GlobalData.CameraShakeScript.ShakeCamera(0.2f,0.25f);
        }
    }
	
	void OnCollisionEnter(Collision collision)
    {
        if (LayerMask.Equals(enemiesLayerMask,collision.gameObject.layer)&& recoveryTimer < 0)
        {
			recoveryTimer = 0f;
        	TakeDamage(3);

			// Test camera shake 
			GlobalData.CameraShakeScript.ShakeCamera(0.2f,0.25f);
        }
    }
}
