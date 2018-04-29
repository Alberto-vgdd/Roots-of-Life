using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBehaviour : ScrollRectEx {
	public GameObject menuBar;
	private Screen activeScreen;
	private bool dragging = false;
	private bool moving = false;
	private float scrollSpeed = 0.05f;

	void Start() {
		setActiveScreen (1);
	}

	float defaultPosition(Screen screen) {
		if (screen == Screen.Story)
			return 0.0f;
		if (screen == Screen.Statistics)
			return 0.5f;
		if (screen == Screen.Objectives)
			return 1.0f;
		return -1f;
	}

	int nextScreen(Screen screen) {
		if (screen == Screen.Story)
			return 1;
		if (screen == Screen.Statistics)
			return 2;
		if (screen == Screen.Objectives)
			return 2;
		return -1;
	}

	int prevScreen(Screen screen) {
		if (screen == Screen.Story)
			return 0;
		if (screen == Screen.Statistics)
			return 0;
		if (screen == Screen.Objectives)
			return 1;
		return -1;
	}

	void Update() {
		float v = horizontalScrollbar.value;

		if (v < 0.25)
			setActiveMenubutton (0);
		else if (v > 0.25 && v < 0.75)
			setActiveMenubutton (1);
		else if (v > 0.75)
			setActiveMenubutton (2);

		if (dragging)
			return;

		if (!moving) {
			float scrollDelta = v - defaultPosition (activeScreen);
			if (scrollDelta > 0.1) {
				setActiveScreen (nextScreen (activeScreen));
				if (scrollDelta > 0.75)
					setActiveScreen (nextScreen (activeScreen));
			} else if (scrollDelta < -0.1) {
				setActiveScreen (prevScreen (activeScreen));
				if (scrollDelta < -0.75)
					setActiveScreen (prevScreen (activeScreen));
			}
		}

		/*if ((v - defaultPosition(activeScreen))
		if (v > 0 && v < 0.25 && activeScreen != Screen.Story && !moving) 
			setScreen (0);
		else if (v > 0.25 && v < 0.75 && activeScreen != Screen.Statistics && !moving) 
			setScreen (1);
		else if (v > 0.75 && v < 1 && activeScreen != Screen.Objectives && !moving) 
			setScreen (2);*/
		
		if (activeScreen == Screen.Story) {
			if (v > 0)
				v -= scrollSpeed;
			else {
				v = 0;
				moving = false;
			}
		} else if (activeScreen == Screen.Statistics) {
			if (v < 0.48)
				v += 0.025f;
			else if (v > 0.52)
				v -= scrollSpeed;
			else {
				v = 0.5f;
				moving = false;
			}
		} else if (activeScreen == Screen.Objectives) {
			if (v < 1)
				v += scrollSpeed;
			else {
				v = 1;
				moving = false;
			}
		}
		horizontalScrollbar.value = v;
	}

	public void setActiveScreen(int s) {
		moving = true;
		if (s == 0) 
			activeScreen = Screen.Story;
		else if (s == 1) 
			activeScreen = Screen.Statistics;
		else if (s == 2) 
			activeScreen = Screen.Objectives;
		setActiveMenubutton (s);
	}

	public void setActiveMenubutton(int s) {
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
