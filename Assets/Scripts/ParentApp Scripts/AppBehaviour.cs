using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppBehaviour : MonoBehaviour {
	public GameObject popup;

	public Scrollbar scrollbar;
	public GameObject playerInfo;

	public InputField inputField;
	public GameObject objectivesList;
	public Toggle objectiveObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// Calculate position of playername and status
		float yValue = -435 - (140 * (scrollbar.value * -1 + 1));
		Vector2 position = GetComponent<RectTransform> ().anchoredPosition;
		position.y = yValue;
		playerInfo.GetComponent<RectTransform> ().anchoredPosition = position;
	}

	public void showPopup(string screen) {
		if (screen == null || screen == "")
			return;
		
		popup.SetActive (true);
		for (int i = 1; i < 5; i++) {
			popup.transform.GetChild (i).gameObject.SetActive (false);
		}
		Debug.Log ("screen = " + screen);
		if (screen == "DayInfo")
			popup.transform.GetChild (1).gameObject.SetActive (true);
		if (screen == "ThreeDayInfo")
			popup.transform.GetChild (2).gameObject.SetActive (true);
		if (screen == "WeekInfo")
			popup.transform.GetChild (3).gameObject.SetActive (true);
		if (screen == "AddObjective")
			popup.transform.GetChild (4).gameObject.SetActive (true);
	}

	public void clearObjective(Toggle objective) {

	}

	public void addNewObjective() {
		if (inputField.text == "")
			return;
		
		popup.SetActive (false);
		Toggle newObjective = Instantiate (objectiveObject);
		newObjective.name = "NewObjective";
		newObjective.transform.SetParent (objectivesList.transform);
		newObjective.GetComponentInChildren<Text>().text = inputField.text;
		inputField.text = "";
		sortObjectives ();
	}

	public void sortObjectives() {
		int count = objectivesList.transform.childCount;
		for (int i = 0; i < count; i++) {
			GameObject objective = objectivesList.transform.GetChild (i).gameObject;
			Vector2 position = objective.GetComponent<RectTransform> ().anchoredPosition;
			position.x = 0;
			position.y = 190 - (i * 30);
			objective.GetComponent<RectTransform> ().anchoredPosition = position;
		}
	}
}
