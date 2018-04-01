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



	// Function for revive the character.
	public void RestoreMaxHealth()
	{
		GlobalData.playerHealth = GlobalData.playerMaxHealth;
	}

	// Function for damage from enemies.
    public void TakeDamage(int damageAmount)
    {
        GlobalData.playerHealth -= damageAmount;
		GlobalData.GameUIScript.UpdateHealthIcons();
        
		if(GlobalData.playerHealth <= 0)
		{
			GlobalData.GameManager.StartGameOver();
		}
	}

	// Function to recover health points
    public void RecoverHealth(int healthAmout)
    {
		GlobalData.playerHealth = Mathf.Min(GlobalData.playerHealth + healthAmout, GlobalData.playerMaxHealth);
		GlobalData.GameUIScript.UpdateHealthIcons();
	}

	//function to detect collision from the enemy 
    void OnCollisionStay(Collision collision)
    {
        if (LayerMask.Equals(enemiesLayerMask,collision.gameObject.layer)&& recoveryTimer < 0)
        {
			recoveryTimer = 0f;
        	TakeDamage(1);
        }
    }
	
	void OnCollisionEnter(Collision collision)
    {
        if (LayerMask.Equals(enemiesLayerMask,collision.gameObject.layer)&& recoveryTimer < 0)
        {
			recoveryTimer = 0f;
        	TakeDamage(1);
        }
    }
}
