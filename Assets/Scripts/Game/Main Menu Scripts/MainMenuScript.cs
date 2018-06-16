using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class MainMenuScript : MonoBehaviour 
{
    public string sceneName = "Main Area";
    public GameObject gameButtons;

    private void Start()
    {
        
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(sceneName);

        Analytics.CustomEvent("SceneLoaded", new Dictionary<string, object>{ {"SceneName", sceneName}});

    }

    public void CreditsButton()
    {

    }

    public void QuitButton()
    {
        Application.Quit();
    }



    public void EnableGameButtons()
    {
        gameButtons.SetActive(true);
    }

     public void DisableGameButtons()
    {
        gameButtons.SetActive(false);
    }

}
