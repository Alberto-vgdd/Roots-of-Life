using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoginManager : MonoBehaviour
{
    private string requestURL = "http://62.131.170.46/roots-of-life/loginRequest.php";
    private string insertURL = "http://62.131.170.46/roots-of-life/insertParent.php";
    private string userURL = "http://62.131.170.46/roots-of-life/userSelect.php";

    [Header("Main Menu Script")]
    public MainMenuScript mainMenuScript;

    [Header("Logging Elements")]
    public GameObject parentInput;
    public InputField nameInput;
    public InputField passInput;
    public InputField passControl;
    public Button cancelButton;
    public Toggle rememberToggle;
    public Toggle automaticToggle;
    public GameObject errorField;
    public GameObject childUserSelect;
    public GameObject usersList;
    public Text sampleText;
    public Text usernameText;

    private string username;
    private string password;

    private bool register;
    private bool loggedIn;
    private int parentID;
    private List<string> users;

    // Use this for initialization
    void Start () {

        loggedIn = false;
        
        // Disable Game Buttons
        mainMenuScript.DisableGameButtons();

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
        }

        if (automaticToggle.isOn)
            StartCoroutine(loginRequest());

        register = false;
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void logOut()
    {
        parentInput.SetActive(true);
        childUserSelect.SetActive(false);
        parentID = -1;
        //profileSelector.unloadUsers();

        // Disable Game Buttons
        mainMenuScript.DisableGameButtons();
    }

    private void registerMode()
    {
        register = true;
        rememberToggle.gameObject.SetActive(false);
        automaticToggle.gameObject.SetActive(false);
        passControl.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);
    }

    public void loginMode()
    {
        register = false;
        rememberToggle.gameObject.SetActive(true);
        automaticToggle.gameObject.SetActive(true);
        passControl.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
    }

    public void onPress()
    {
        if (nameInput.text == null || nameInput.text == "")
            return;
        if (passInput.text == null || passInput.text == "")
            return;
        username = nameInput.text;
        password = passInput.text;

        if (register)
        {
            // do register stuff
            if (passInput.text != passControl.text)
            {
                StartCoroutine(error(3));
                return;
            }

            StartCoroutine(error(4));
            StartCoroutine(insertParent());
            loginMode();

            return;
        }

        StartCoroutine(loginRequest());
        if (!loggedIn)
            return;
    }

    private void loginSuccesful()
    {
        loggedIn = true;

        if (rememberToggle.isOn)
        {
            PlayerPrefs.SetInt("remember", 1);
            PlayerPrefs.SetString("username", username);
        }
        else
        {
            PlayerPrefs.SetInt("remember", 0);
            PlayerPrefs.SetString("username", "");
        }
        if (automaticToggle.isOn)
        {
            PlayerPrefs.SetInt("automatic", 1);
            PlayerPrefs.SetString("password", password);
        }
        else
        {
            PlayerPrefs.SetInt("automatic", 0);
            PlayerPrefs.SetString("password", "");
        }

        //profileSelector.StartCoroutine(profileSelector.loadUsers(parentID));

        // load profiles in profileselector
        StartCoroutine(loadUsers());

        parentInput.SetActive(false);
        childUserSelect.SetActive(true);
    }

    IEnumerator insertParent()
    {
        WWWForm form = new WWWForm();
        form.AddField("setUsername", username);
        form.AddField("setPassword", password);

        WWW www = new WWW(insertURL, form);
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

    IEnumerator loginRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("setUsername", username);
        form.AddField("setPassword", password);

        WWW www = new WWW(requestURL, form);
        yield return www;
        int result;
        if (!string.IsNullOrEmpty(www.error))
        {
            //Debug.Log("error: " + www.error);
            yield break;
        }
        else
        {
            //Debug.Log("result: " + www.text);
            result = int.Parse(www.text);
        }

        if (result == -1)
        {
            // wrong password
            passInput.text = "";
            StartCoroutine(error(1));
        }
        else if (result == -2)
        {
            // enter register mode
            registerMode();
            StartCoroutine(error(2));
        }
        else
        {
            Debug.Log("result:" + result);
            // logged in;
            parentID = result;
            loginSuccesful();
        }
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
                if (user.Split(',')[1] == "1")
                    continue;
                users.Add(user.Split(',')[0]);
            }
        }
        displayUsers();
    }

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
        usernameText.gameObject.SetActive(true);
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
    }
}
