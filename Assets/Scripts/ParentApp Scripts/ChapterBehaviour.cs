using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterBehaviour : MonoBehaviour {
	public Image textMask;
	public GameObject nextChapter;
	public float waitTime = 10.0f;
	public float movespeed = 32f;
	private int state = 0;

	// Update is called once per frame
	void Update () {
		if (state == 1) {
			textMask.fillAmount += 1.0f / waitTime * Time.deltaTime;
			Vector2 position = nextChapter.GetComponent<RectTransform> ().anchoredPosition;
			position.y -= movespeed;
			nextChapter.GetComponent<RectTransform> ().anchoredPosition = position;
			if (textMask.fillAmount >= 1)
				state = 2;
		}
		if (state == 3) {
			textMask.fillAmount -= 1.0f / waitTime * Time.deltaTime;
			Vector2 position = nextChapter.GetComponent<RectTransform> ().anchoredPosition;
			position.y += movespeed;
			nextChapter.GetComponent<RectTransform> ().anchoredPosition = position;
			if (textMask.fillAmount <= 0)
				state = 0;
		}
	}

	public void press() {
		if (state == 0)
			state = 1;
		if (state == 2)
			state = 3;
	}
}