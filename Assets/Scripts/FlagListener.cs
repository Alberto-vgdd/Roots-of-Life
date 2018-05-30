using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlagListener : MonoBehaviour {
    string URL = "http://143.176.117.92/roots-of-life/getFlags.php";
    public string flag;
    public int valueReadOnly;
    protected int value;
    protected string[] flagData;

    public UnityEvent flagValueChange;

	// Use this for initialization
	void Start () {
    }

    IEnumerator loadFlags()
    {
        WWW flags = new WWW(URL);
        yield return flags;
        string flagsDataString = flags.text.TrimEnd(';');
        flagData = flagsDataString.Split(';');
        foreach (string data in flagData)
        {
            //Debug.Log(data);
            string name = data.Split(',')[0];
            string sValue = data.Split(',')[1];
            if (name == flag)
            {
                int oldValue = value;
                value = int.Parse(sValue);
                if (oldValue != value)
                    onFlagChange();
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(loadFlags());
        valueReadOnly = value;
	}

    void onFlagChange()
    {
        Debug.Log("event called");
        flagValueChange.Invoke();
    }

    public int getValue()
    {
        return value;
    }
}
