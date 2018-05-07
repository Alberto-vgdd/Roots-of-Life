using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileImage : MonoBehaviour {
    public Scrollbar scrollbar;
    public GameObject content;
    private float offset;
    private float s;

	// Use this for initialization
	void Start () {
        RectTransform rT = content.GetComponent<RectTransform>();
        offset = GetComponent<RectTransform>().anchoredPosition.x - (rT.anchoredPosition.x - (rT.sizeDelta.x / 2));
        Debug.Log(gameObject.name + offset);
        s = GetComponent<RectTransform>().sizeDelta.x;
	}
	
	// Update is called once per frame
	void Update () {
        RectTransform rT = GetComponent<RectTransform>();
        float f = (scrollbar.value - 0.5f) * 2 - ((offset / 531.9f) - 1);
        if (f < 0)
            f = f * -1;
        rT.sizeDelta = new Vector2(s - (150 * f), s - (150 * f));
	}
}
