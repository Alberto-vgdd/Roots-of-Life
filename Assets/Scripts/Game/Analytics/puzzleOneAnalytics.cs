using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class puzzleOneAnalytics : MonoBehaviour
{

    void Start()
    {
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;

        if (sceneName == "Puzzle 1")
        {
            GlobalData.GameManager.timeSinceStart = 0;

        }
        else if (sceneName == "Puzzle 2")
        {
            GlobalData.GameManager.timeSinceStart = 0;
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Puzzle 1 exit")){
            Analytics.CustomEvent("Puzzle 1 length", new Dictionary<string, object>
            {
                {"Puzzle 1 was completed in:", GlobalData.GameManager.timeSinceStart }
            });
        }
        if(other.CompareTag("Puzzle 2 exit"))
        {
            Analytics.CustomEvent("Puzzle 2 length", new Dictionary<string, object>
            {
                {"Puzzle 2 was completed in:", GlobalData.GameManager.timeSinceStart }
            });
        }
    }
}