﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayDetector : MonoBehaviour {

    private string activeURL = "http://62.131.170.46/roots-of-life/setActive.php";
    private string inactiveURL = "http://62.131.170.46/roots-of-life/setInactive.php";
    private string pingURL = "http://62.131.170.46/roots-of-life/userPing.php";

    public string username;
    public bool liveUpdate;

    private void Awake()
    {
        username = GlobalData.username;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (liveUpdate)
            StartCoroutine(pingTimer());
    }

    IEnumerator pingTimer()
    {
        liveUpdate = false;
        StartCoroutine(ping());
        yield return new WaitForSeconds(60);
        liveUpdate = true;
    }

    public void setUsername(string username)
    {
        this.username = username;
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

    IEnumerator ping()
    {
        WWWForm form = new WWWForm();
        form.AddField("setUsername", username);
        form.AddField("setPing", (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        WWW www = new WWW(pingURL, form);
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
        StartCoroutine(setActive());
    }

    public void stopPlay()
    {
        if (username != null && username != "")
            StartCoroutine(setInactive());
    }

    public void stopPlay(string username)
    {
        this.username = username;
        StartCoroutine(setInactive());
    }
}
