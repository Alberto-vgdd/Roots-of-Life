using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SnapscrollBehaviour : Scrollbar {
	private float oldValue = 1.0f;
	private bool dragging = false;
	private float scrollSpeed = 0.05f;
	public bool state = true; // true = up, false = down

	// Update is called once per frame
	void Update () {
		if (dragging) 
			return;
		
		if (state) {
			if (value > 0.8f)
				value += scrollSpeed;
			else
				value -= scrollSpeed;
			if (value <= 0)
				state = false;
		} else {
			if (value > 0.2f) 
				value += scrollSpeed;
			else
				value -= scrollSpeed;
			if (value >= 1)
				state = true;
		}
	}

	public void startDrag() {
		dragging = true;
	}

	public void endDrag() {
		dragging = false;
	}
}
