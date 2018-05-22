using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileSelector : MonoBehaviour {
    public Image background;
	public GameObject playerinfo;
	public GameObject content;
	public GameObject profileTemplate;
	public static GameObject staticContent;
	public static GameObject staticProfileTemplate;
    private float bY;
    private float bX;
    private int s;
    private bool dragging;
    private bool moving;
	private List<Profile> profiles;
	private Profile selected;

	public class Profile : MonoBehaviour {
		public string name;
		public bool active;
		public Image image;
		public Profile(string name) {
			this.name = name;
			image = Instantiate(staticProfileTemplate).GetComponent<Image>();
			image.transform.SetParent(staticContent.transform);
			image.name = name;
			image.rectTransform.anchoredPosition = new Vector2(0, 0);
			image.rectTransform.sizeDelta = new Vector2(256, 256);
		}
	}

    // Use this for initialization
    void Start()
    {
		staticContent = content;
		staticProfileTemplate = profileTemplate;
        bY = background.rectTransform.anchoredPosition.y;
        bX = background.rectTransform.anchoredPosition.x;

		profiles = new List<Profile> ();
		profiles.Add (new Profile ("Player 1"));
		profiles.Add (new Profile ("Player 2"));
		profiles.Add (new Profile ("Player 3"));
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

		float profileWidth = (float)1 / profiles.Count;
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
		selected = profiles [profile];
		playerinfo.transform.GetChild (0).GetComponent<Text> ().text = selected.name;
    }

    private float getTarget(int account)
    {
		if (account > profiles.Count)
            return -1;
		return account * (1f / (profiles.Count - 1));
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
