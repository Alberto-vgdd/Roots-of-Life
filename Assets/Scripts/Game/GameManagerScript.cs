using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Analytics;

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
			DontDestroyOnLoad(this.gameObject);
		}
		else if (GlobalData.GameManager != this)
		{
			Destroy(this.gameObject);
		}

		GlobalData.PlayerTransform = GameObject.Find("Player Character").transform;
		GlobalData.PlayerTargetTransform = GlobalData.PlayerTransform.Find("Target");
		GlobalData.PlayerMovementScript = GlobalData.PlayerTransform.GetComponent<PlayerMovementScript>();
		GlobalData.PlayerActionScript = GlobalData.PlayerTransform.GetComponent<PlayerActionScript>();
		GlobalData.PlayerHealthScript = GlobalData.PlayerTransform.GetComponent<PlayerHealthScript>();
		GlobalData.PlayerAnimator = GlobalData.PlayerTransform.GetComponentInChildren<Animator>();
		GlobalData.PlayerCameraHorizontalPivotTransform = GameObject.Find("Player Camera Horizontal Pivot").transform;
		GlobalData.PlayerCamera = GlobalData.PlayerCameraHorizontalPivotTransform.GetComponentInChildren<Camera>();



		// TEST
		//Screen.SetResolution(853, 480, true, 0);
		//Application.targetFrameRate = 30;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
	{
		// Pause
		if (Input.GetKeyDown(KeyCode.P))
		{
			Time.timeScale = 1 - Time.timeScale;

			if (Time.timeScale == 0)
			{
				DisableInput();
			}
			else
			{
				EnableInput();
			}

			
		}

		if (Input.GetKeyDown(KeyCode.U))
		{
			Unlock(GlobalData.LEVEL_2);
		}

	}

	public void ChangeScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	//This is called each time a scene is loaded.
	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		playerTransform = GlobalData.PlayerTransform;
		playerCameraTransform = GlobalData.PlayerCameraHorizontalPivotTransform;
		freeCameraMovementScript = GlobalData.FreeCameraMovementScript;
		fixedCameraMovementScript = GlobalData.FixedCameraMovementScript;
		gameUIScript = GlobalData.GameUIScript;
		cameraShakeScript = GlobalData.CameraShakeScript;

	}

	void OnEnable()
	{
		//Tell our ‘OnLevelFinishedLoading’ function to start listening for a scene change event as soon as this script is enabled.
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable()
	{
		//Tell our ‘OnLevelFinishedLoading’ function to stop listening for a scene change event as soon as this script is disabled.
		//Remember to always have an unsubscription for every delegate you subscribe to!
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
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

        Analytics.CustomEvent("Game Over", new Dictionary<string, object>{
            {"Acons left", GlobalData.AcornCount } });

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

	public void EnableInput()
	{
		GlobalData.PlayerMovementScript.EnableInput();
		GlobalData.PlayerActionScript.EnableInput();
		GlobalData.FreeCameraMovementScript.EnableInput();
		GlobalData.FixedCameraMovementScript.EnableInput();
	}

	public void DisableInput()
	{
		GlobalData.PlayerMovementScript.DisableInput();
		GlobalData.PlayerActionScript.DisableInput();
		GlobalData.FreeCameraMovementScript.DisableInput();
		GlobalData.FixedCameraMovementScript.DisableInput();
	}

	public void Unlock(int index)
	{
		switch (index)
		{
			case GlobalData.RUN_ABILITY:
				GlobalData.runUnlocked = true;
				GlobalData.PlayerMovementScript.UpdateUnlockedAbilities();

                Analytics.CustomEvent("The player has unlocked 'run'");

				break;

			case GlobalData.DOUBLE_JUMP_ABILITY:
				GlobalData.doubleJumpUnlocked = true;
				GlobalData.PlayerMovementScript.UpdateUnlockedAbilities();

                Analytics.CustomEvent("The player has unlocked 'jump'");

				break;

			case GlobalData.LEVEL_2: 
				GlobalData.level2Unlocked = true;

                Analytics.CustomEvent("The player has unlocked 'Level 2'");

				GameObject level2Lock = GameObject.Find("LEVEL 2 LOCK");
				if (level2Lock != null)
				{
					level2Lock.SetActive(false);
				}
				break;
			default:
				break;
		}
		
	}
    
}
