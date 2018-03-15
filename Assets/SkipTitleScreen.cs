using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipTitleScreen : MonoBehaviour 
{


	// Update is called once per frame
	void Update () 
	{
		if (SceneManager.GetActiveScene().buildIndex != 1 && (Input.GetButtonDown("JumpJoystick") || Input.GetButtonDown("JumpKeyboard")))
		{
			SceneManager.LoadScene(1);
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{

			Application.Quit();
		
		}
	}
}
