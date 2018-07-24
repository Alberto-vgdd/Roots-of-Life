using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlagManager : MonoBehaviour {
	public static FlagManager main;

	public List<string> flagnames;
	public List<UnityEvent> events;
	public List<int> valuesReadOnly;

	private string[] flagData;
	private Dictionary<string, int> flags; // Stores flags on runtime to compare for updates

	private static string setURL = "http://62.131.170.46/roots-of-life/profileSetFlag.php";
	private static string getURL = "http://62.131.170.46/roots-of-life/profileGetFlags.php";

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

	// Use this for initialization
	void Start () 
	{
		main = this;

		// Load flags and store them in the dictionary
		flags = new Dictionary<string, int>();
		foreach (string name in flagnames)
			flags.Add(name, 0);

		StartCoroutine (compareLoop ());
	}

	public static void setFlag(string flag, int value) {
		main.StartCoroutine (setForm (flag, value));
	}

	public static int getFlag(string flag) {
		IEnumerator flags = getForm ();
		while (flags.MoveNext ());

		if (main.flagData == null)
			return 0;

		foreach (string data in main.flagData)
			if (data.Split (':') [0] == flag) {
				return int.Parse (data.Split (':') [1]);
				break;
			}
		return 0;
	}

	static IEnumerator setForm(string flag, int value) {
		WWWForm form = new WWWForm ();
		form.AddField ("userID", GlobalData.userid);
		form.AddField ("flagName", flag);
		form.AddField ("flagValue", value);

		WWW www = new WWW (setURL, form);
		yield return www;
	}

	static IEnumerator getForm() {
		WWWForm form = new WWWForm ();
		form.AddField ("setUserID", GlobalData.userid);

		WWW www = new WWW (getURL, form);
		yield return www;

		while (!www.isDone)
			yield return null;

		if (www.text != "")
			main.flagData = www.text.TrimEnd (',').Split (',');
		else
			main.flagData = null;
	}

	// Loop that checks for updates in the flags on the database
	IEnumerator compareLoop() {

		// Make sure to always repeat
		while (true) {

			// Load flags into flagdata
			IEnumerator flags = getForm ();
			while (flags.MoveNext ());

			// Make sure data is loaded
			if (main.flagData != null) {

				// Iterate over each flag
				foreach (string data in flagData)
				{
					Debug.Log("retrieved: " + data);

					// Store flag name and new/current value
					string name = data.Split(':')[0];
					int value = int.Parse(data.Split(':')[1]);
					bool invoke = false;

					// Check if flag is stored in dictionary
					if (main.flags.ContainsKey(name))
					{
						// Check value to see if it is changed
						int i = flagnames.IndexOf(name);
						if (main.flags [name] != value)
							invoke = true;

						// Update in dictionary
						main.flags[name] = value;
						valuesReadOnly[i] = value;

						if (invoke)
							events [i].Invoke ();
					}
				}
			}

			yield return new WaitForSeconds (1);
		}
	}
}
