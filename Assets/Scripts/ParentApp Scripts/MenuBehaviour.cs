using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBehaviour : ScrollRectEx {
	public GameObject menuBar;
	private Screen activeScreen;
	private bool dragging = false;
	private bool moving = false;

	void Start() {
		setScreen (1);
	}

	void Update() {
		float v = horizontalScrollbar.value;

		if (v > 0 && v < 0.25 && activeScreen != Screen.Story && !moving) 
			setScreen (0);
		else if (v > 0.25 && v < 0.75 && activeScreen != Screen.Statistics && !moving) 
			setScreen (1);
		else if (v > 0.75 && v < 1 && activeScreen != Screen.Objectives && !moving) 
			setScreen (2);
		

		if (dragging)
			return;

		if (activeScreen == Screen.Story) {
			if (v > 0)
				v -= 0.025f;
			else {
				v = 0;
				moving = false;
			}
		} else if (activeScreen == Screen.Statistics) {
			if (v < 0.48)
				v += 0.025f;
			else if (v > 0.52)
				v -= 0.025f;
			else {
				v = 0.5f;
				moving = false;
			}
		} else if (activeScreen == Screen.Objectives) {
			if (v < 1)
				v += 0.025f;
			else {
				v = 1;
				moving = false;
			}
		}
		horizontalScrollbar.value = v;
	}

	public void setScreen(int s) {
		if (!dragging)
			moving = true;
		if (s == 0)
			activeScreen = Screen.Story;
		else if (s == 1)
			activeScreen = Screen.Statistics;
		else if (s == 2)
			activeScreen = Screen.Objectives;

		for (int i = 0; i < 3; i++)
			menuBar.transform.GetChild (i).gameObject.GetComponent<TabBehaviour> ().unselect ();

		menuBar.transform.GetChild (s).gameObject.GetComponent<TabBehaviour> ().select ();
	}

	public void startDrag() {
		dragging = true;
	}

	public void endDrag() {
		dragging = false;
	}

	public enum Screen {
		Story,
		Statistics,
		Objectives
	}
}
