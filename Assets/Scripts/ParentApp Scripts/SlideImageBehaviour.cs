using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideImageBehaviour : MonoBehaviour {
	public Scrollbar scrollbar;
	public Image image;
	public Text text;
	private bool loopup = false;
	private float alpha = 0.9f;

	// Update is called once per frame
	void Update () {
		if (loopup) {
			alpha += 0.0015f;
			if (alpha >= 0.8)
				loopup = false;
		} else {
			alpha -= 0.0015f;
			if (alpha <= 0.5)
				loopup = true;
		}
		float f = (float) (scrollbar.value - 0.5) * 2;
		if (f < alpha)
			alpha = f;
		if (alpha < 0)
			alpha = 0;
		image.CrossFadeAlpha (alpha, 0.0f, true);
		text.CrossFadeAlpha (alpha, 0.0f, true);
	}
}
