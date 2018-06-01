using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarBehaviour : MonoBehaviour {
	public Image progressImage;
	public Text percentageText;
	public float percentage = 1.0f;
	private bool d = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		progressImage.fillAmount = percentage;
		percentageText.text = Mathf.Round(percentage * 100) + "%";

		calc ();
	}

	private void calc() {
		if (d)
			percentage -= Time.deltaTime;
		else
			percentage += Time.deltaTime * 0.1f;
		if (percentage >= 1.0f)
			d = true;
		if (percentage <= 0.0f)
			d = false;
	}
}
