using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollerBehaviour : MonoBehaviour {
	public Scrollbar verticalBar;
	public float waitTime = 30.0f; // delay for hiding scroller
	private float idleTime = 0.0f; // time since last use
	private bool active = false; // true if scroller is used on current frame
	private int state = 0; // 0 = shown, 1 = hiding, 2 = hidden, 3 = showing
	private float showX; // shown position
	private float hideX; // hidden position

	void Start () {
		showX = verticalBar.GetComponent<RectTransform> ().anchoredPosition.x;
		hideX = 30 + showX;
		hideScrollbar ();
	}
	
	// Update is called once per frame
	void Update () {
		if (active)
			showScrollbar ();
		else
			idleTime += Time.deltaTime;
		if (idleTime >= waitTime)
			hideScrollbar ();
		active = false;

		if (state == 2 || state == 0)
			return;

		Vector2 position = verticalBar.GetComponent<RectTransform> ().anchoredPosition;
		if (state == 1) {
			position.x += 1;
			if (position.x >= hideX)
				state = 2;
		}
		if (state == 3) {
			position.x -= 1;
			if (position.x <= showX)
				state = 0;
		}
		verticalBar.GetComponent<RectTransform> ().anchoredPosition = position;
	}

	public void onScrollUse() {
		active = true;
	}

	private void hideScrollbar() {
		if (state == 2)
			return;
		state = 1;
	}

	private void showScrollbar() {
		idleTime = 0.0f;
		if (state == 0)
			return;
		state = 3;
	}
}
