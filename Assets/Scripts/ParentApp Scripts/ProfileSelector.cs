using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ProfileSelector : MonoBehaviour {
	public GameObject playerinfo;
	public GameObject content;
	public GameObject profileTemplate;

	public UnityEvent onSelectNewProfile;

	public List<Profile> profiles;
    public float profileWidth;
    public int selected;
    private bool dragging;
    private bool moving;
    private bool update;

    string URL = "http://143.176.117.92/roots-of-life/userSelect.php";
    public string[] usersData;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(loadUsers());
    }

    IEnumerator loadUsers()
    {
        WWW users = new WWW(URL);
        yield return users;
        string usersDataString = users.text.TrimEnd(';');
        usersData = usersDataString.Split(';');
        
        profiles = new List<Profile>();
        foreach (string data in usersData)
        {
            string name = data.Split(',')[0];
			int progress = int.Parse (data.Split(',') [2]);
			float completion = float.Parse (data.Split(',') [3]);
			int playtime = int.Parse (data.Split(',') [4]);
			int openings = int.Parse (data.Split (',') [5]);
			int deathcount = int.Parse (data.Split (',') [6]);
            Image image = Instantiate(profileTemplate).GetComponent<Image>();
            image.transform.SetParent(content.transform, false);
			Profile p = new Profile(name, image, progress, completion, playtime, openings, deathcount);
            if (data.Split(',')[1] == "1")
                p.active = true;
            profiles.Add(p);
        }
        profileWidth = 1f / profiles.Count;
        selected = 0;
        displayUsers();
    }

    void displayUsers()
    {
        float contentwidth = 1080 + (660 * (profiles.Count - 1));
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(contentwidth, 660);
        for (int i = 0; i < profiles.Count; i++)
        {
            Profile p = profiles[i];
            p.image.rectTransform.anchoredPosition = new Vector2(660 * i - ((contentwidth - 1080) * 0.5f), 0);
        }

        setProfile(profiles.Count / 2);
    }

    // Update is called once per frame
    void Update() {
        if (dragging)
            return;

        // Check if adjusting of view is necessary and animate the proper action
        if (moving)
        {
            animate();
            return;
        }

        if (update)
            updateAfterDrag();
        update = false;
    }

	// Move the scrollbar
    private void animate()
    {
        float v = GetComponent<ScrollRectEx>().horizontalScrollbar.value;
        float t = getTarget(selected);
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

    void updateAfterDrag()
    {
        float v = GetComponent<ScrollRectEx>().horizontalScrollbar.value;
        float w = 0;
        int selection = 0;

        // find what profile the scrollbar value has in view
        while (!(v >= w && v <= (w + profileWidth)))
        {
            w += profileWidth;
            selection++;
        }

        // update selected profile if view was open on a different profile than the currently selected profile
        if (selection != selected)
            setProfile(selection);

        // move view to make sure current profile is shown in the exact center
        if (v != getTarget(selected))
            moving = true;
    }

	// Changes the value of selected to the index of the new selected user
	// gets called every time the slider is changed, only call it when the user is actually updated
    public void setProfile(int profile)
    {
        selected = profile;
		onSelectNewProfile.Invoke ();
    }

	// Return the profile instance of the selected user
	public Profile getSelected() 
	{
		return profiles [selected];
	}

	// Find the location on the scrollbar to display the given user
    private float getTarget(int account)
    {
		if (account > profiles.Count)
            return -1;
		return account * (1f / (profiles.Count - 1));
    }

    public void startDrag()
    {
        dragging = true;
    }

    public void endDrag()
    {
        dragging = false;
        update = true;
    }
}
