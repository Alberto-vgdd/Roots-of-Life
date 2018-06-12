using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class FlagListener : MonoBehaviour {
	string URL = "http://62.131.170.46/roots-of-life/getFlags.php";

    public List<string> flagnames;
    public List<UnityEvent> events;
    public List<int> valuesReadOnly;

    private string[] flagData;
    private Dictionary<string, int> flags;

	// Use this for initialization
	void Start () {
        flags = new Dictionary<string, int>();
        foreach (string name in flagnames)
            flags.Add(name, 0);
		Debug.Log ("flag count:" + flags.Count);
    }

    private void OnValidate()
    {
        // Set events list size equal to flag names size, but save the data in the list
        if (events.Count != flagnames.Count)
        {
            UnityEvent[] newArray = new UnityEvent[flagnames.Count];
            for (int i = 0; i < flagnames.Count; i++)
            {
                if (events.Count <= i)
                    continue;
                newArray[i] = events[i];
            }
            events = new List<UnityEvent>(newArray);
        }
        valuesReadOnly = new List<int>(new int[flagnames.Count]);
    }

    IEnumerator loadFlags()
    {
        WWW flagloader = new WWW(URL);
        yield return flagloader;
        string flagsDataString = flagloader.text.TrimEnd(';');
        flagData = flagsDataString.Split(';');
    }
	
	// Update is called once per frame
	void Update () {
		StartCoroutine(loadFlags());
		if (flagData == null)
			return;

		bool invoke = false;
		// Iterate over all flag values obtained from url
		foreach (string data in flagData)
		{
			//Debug.Log ("retrieved string:" + data);
			// Store flag name and flag value
			string name = data.Split(',')[0];
			int value = int.Parse(data.Split(',')[1]);

			// Check if flag is checked for
			if (flags.ContainsKey(name))
			{
				//Debug.Log ("checking flag:" + name);
				int i = flagnames.IndexOf(name);
				//Debug.Log ("index:" + i);

				// Check for value update
				if (flags [name] != value)
					invoke = true;

				// Update value
				flags[name] = value;
				valuesReadOnly[i] = value;

				if (invoke)
					events [i].Invoke ();
			}
		}
	}

    public int getValue(string flag)
    {
        if (!flags.ContainsKey(flag))
            return -1;
        return flags[flag];
    }
}
