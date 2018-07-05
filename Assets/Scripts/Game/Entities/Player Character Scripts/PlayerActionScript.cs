using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionScript : MonoBehaviour
{
	// Player variables
    private Animator playerAnimator;
	private PlayerMovementScript playerMovementScript;

	// Sound Manager 
	private SoundManagerScript soundManagerScript;

	// Physics variables (Raycasts, Capsulecasts, etc.)
    int interactableLayerMask;
    int enemiesLayerMask;

	// Pause the game
	private bool inputEnabled = true;

 	// Variables to handle Attack1 mechanic
	// Meele attack in all directions at the same time.
	// If the input is given while attacking1, the attack restarts.
	[Header("Quick Spin Parameters")]
	public float quickSpinRadius = 2f;
	public float quickSpinDuration = 0.4f;
	private bool chargingQuickSpin;
	private float quickSpinTimer = -1f;
	private string quickSpin = "Quick Spin";

	
	// Variables to handle Attack2 mechanic
	// The inpunt must be constant, and everything inside a cone casted from the forward vector will be sucked slowly.
	// The momevemnt should be disable while this attack is enabled.
	[Header("Wind Pull  Parameters")]
	private bool windPullEnabled;
	private string windPull = "Wind Pull";


	void Awake() 
	{

	}

	void Start() 
	{
		playerAnimator = GlobalData.PlayerAnimator;
		playerMovementScript = GlobalData.PlayerMovementScript;
		interactableLayerMask = 1 << (int) Mathf.Log(GlobalData.InteractableLayerMask.value,2);
        enemiesLayerMask = 1 << (int) Mathf.Log(GlobalData.EnemiesLayerMask.value,2);
		soundManagerScript = GlobalData.SoundManagerScript;
	}

	void Update() 
	{
		if (!inputEnabled)
		{
			return;
		}
		
		// Quick Spin
		if (GlobalData.GetAttack1ButtonDown() && playerMovementScript.playerCloseToGround && !playerMovementScript.playerSliding &&!windPullEnabled)
		{
			chargingQuickSpin = true;
			playerMovementScript.DisableInput();
			playerAnimator.SetBool(quickSpin, chargingQuickSpin);
		}

		if (chargingQuickSpin && !GlobalData.GetAttack1Button())
		{
			chargingQuickSpin = false;
			playerMovementScript.EnableInput();
			playerAnimator.SetBool(quickSpin, chargingQuickSpin);
			soundManagerScript.PlayAttack1Sound();
			quickSpinTimer = 0f;
		}

		// Wind Pull
		if (GlobalData.GetAttack2Button() && quickSpinTimer < 0f && !windPullEnabled)
		{
			windPullEnabled = true;

			playerMovementScript.DisableInput();
			playerAnimator.SetBool(windPull, windPullEnabled);
			soundManagerScript.PlayAttack2Sound();

		}
		if (windPullEnabled && !GlobalData.GetAttack2Button())
		{
			windPullEnabled = false;

			playerMovementScript.EnableInput();
			playerAnimator.SetBool(windPull, windPullEnabled);
			soundManagerScript.StopAttack2Sound();
		}
	}

	void FixedUpdate()
	{
		// Quick Spin
		if (!chargingQuickSpin && quickSpinTimer > -1f)
        {
			quickSpinTimer += Time.fixedDeltaTime;
			
			if (quickSpinTimer <= quickSpinDuration)
			{
				Collider[] colliders = Physics.OverlapSphere(transform.position, quickSpinRadius , interactableLayerMask | enemiesLayerMask);
				foreach (Collider collider in colliders)
				{
					IInteractable interactable = collider.gameObject.GetComponent<IInteractable>();
					if (interactable != null)
					{
						interactable.OnPush();
					}
				}
			}
			else
			{
				quickSpinTimer =  -1f;
			}
        }
		
		// Wind Pull
		if (windPullEnabled)
		{
			Collider[] colliders = Physics.OverlapBox(transform.position+transform.forward*1.5f+transform.up*0.5f,Vector3.one*2f,Quaternion.LookRotation(transform.forward,transform.up),interactableLayerMask | enemiesLayerMask);
			
			foreach (Collider collider in colliders)
            {
                IInteractable interactable = collider.gameObject.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.OnPull();
                }
            }
		}
	}

	public void DisableInput()
    {
        inputEnabled = false;
    }

    public void EnableInput()
    {
        inputEnabled = true;
    }
}
