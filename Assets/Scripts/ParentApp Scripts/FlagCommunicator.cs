using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCommunicator : MonoBehaviour {
    string URL = "http://143.176.117.92/roots-of-life/setFlag.php";
    public string flag, valueOne, valueTwo;
    private bool switcher;

    // Use this for initialization
    void Start () {
        StartCoroutine(form());
	}
	
	// Update is called once per frame
	void Update () {
	}

    IEnumerator form()
    {
        WWWForm form = new WWWForm();
        form.AddField("flagName", flag);
        if (switcher)
            form.AddField("flagValue", valueOne);
        else
            form.AddField("flagValue", valueTwo);
        switcher = !switcher;

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

    public void setFlag()
    {
        StartCoroutine(form());
    }
}
