using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleBehaviour : MonoBehaviour {
	public Scrollbar bar;

	public void Update() {
		float newAlpha = bar.value * -1 + 1;
		Debug.Log("new alpha = " + newAlpha);
		GetComponent<CanvasGroup> ().alpha = newAlpha;
	}
}
