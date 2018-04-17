using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterBehaviour : MonoBehaviour {
	public Image textMask;
	public GameObject nextChapter;
	public float moveSpeed = 25f;
	public float waitTime = 10.0f;
	private int state = 0;

	// Update is called once per frame
	void Update () {
		if (state == 1) {
			textMask.fillAmount += 1.0f / waitTime * Time.deltaTime;
			Vector2 position = nextChapter.GetComponent<RectTransform> ().anchoredPosition;
			position.y -= moveSpeed;
			nextChapter.GetComponent<RectTransform> ().anchoredPosition = position;
			if (textMask.fillAmount >= 1) {
				state = 2;
				position = nextChapter.GetComponent<RectTransform> ().anchoredPosition;
				position.y = -440;
				nextChapter.GetComponent<RectTransform> ().anchoredPosition = position;
			}
		}
		if (state == 3) {
			textMask.fillAmount -= 1.0f / waitTime * Time.deltaTime;
			Vector2 position = nextChapter.GetComponent<RectTransform> ().anchoredPosition;
			position.y += moveSpeed;
			nextChapter.GetComponent<RectTransform> ().anchoredPosition = position;
			if (textMask.fillAmount <= 0) {
				state = 0;
				position = nextChapter.GetComponent<RectTransform> ().anchoredPosition;
				position.y = -46;
				nextChapter.GetComponent<RectTransform> ().anchoredPosition = position;
			}
		}
	}

	public void press() {
		//float width = 360;
		//float height = textMask.GetComponentInChildren<Text> ().preferredHeight;
		//textMask.rectTransform.sizeDelta = new Vector2 (width, height);

		if (state == 0)
			state = 1;
		if (state == 2)
			state = 3;
	}
}