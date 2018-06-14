using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayDetector : MonoBehaviour {

    private string URL = "http://62.131.170.46/roots-of-life/setActive.php";

    public Text usernameText;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    IEnumerator setActive()
    {
        WWWForm form = new WWWForm();
        form.AddField("setUsername", usernameText.text);
        form.AddField("setLogin", (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        WWW www = new WWW(URL, form);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log("error: " + www.error);
        }
        else
        {
            Debug.Log("result: " + www.text);
        }
    }

    public void processPlay()
    {
        StartCoroutine(setActive());
    }
}
