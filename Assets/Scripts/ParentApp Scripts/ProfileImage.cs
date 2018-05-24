using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileImage : MonoBehaviour {
    public Scrollbar scrollbar;
    public GameObject content;
    private float offset;
    private float contentsize;
    private float s;
    private bool initialised = false;

	// Use this for initialization
	void Start () {
        //initialise();
	}

    public void initialise()
    {
        RectTransform rT = content.GetComponent<RectTransform>();
        contentsize = rT.sizeDelta.x / 2;
        offset = GetComponent<RectTransform>().anchoredPosition.x - (rT.anchoredPosition.x - contentsize);
        s = GetComponent<RectTransform>().sizeDelta.x;
        Debug.Log(s);
        initialised = true;
        Debug.Log(initialised);
    }

    // Update is called once per frame
    void Update ()
    {
        RectTransform rT = GetComponent<RectTransform>();
        float f = (scrollbar.value - 0.5f) * 2 - ((offset / contentsize) - 1);
        if (f < 0)
            f = f * -1;
        rT.sizeDelta = new Vector2(s - (150 * f), s - (150 * f));
	}

}
