using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchBehaviour : MonoBehaviour {

	public void beginDrag(string input) {
		Debug.Log ("beginDrag:"+input);
	}

	public void endDrag(string input) {
		Debug.Log ("endDrag:"+input);
	}

	public void pointerDown(string input) {
		Debug.Log ("pointerDown:"+input);
	}

	public void pointerUp(string input) {
		Debug.Log ("pointerUp:"+input);
	}

	public void pointerClick(string input) {
		Debug.Log ("pointerClick:"+input);
	}

	public void drag(string input) {
		Debug.Log ("drag:"+input);
	}
}
