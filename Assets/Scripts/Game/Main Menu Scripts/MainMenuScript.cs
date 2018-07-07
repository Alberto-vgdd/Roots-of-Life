using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class MainMenuScript : MonoBehaviour 
{
    public string sceneName = "Main Area";
    public GameObject gameButtons;
    public float sceneLoadedPercentage;

    private IEnumerator loadSceneCoroutine;
    private AsyncOperation asyncSceneLoad; 

    void Start()
    {
        
        if (loadSceneCoroutine == null)
        {
            loadSceneCoroutine = AsyncSceneLoad(sceneName);
            StartCoroutine(loadSceneCoroutine);
        }

    }

    IEnumerator AsyncSceneLoad(string sceneName)
    {
        asyncSceneLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncSceneLoad.allowSceneActivation = false;

        while (!asyncSceneLoad.isDone)
        {
            sceneLoadedPercentage = Mathf.Ceil(asyncSceneLoad.progress*100f);
            yield return null;
        }

        Debug.Log(sceneName + " loaded");
    }

    public void PlayButton()
    {
        asyncSceneLoad.allowSceneActivation = true;
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
