using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public static MainMenu main;
	private string requestURL = "http://62.131.170.46/roots-of-life/loginRequest.php";
	private string insertURL = "http://62.131.170.46/roots-of-life/parentInsert.php";
	private string deleteURL = "http://62.131.170.46/roots-of-life/profileDelete.php";
	private string userURL = "http://62.131.170.46/roots-of-life/profileSelect.php";

	[Header("Login Manager Input")]
	public InputField nameInput;
	public InputField passInput;
	public InputField passControl;
	public Toggle rememberToggle;
	public Toggle automaticToggle;

	[Header("Profile Selector Input")]
	public Image profileImage;
	public Text profileName;
	public Text profileText;

	[Header("Popup Manager Input")]
	public GameObject popupView;
	public Text popupText;

	[Header("Menu Manager Input")]
	public GameObject loginScreen;
	public GameObject titleScreen;
	public GameObject selectScreen;
    public GameObject settingsScreen;
	public GameObject UIPanel;

	// Log in manager
	private string username;
	private string password;

	private bool loggedIn = false;
	private int parentID;

	// Profile manager
	private List<Profile> profiles;
	private int selected = 0;

	// Menu manager
	private List<GameObject> screens;

	// Use this for initialization
	void Start () {
		main = this;

		if (PlayerPrefs.GetInt("remember") == 1)
		{
			username = PlayerPrefs.GetString("username");
			rememberToggle.isOn = true;
			nameInput.text = username;
		}

		if (PlayerPrefs.GetInt("automatic") == 1)
		{
			password = PlayerPrefs.GetString("password");
			automaticToggle.isOn = true;
			StartCoroutine(loginRequest());
		}

		main.screens = new List<GameObject> ();
		main.screens.Add (main.loginScreen);
		main.screens.Add (main.titleScreen);
		main.screens.Add (main.selectScreen);
        main.screens.Add (main.settingsScreen);

		if (!loggedIn)
			showScreen ("login", true);
	}

	// -- Login Manager --
	public void logIn() {

		if (nameInput.text == null || nameInput.text == "")
			return;
		if (passInput.text == null || passInput.text == "")
			return;
		username = nameInput.text;
		password = passInput.text;

		StartCoroutine(loginRequest());
	}

	public void logOut()
	{
		parentID = -1;
		loggedIn = false;

		showScreen ("login", true);
	}

    public void register() {

        if (passInput.text != passControl.text) { 
            showMessage("Your passwords do not match, please try again.");
            return;
        }   

		showMessage ("Your account was successfully created!");
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
                nameInput.text = "";
                username = "";
            }
			if (automaticToggle.isOn) {
				PlayerPrefs.SetInt("automatic", 1);
				PlayerPrefs.SetString("password", password);
			} else {
				PlayerPrefs.SetInt("automatic", 0);
				PlayerPrefs.SetString("password", "");
                passInput.text = "";
                password = "";
			}

			loggedIn = true;
			parentID = result;

			StartCoroutine(loadProfiles());

			showScreen ("select", true);
		}
		if (result == -1) 
			showMessage ("Wrong password, please try again.");
		else if (result == -2) 
			showMessage ("Account doesn't exist, please register.");
	}

	IEnumerator registerParent()
	{
		WWWForm form = new WWWForm();
		form.AddField("setUsername", username);
		form.AddField("setPassword", password);
		WWW www = new WWW(insertURL, form);
		yield return www;
	}

	// -- Profile Manager -- 
	IEnumerator loadProfiles()
	{
		WWWForm form = new WWWForm();
		form.AddField("setParentID", parentID);
		WWW www = new WWW(userURL, form);
		yield return www;

		profiles = new List<Profile>();
		if (www.text != "")
		{
			foreach (string user in www.text.TrimEnd(';').Split(';'))
			{
				string name = user.Split(',')[0];
				bool active = (user.Split(',')[1] == "1");
				int ping = int.Parse(user.Split(',')[3]);
                string imageURL = user.Split(',')[4];
                int id = int.Parse(user.Split(',')[5]);

				int pingbarrier = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds - 60;

				if (ping < pingbarrier && active)
				{
                    FlagManager.stopPlay(this, id);
					active = false;
				}

				if (!active)
                    profiles.Add(new Profile(name, id, imageURL));
			}
		}
		showSelectedProfile ();
    }

    // Show next profile in profile selector
    public void nextProfile() {
		if (selected + 1 == profiles.Count)
			selected = 0;
		else
			selected++;
        showSelectedProfile ();
	}

    // Show previous profile in profile selector
	public void previousProfile()
    {
        if (selected == 0)
			selected = (profiles.Count - 1);
		else
			selected--;
        showSelectedProfile ();
	}

    // Select a new profile
	public void select() {
		profileText.text = profiles [selected].username;
		showScreen ("title", false);
        GlobalData.username = profiles[selected].username;
        GlobalData.userid = profiles[selected].id;
	}

    // Display profile in the profile selector
	private void showSelectedProfile() {
		profileName.text = profiles [selected].username;
        // change image
	}

    // Show profile selector again to select a new user
    public void selectNewProfile()
    {
        showScreen("select", true);
    }

    // -- Menu Manager -- 

    // Change menu screen
	private void showScreen(string screen, bool panel) {
		foreach (GameObject s in screens)
			s.SetActive (false);
		screens [convertScreen (screen)].SetActive (true);
		if (panel)
			UIPanel.SetActive (true);
		else
			UIPanel.SetActive (false);
	}

    // Convert a menu screen name to its corresponding index in the list
	private int convertScreen(string screen) {
		if (screen == "login")
			return 0;
		if (screen == "title")
			return 1;
		if (screen == "select")
			return 2;
        if (screen == "settings")
            return 3;
		return -1;
	}

	// -- Popup Manager --
	public static void showMessage(string message) {
		main.StartCoroutine (coroutine (message));
	}

	static IEnumerator coroutine(string message) {
		main.popupView.SetActive (true);
		main.popupText.text = message;
		yield return new WaitForSeconds (3);
		main.popupView.SetActive (false);
    }

    // Title Screen

    public void Play()
    {
        string startscene = "Main_Area_2.0";
        SceneManager.LoadScene(startscene);
        FlagManager.startPlay(this);
    }

    public void Settings()
    {
        showScreen("settings", true);
    }

    public void Credits()
    {
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void TitleScreen()
    {
        showScreen("title", false);
    }
}
