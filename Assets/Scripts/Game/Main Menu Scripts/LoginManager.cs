using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoginManager : MonoBehaviour
{
    private string requestURL = "http://62.131.170.46/roots-of-life/loginRequest.php";
    private string insertURL = "http://62.131.170.46/roots-of-life/parentInsert.php";
    private string userURL = "http://62.131.170.46/roots-of-life/profileSelect.php";
	private string pingURL = "http://62.131.170.46/roots-of-life/profileGetPing.php";

	[Header("Log-in Input")]
	public GameObject loginElement;
	public InputField nameInput;
	public InputField passInput;
	public InputField passControl;
	public Toggle rememberToggle;
	public Toggle automaticToggle;
	public GameObject buttons;

    private string username;
    private string password;

	private bool loggedIn = false;
    private int parentID;
    private List<string> users;

    // Use this for initialization
    void Start () {

        if (PlayerPrefs.GetInt("remember") == 1)
        {
            rememberToggle.isOn = true;
            username = PlayerPrefs.GetString("username");
            nameInput.text = username;
        }

        if (PlayerPrefs.GetInt("automatic") == 1)
        {
            password = PlayerPrefs.GetString("password");
			automaticToggle.isOn = true;
			StartCoroutine(loginRequest());
        }
    }
	
	// Update is called once per frame
	void Update () {

    }

	public void logIn() {

		if (nameInput.text == null || nameInput.text == "")
			return;
		if (passInput.text == null || passInput.text == "")
			return;
		username = nameInput.text;
		password = passInput.text;

		StartCoroutine(loginRequest());
		if (!loggedIn)
			return;
	}

    public void logOut()
    {
		parentID = -1;
		loggedIn = false;

		loginElement.gameObject.SetActive (true);
		buttons.SetActive (false);



        /*users.Clear();
        foreach (Transform child in usersList.transform)
        {
            if (child.gameObject.name == "SampleText")
                continue;
            GameObject.Destroy(child.gameObject);
        }*/
    }

	public void register() {

		if (passInput.text != passControl.text)
			// TODO passwords do not match
			return;

		// TODO account created
		StartCoroutine (registerParent());
		StartCoroutine (loginRequest ());
	}

    IEnumerator loginRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("setUsername", username);
        form.AddField("setPassword", password);

        WWW www = new WWW(requestURL, form);
        yield return www;
        int result;
		if (!string.IsNullOrEmpty (www.text))
			result = int.Parse (www.text);
		else
			yield break;

		if (result > 0) {
			if (rememberToggle.isOn) {
				PlayerPrefs.SetInt("remember", 1);
				PlayerPrefs.SetString("username", username);
			} else {
				PlayerPrefs.SetInt("remember", 0);
				PlayerPrefs.SetString("username", "");
			}
			if (automaticToggle.isOn) {
				PlayerPrefs.SetInt("automatic", 1);
				PlayerPrefs.SetString("password", password);
			} else {
				PlayerPrefs.SetInt("automatic", 0);
				PlayerPrefs.SetString("password", "");
			}

			loggedIn = true;
			parentID = result;

			loginElement.gameObject.SetActive (false);
			buttons.SetActive (true);

			// load users
		}
		if (result == -1) 
			MainMenu.showMessage ("Wrong password, please try again.");
		else if (result == -2) 
			MainMenu.showMessage ("Account doesn't exist, please register.");
	}

	IEnumerator registerParent()
	{
		WWWForm form = new WWWForm();
		form.AddField("setUsername", username);
		form.AddField("setPassword", password);
		WWW www = new WWW(insertURL, form);
		yield return www;
	}

    IEnumerator loadUsers()
    {
        WWWForm form = new WWWForm();
        form.AddField("setParentID", parentID);
        WWW www = new WWW(userURL, form);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log("error: " + www.error);
        }
        else
        {
            Debug.Log("result: " + www.text);
        }

        users = new List<string>();
        if (www.text != "")
        {
            string userString = www.text.TrimEnd(';');
            foreach (string user in userString.Split(';'))
            {
                string name = user.Split(',')[0];
                bool active = (user.Split(',')[1] == "1");
                int ping = int.Parse(user.Split(',')[3]);
                
                int pingbarrier = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds - 60;

                if (ping < pingbarrier && active)
                {
                    //playDetector.stopPlay(name);
                    active = false;
                }
                if (!active)
                    users.Add(name);
            }
        }
        //displayUsers();
    }

	/*
    void displayUsers()
    {
        if (users.Count == 0)
        {
            StartCoroutine(error(5));
            return;
        }
        
        float y = sampleText.GetComponent<RectTransform>().anchoredPosition.y;
        float height = sampleText.GetComponent<RectTransform>().sizeDelta.y;
        int i = 0;
        foreach (string user in users)
        {
            Text userText = Instantiate(sampleText).GetComponent<Text>();
            userText.transform.SetParent(usersList.transform);
            userText.text = user;
            userText.name = user;
            userText.gameObject.SetActive(true);

            RectTransform rT = userText.GetComponent<RectTransform>();
            rT.anchoredPosition = sampleText.GetComponent<RectTransform>().anchoredPosition;
            rT.localScale = new Vector3(1, 1, 1);
            rT.anchoredPosition = new Vector2(rT.anchoredPosition.x, y - ((height - 10) * i));

            EventTrigger eT = userText.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData)=>this.setUsername(user));
            eT.triggers.Add(entry);

            i++;
        }
    }

    void setUsername(string username)
    {
        usernameText.text = username;
        playDetector.username = username;
        gameObject.SetActive(false);

        // Enable Play, Settings and Credits button
        // Store the name into the GlobalData file of the game.
        GlobalData.username = username;
        mainMenuScript.EnableGameButtons();
    }

    IEnumerator error(int error)
    {
        if (error == 1)
            errorField.GetComponentInChildren<Text>().text = "Wrong password.";
        if (error == 2)
            errorField.GetComponentInChildren<Text>().text = "Account not found, please register.";
        if (error == 3)
            errorField.GetComponentInChildren<Text>().text = "Passwords don't match.";
        if (error == 4)
            errorField.GetComponentInChildren<Text>().text = "Account created. You can now log in.";
        if (error == 5)
            errorField.GetComponentInChildren<Text>().text = "No users found, use the parent app to add users!";

        errorField.SetActive(true);
        yield return new WaitForSeconds(3);
        errorField.SetActive(false);
    }*/
}
