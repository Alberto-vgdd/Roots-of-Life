using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileSelector : MonoBehaviour {
    public Image background;
    private float bY;
    private float bX;
    private int accounts = 3;
    private int s;
    private bool dragging;
    private bool moving;

    // Use this for initialization
    void Start()
    {
        bY = background.rectTransform.anchoredPosition.y;
        bX = background.rectTransform.anchoredPosition.x;
    }

    // Update is called once per frame
    void Update() {
        if (dragging)
            return;

        if (moving)
        {
            animate();
            return;
        }

        float profileWidth = (float)1 / accounts;
        float v = GetComponent<ScrollRectEx>().horizontalScrollbar.value;
        float n = 0;
        int selection = 0;
        while (!(v >= n && v <= (n + profileWidth)))
        {
            n += profileWidth;
            selection++;
        }
        if (selection != s)
            setProfile(selection);
        if (v != getTarget(s))
            moving = true;
	}

    public void setProfile(int profile)
    {
        s = profile;
    }

    private float getTarget(int account)
    {
        if (account > accounts)
            return -1;
        return account * (1f / (accounts - 1));
    }

    private void animate()
    {
        float v = GetComponent<ScrollRectEx>().horizontalScrollbar.value;
        float t = getTarget(s);
        if (v > t)
        {
            float next = v - (Time.deltaTime * 5);
            if (next < t)
                v = t;
            else
                v = next;
        }
        else if (v < t)
        {
            float next = v + (Time.deltaTime * 5);
            if (next > t)
                v = t;
            else
                v = next;
        }
        if (v == t)
            moving = false;
        GetComponent<ScrollRectEx>().horizontalScrollbar.value = v;
    }

    public void adjustBackground()
    {
        float v = GetComponent<ScrollRectEx>().horizontalScrollbar.value;
        float w = GetComponent<ScrollRectEx>().content.sizeDelta.x;
        background.rectTransform.anchoredPosition = new Vector2(bX + (w * (v - 0.5f)), bY);
    }

    public void startDrag()
    {
        dragging = true;
    }

    public void endDrag()
    {
        dragging = false;
    }
}
