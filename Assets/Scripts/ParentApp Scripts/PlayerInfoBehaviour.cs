using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoBehaviour : MonoBehaviour {
	public Scrollbar bar;

	void Update () {
		float yValue = -435 - (140 * (bar.value * -1 + 1));
		Vector2 position = GetComponent<RectTransform> ().anchoredPosition;
		position.y = yValue;
		GetComponent<RectTransform> ().anchoredPosition = position;
	}
}
