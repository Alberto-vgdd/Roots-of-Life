using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterBehaviour : MonoBehaviour {
	public Image textMask;
	public float waitTime = 10.0f;
	private int state = 0;

	// Update is called once per frame
	void Update () {
		if (state == 1) {
			textMask.fillAmount += 1.0f / waitTime * Time.deltaTime;
			if (textMask.fillAmount >= 1)
				state = 2;
		}
		if (state == 3) {
			textMask.fillAmount -= 1.0f / waitTime * Time.deltaTime;
			if (textMask.fillAmount <= 0)
				state = 0;
		}
	}

	public void press() {
		float width = 360;
		float height = textMask.GetComponentInChildren<Text> ().preferredHeight;
		textMask.rectTransform.sizeDelta = new Vector2 (width, height);
		Vector2 position = gameObject.GetComponent<RectTransform> ().position;
		position.x = 0;
		position.y -= 705;
		textMask.rectTransform.anchoredPosition = position;

		if (state == 0)
			state = 1;
		if (state == 2)
			state = 3;
	}
}