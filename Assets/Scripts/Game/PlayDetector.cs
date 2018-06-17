using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayDetector : MonoBehaviour {

    private string activeURL = "http://62.131.170.46/roots-of-life/setActive.php";
    private string inactiveURL = "http://62.131.170.46/roots-of-life/setInactive.php";

    public string username;

    private void Awake()
    {
        if (GetComponent<FlagListener>() != null)
            username = GetComponent<FlagListener>().username;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    IEnumerator setActive()
    {
        Debug.Log(username);
        WWWForm form = new WWWForm();
        form.AddField("setUsername", username);
        form.AddField("setLogin", (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        WWW www = new WWW(activeURL, form);
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

    IEnumerator setInactive()
    {
        WWWForm form = new WWWForm();
        form.AddField("setUsername", username);
        form.AddField("setLogin", (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        WWW www = new WWW(inactiveURL, form);
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

    public void startPlay()
    {
        Debug.Log("test1");
        StartCoroutine(setActive());
    }

    public void stopPlay()
    {
        if (username != null && username != "")
            StartCoroutine(setInactive());
    }
}
