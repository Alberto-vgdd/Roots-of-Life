using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterBehaviour : MonoBehaviour {
	public Image textMask;
	public GameObject nextChapter;
	public static float moveSpeed = 25f;
	public float waitTime = 10.0f;
	private int state = 0;

	// Update is called once per frame
	void Update () {
		if (state == 1) {
			textMask.fillAmount += 1.0f / waitTime * Time.deltaTime;
			nextChapter.GetComponent<ChapterBehaviour> ().move (false);
			if (textMask.fillAmount >= 1) {
				state = 2;
				nextChapter.GetComponent<ChapterBehaviour> ().fixposition (false);
			}
		}
		if (state == 3) {
			textMask.fillAmount -= 1.0f / waitTime * Time.deltaTime;
			nextChapter.GetComponent<ChapterBehaviour> ().move (true);
			if (textMask.fillAmount <= 0) {
				state = 0;
				nextChapter.GetComponent<ChapterBehaviour> ().fixposition (true);
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

	private void fixposition(bool up) {
		if (up)
			setY (-46);
		else
			setY (-440);
	}

	private void move(bool up) {
		if (up)
			setY (getY () + moveSpeed);
		else
			setY (getY () - moveSpeed);
		
		if (nextChapter != null)
			nextChapter.GetComponent<ChapterBehaviour> ().move (up);
	}

	private void setY(float newY) {
		Vector2 position = GetComponent<RectTransform> ().anchoredPosition;
		position.y = newY;
		GetComponent<RectTransform> ().anchoredPosition = position;
	}

	private float getY() {
		return GetComponent<RectTransform> ().anchoredPosition.y;
	}
}