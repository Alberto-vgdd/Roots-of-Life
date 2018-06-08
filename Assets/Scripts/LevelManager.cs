using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;    

public class LevelManager : MonoBehaviour {

    public void LoadScene(string name)
    {
        Application.LoadLevel(name); //function to load scene based on string value attatched in the inspector

        Analytics.CustomEvent("SceneLoaded", new Dictionary<string, object>{
        {"SceneName", "name"}
        });


    }

    public void QuitGame()
    {
        Application.Quit(); //unitys quit function which terminates game
    }
}
