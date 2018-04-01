using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveBehaviour : MonoBehaviour {
	public GameObject objectiveScreen;
	public InputField inputField;
	public Toggle objectiveObject;

	public void addNewObjective() {
		if (inputField.text == "")
			return;
		objectiveScreen.SetActive (false);
		Toggle newObjective = Instantiate (objectiveObject);
		newObjective.name = "NewObjective";
		newObjective.transform.SetParent (gameObject.transform);
		Vector2 position = gameObject.GetComponent<RectTransform> ().anchoredPosition;
		position.y = 190;
		newObjective.GetComponent<RectTransform> ().anchoredPosition = position;
		newObjective.GetComponentInChildren<Text> ().text = inputField.text;
	}
}
