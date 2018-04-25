using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SnapscrollBehaviour : Scrollbar {
	private float oldValue = 1.0f;
	private bool dragging = false;

	// Update is called once per frame
	void Update () {
		if (dragging)
			return;
		
		float scrollDelta = oldValue - value;
		oldValue = value;
		if (scrollDelta > 0.005f || scrollDelta < -0.005f)
			return;

		if (value > 0.5f)
			value += 0.025f;
		else
			value -= 0.025f;
	}

	public void startDrag() {
		dragging = true;
	}

	public void endDrag() {
		dragging = false;
	}
}
