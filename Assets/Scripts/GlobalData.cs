using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData
{
    // In-game variables, stats, etc.
    public static int playerHealth = 3;
    public static int playerMaxHealth = 3;
    public static float playerRecoveryTime = 2f;
    public static bool PlayerDeath;

    public static int AcornCount = 0;
    public static int InfectionCount = 0;

    //  GameManager variables
    public static GameManagerScript GameManager;

    // Reference to the GameUJI
    public static GameUIScript GameUIScript;
    
    // Layer Masks
    public static LayerMask EnvironmentLayerMask = LayerMask.GetMask("Environment");
    public static LayerMask EnemiesLayerMask = LayerMask.GetMask("Enemies");

    // Tags
    public const string PlayerTag = "Player";

    // Variables to lock enemies
    public static bool IsEnemyLocked;
    public static Transform LockedEnemyTransform;

    // Player Transforms, Scripts, Animators
    public static Transform PlayerTransform;
    public static Transform PlayerTargetTransform;

    public static PlayerMovementScript PlayerMovementScript;
    public static PlayerActionScript PlayerActionScript;
    public static PlayerHealthScript PlayerHealthScript;

    public static Animator PlayerAnimator;

    public static Camera PlayerCamera;
    public static Transform PlayerCameraHorizontalPivotTransform;

    // Camera Scripts
    public static FreeCameraMovementScript FreeCameraMovementScript;
    public static FixedCameraMovementScript FixedCameraMovementScript;
    public static CameraEnemyTrackerScript CameraEnemyTrackerScript;

    // Input Manager Script
    public static InputManagerScript InputManagerScript;

    // Sound Manager Script
    public static SoundManagerScript SoundManagerScript;

    // Call the function in the CameraEnemyTrackerScript
    public static void ChangeLockOn(float input)  {    CameraEnemyTrackerScript.ChangeLockOn(input);  }

    // Call the function in the CameraMovementScript
    public static void CenterCamera(){    FreeCameraMovementScript.CenterCamera();}


    // Call the function in InputManagerScript
    public static bool GetJoystickInUse(){     return InputManagerScript.GetJoystickInUse();}
    public static float GetHorizontalInput(){    return InputManagerScript.GetHorizontalInput();}
    public static float GetVerticalInput(){    return InputManagerScript.GetVerticalInput();}
    public static float GetHorizontalCameraInput(){    return InputManagerScript.GetHorizontalCameraInput();}
    public static float GetVerticalCameraInput(){    return InputManagerScript.GetVerticalCameraInput();}
    public static bool GetLockOnButton(){    return InputManagerScript.GetLockOnButton();}
    public static float GetChangeTarget(){		return InputManagerScript.GetChangeTarget();}
    public static bool GetJumpButtonDown(){		return InputManagerScript.GetJumpButtonDown();}
    public static float GetRunButton(){   return InputManagerScript.GetRunButton();}

    

}
