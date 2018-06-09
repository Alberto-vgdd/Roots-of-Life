using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class ScreenManager : MonoBehaviour {

    private string SceneName;

    private void Start()
    {
        SceneName = name;
    }

    public void LoadScene(string name)
    {
        Application.LoadLevel(name); //function to load scene based on string value attatched in the inspector
        Analytics.CustomEvent("SceneLoaded", new Dictionary<string, object>{
        {"SceneName", SceneName}
        });

    }

    public void QuitGame()
    {
        Application.Quit(); //unitys quit function which terminates game
    }
}
