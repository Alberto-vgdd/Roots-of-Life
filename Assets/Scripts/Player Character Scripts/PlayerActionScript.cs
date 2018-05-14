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

 	// Variables to handle Attack1 mechanic
	// Meele attack in all directions at the same time.
	// If the input is given while attacking1, the attack restarts.
	[Header("Attack 1 Parameters")]
	public float attack1Radius = 2f;
	public float attack1Duration = 0.5f;
	public bool isAttacking1;
	private float attack1Timer = 0f;

	// Variables to handle Attack2 mechanic
	// The inpunt must be constant, and everything inside a cone casted from the forward vector will be sucked slowly.
	// The momevemnt should be disable while this attack is enabled.
	[Header("Attack 2 Parameters")]
	public bool isAttacking2;


	
	




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
		// Attack 1
		if (Input.GetKeyDown(KeyCode.E) && !isAttacking2)
		{
			isAttacking1 = true;
			playerAnimator.SetTrigger("Attack 1");
			soundManagerScript.PlayAttack1Sound();
			attack1Timer = 0f;
		}

		if (isAttacking1)
		{
			attack1Timer += Time.deltaTime;
			if (attack1Timer >= attack1Duration)
			{
				isAttacking1 = false;
			}
		}

		// Attack 2
		if (Input.GetKeyDown(KeyCode.Q) && !isAttacking1)
		{
			playerMovementScript.DisableInput();
			isAttacking2 = true;
			playerAnimator.SetBool("Attack 2", true);
			soundManagerScript.PlayAttack2Sound();

		}
		if (isAttacking2 && Input.GetKeyUp(KeyCode.Q))
		{
			playerMovementScript.EnableInput();
			isAttacking2 = false;
			playerAnimator.SetBool("Attack 2", false);
			soundManagerScript.StopAttack2Sound();

		}
	}

	void FixedUpdate()
	{
		if (isAttacking1)
        {

            Collider[] colliders = Physics.OverlapSphere(transform.position,attack1Radius,interactableLayerMask | enemiesLayerMask);

            foreach (Collider collider in colliders)
            {
                IInteractable interactable = collider.gameObject.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.OnPush();
                }
            }
        }

		if (isAttacking2)
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
}
