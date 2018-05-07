using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChapterBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
	public State state = State.closed;
	public Image chapterMask;
	public ChapterBehaviour nextChapter;
	public ScrollRect parent;

	public static float openTime = 0.3f;
	public float t = 0.0f;
	private float opensize;

	private static List<ChapterBehaviour> chapters = new List<ChapterBehaviour>();
	private bool drag;

	public enum State {
		opening,
		open,
		closing,
		closed
	}

	// Use this for initialization
	void Start () {
		chapters.Add (this);
		RectTransform rT = GetComponent<RectTransform> ();
        float y = chapterMask.GetComponent<RectTransform>().sizeDelta.y;
		opensize = y + 200;
	}
	
	// Update is called once per frame
	void Update () {
		if (state == State.opening || state == State.closing)
			animate ();
	}

	private void animate() {

		// Calculate p for t as percentage of openTime, if t exceeds opentime then p will be 1.0 and the state will be switched
		float p = getPercentage();
		RectTransform rT = GetComponent<RectTransform> ();

		// Adjust size of textfield according to p
		float d = Mathf.Lerp(200, opensize, p) - rT.sizeDelta.y;
		rT.sizeDelta = new Vector2(rT.sizeDelta.x, Mathf.Lerp(200, opensize, p));

		// Adjust y of textfield according to p
		Vector2 position = rT.anchoredPosition;
		position.y -= d * 0.5f;
		rT.anchoredPosition = position;

		// Adjust mask of textfield according to p
		chapterMask.fillAmount = p;
		Debug.Log (p);

		// Move lower chapters based on size increment
		if (nextChapter != null)
			nextChapter.move (d);
	}

	private float getPercentage()
    {
        if (state == State.opening)
            t += Time.deltaTime;
        else if (state == State.closing)
            t -= Time.deltaTime;

        if (t >= openTime)
        {
            t = openTime;
            state = State.open;
        }
        else if (t <= 0.0f)
        {
            t = 0.0f;
            state = State.closed;
        }
        return t / openTime;
	}

	private void move (float difference) {
		RectTransform rT = GetComponent<RectTransform> ();
		Vector2 position = rT.anchoredPosition;
        position.y -= difference;
		rT.anchoredPosition = position;
		if (nextChapter != null)
			nextChapter.move (difference);
	}

	public void onClick() {
		if (drag) {
			drag = false;
			return;
		}
		if (state == State.closed || state == State.closing) {
			foreach (var c in chapters) {
				c.close ();
			}
			state = State.opening;
		} else
			close ();
	}

	private void close() {
		if (state == State.open || state == State.opening) 
			state = State.closing;
	}

	public void OnPointerDown(PointerEventData eventData) {
		parent.OnBeginDrag (eventData);
	}

	public void OnPointerUp(PointerEventData eventData) {
		parent.OnEndDrag(eventData);
	}

	public void OnDrag(PointerEventData eventData) {
		parent.OnDrag (eventData);
		drag = true;
	}
}
