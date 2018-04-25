using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerinfoBehaviour : MonoBehaviour {
	public Scrollbar scrollbar;
	private float defaulty;

	void Start() {
		defaulty = gameObject.GetComponent<RectTransform> ().anchoredPosition.y;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 position = gameObject.GetComponent<RectTransform> ().anchoredPosition;
		position.y = defaulty + -820 * (scrollbar.value * -1 + 1);
		gameObject.GetComponent<RectTransform> ().anchoredPosition = position;
	}
}
