using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapBehaviour : MonoBehaviour {
	public Scrollbar scrollbar;
	public float snapValue = 0.65f;
	private float oldValue = 1.0f;
	
	// Update is called once per frame
	void Update () {
		float scrollDelta = oldValue - scrollbar.value;
		oldValue = scrollbar.value;
		if (scrollDelta > 0.001)
			return;
		
		if (scrollbar.value > snapValue)
			scrollbar.value += 0.01f;
		else
			scrollbar.value -= 0.01f;
	}
}
