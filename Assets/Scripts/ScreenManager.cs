using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour {

    public void LoadScene(string name)
    {
        Application.LoadLevel(name); //function to load scene based on string value attatched in the inspector

    }

    public void QuitGame()
    {
        Application.Quit(); //unitys quit function which terminates game
    }
}
