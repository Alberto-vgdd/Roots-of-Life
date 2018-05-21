using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour 
{
	// Player and Camera variables, used to move them.
	private Transform playerTransform;
	private Transform playerCameraTransform;
	private FreeCameraMovementScript freeCameraMovementScript;
	private FixedCameraMovementScript fixedCameraMovementScript;
	private CameraShakeScript cameraShakeScript;

	// Checkpoint stuff, used to restore the state after a death
	private Transform checkPoint;
	private bool checkPointFreeCameraEnabled;
	private bool checkPointFixedCameraEnabled;

	// GameUI script
	private GameUIScript gameUIScript;

	// Use this for initialization
	void Awake()
	{
		if (GlobalData.GameManager == null)
		{
			GlobalData.GameManager = this;

			GlobalData.PlayerTransform = playerTransform = GameObject.Find("Player Character").transform;
			GlobalData.PlayerTargetTransform = playerTransform.Find("Target");
			GlobalData.PlayerMovementScript = playerTransform.GetComponent<PlayerMovementScript>();
			GlobalData.PlayerActionScript = playerTransform.GetComponent<PlayerActionScript>();
			GlobalData.PlayerHealthScript = playerTransform.GetComponent<PlayerHealthScript>();
			GlobalData.PlayerAnimator = playerTransform.GetComponentInChildren<Animator>();
			GlobalData.PlayerCameraHorizontalPivotTransform = playerCameraTransform = GameObject.Find("Player Camera Horizontal Pivot").transform;
			GlobalData.PlayerCamera = playerCameraTransform.GetComponentInChildren<Camera>();

			// TEST
			Screen.SetResolution(853, 480, true, 0);
        	Application.targetFrameRate = 30;
			Cursor.lockState = CursorLockMode.Locked;


			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(this.gameObject);
		}
	}

	void Start()
	{
		freeCameraMovementScript = GlobalData.FreeCameraMovementScript;
		fixedCameraMovementScript = GlobalData.FixedCameraMovementScript;
		cameraShakeScript = GlobalData.CameraShakeScript;
		gameUIScript = GlobalData.GameUIScript;

	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

	}

	public void UpdateCheckPoint(Transform newCheckPoint, bool freeCameraEnabled, bool fixedCameraEnabled)
	{
		checkPoint = newCheckPoint;
		checkPointFreeCameraEnabled = freeCameraEnabled;
		checkPointFixedCameraEnabled = fixedCameraEnabled;
		
	}

	public void StartGameOver()
	{
		StartCoroutine(GameOver());
	}

	IEnumerator GameOver()
	{
		// "Kill" the character
		GlobalData.PlayerMovementScript.DisableInput();
		GlobalData.PlayerDeath = true;

		// Fade out the game.
		gameUIScript.StartGameFadeOut();

		// Wait for the game to fade out, and then move the character and the camera to the checkpoint's position.
		yield return new WaitForSeconds(1f);
		playerTransform.position = playerCameraTransform.position = checkPoint.position;
		playerTransform.rotation = playerCameraTransform.rotation = checkPoint.rotation;
		
		// Enable/Disable the camera scripts
		fixedCameraMovementScript.enabled = checkPointFixedCameraEnabled;
		freeCameraMovementScript.enabled = checkPointFreeCameraEnabled;

		if (checkPointFixedCameraEnabled)
		{
			fixedCameraMovementScript.StartCameraTransition();
		}
		if (checkPointFreeCameraEnabled)
		{
			freeCameraMovementScript.CenterCamera();
		}
		
		// Wait for the camera to move properly to the character position and then fade in.
		yield return new WaitForSeconds(0.5f);
		gameUIScript.StartGameFadeIn();

		// If the character loses all the acorns, recharge them.
		if (GlobalData.AcornCount == 0)
		{
			GlobalData.AcornCount = GlobalData.MinimumAcornCount;
			gameUIScript.UpdateAcornCounter();
		}

		// "Revive" the character and show the health in the UI again.
		GlobalData.PlayerMovementScript.EnableInput();
		GlobalData.PlayerDeath = false;
	
	}
     
    public void ShakeCamera(float shakeDistance, float shakeDuration)
	{
		cameraShakeScript.ShakeCamera(shakeDistance,shakeDuration);
	}
    
}
