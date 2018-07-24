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
	private string pingURL = "http://62.131.170.46/roots-of-life/profileGetPing.php";

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
	public GameObject menuScreen;
	public GameObject selectScreen;
	public GameObject UIPanel;

	// Log in manager
	private string username;
	private string password;

	private bool loggedIn = false;
	private int parentID;

	// Profile manager
	private List<string> profiles;
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
		main.screens.Add (main.menuScreen);
		main.screens.Add (main.selectScreen);

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

		if (passInput.text != passControl.text)
			showMessage ("Your passwords do not match, please try again.");
			return;

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

			StartCoroutine(loadProfiles());

			showScreen ("select", true);
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

	// -- Profile Manager -- 
	IEnumerator loadProfiles()
	{
		WWWForm form = new WWWForm();
		form.AddField("setParentID", parentID);
		WWW www = new WWW(userURL, form);
		yield return www;

		profiles = new List<string>();
		if (www.text != "")
		{
			foreach (string user in www.text.TrimEnd(';').Split(';'))
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
					profiles.Add(name);
			}
		}
		showSelectedProfile ();
	}

	IEnumerator deleteProfile() {
		WWWForm form = new WWWForm ();
		form.AddField ("setChildID", 0); // set id of selected
		WWW www = new WWW(deleteURL, form);
		yield return www;

		if (selected == 0)
			selected = (profiles.Count - 1);
		else
			selected--;
		StartCoroutine (loadProfiles ());
	}

	public void nextProfile() {
		if (selected + 1 == profiles.Count)
			selected = 0;
		else
			selected++;
		showSelectedProfile ();
	}

	public void previousProfile() {
		if (selected == 0)
			selected = (profiles.Count - 1);
		else
			selected--;
		showSelectedProfile ();
	}

	public void select() {
		profileText.text = profiles [selected];
		showScreen ("menu", false);
	}

	private void showSelectedProfile() {
		profileName.text = profiles [selected];
	}

	public void Play() {
		string startscene = "Main Area 2.0";
		SceneManager.LoadScene(startscene);
	}

	public void Settings() {
	}

	public void Credits() {
	}

	public void Quit() {
		Application.Quit();
	}

	private void showScreen(string screen, bool panel) {
		foreach (GameObject s in screens)
			s.SetActive (false);
		screens [convertScreen (screen)].SetActive (true);
		if (panel)
			UIPanel.SetActive (true);
		else
			UIPanel.SetActive (false);
	}

	private int convertScreen(string screen) {
		if (screen == "login")
			return 0;
		if (screen == "menu")
			return 1;
		if (screen == "select")
			return 2;
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
}
