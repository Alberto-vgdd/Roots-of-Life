using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SQL

public class PlayerinfoBehaviour : MonoBehaviour {
	public Scrollbar scrollbar;
	private float defaulty;

	void Start() {
		defaulty = gameObject.GetComponent<RectTransform> ().anchoredPosition.y;
		gameObject.transform.GetChild (0).GetComponent<Text> ().text = "test";
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void move() {
		Vector2 position = gameObject.GetComponent<RectTransform> ().anchoredPosition;
		position.y = defaulty + -820 * (scrollbar.value * -1 + 1);
		gameObject.GetComponent<RectTransform> ().anchoredPosition = position;
	}
}
