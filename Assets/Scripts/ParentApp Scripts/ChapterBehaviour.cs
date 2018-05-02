﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChapterBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
	public State state = State.closed;
	public Image chapterMask;
	public ChapterBehaviour nextChapter;
	public ScrollRect parent;
	public static float openSpeed = 100f;
    private static float actualOpenSpeed = 100f;
	private float opensize;
	private float maskIncrement;
	private float startY;
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
		opensize = rT.sizeDelta.y + chapterMask.GetComponent<RectTransform> ().sizeDelta.y - 200;

		
		startY = rT.anchoredPosition.y;
	}
	
	// Update is called once per frame
	void Update () {

        actualOpenSpeed = openSpeed * (0.005f / Time.deltaTime);

        float r = opensize / actualOpenSpeed;
        int n = (int)Mathf.Round(r);
        if (r > 0.5)
            n++;
        maskIncrement = 1f / n;

        RectTransform rT = GetComponent<RectTransform> ();
		if (state == State.opening) {
			rT.sizeDelta = new Vector2(rT.sizeDelta.x,rT.sizeDelta.y + actualOpenSpeed);

			Vector2 position = rT.anchoredPosition;
			position.y -= (actualOpenSpeed / 2);
			rT.anchoredPosition = position;

			if (nextChapter != null)
				nextChapter.move (false);

			chapterMask.fillAmount += maskIncrement;

			if (rT.sizeDelta.y >= opensize) {
				float difference = rT.sizeDelta.y - opensize;
				Debug.Log (difference);
				position = rT.anchoredPosition;
				position.y += (difference/2);
				rT.anchoredPosition = position;
				rT.sizeDelta = new Vector2 (rT.sizeDelta.x, opensize);
				state = State.open;
			}
		}
		if (state == State.closing) {
			rT.sizeDelta = new Vector2(rT.sizeDelta.x,rT.sizeDelta.y - actualOpenSpeed);

			Vector2 position = rT.anchoredPosition;
			position.y += (actualOpenSpeed / 2);
			rT.anchoredPosition = position;

			if (nextChapter != null)
				nextChapter.move (true);

			chapterMask.fillAmount -= maskIncrement;
			
			if (rT.sizeDelta.y <= 200) {
				rT.sizeDelta = new Vector2 (rT.sizeDelta.x, 200);
				state = State.closed;
				position = rT.anchoredPosition;
				position.y = startY;
				rT.anchoredPosition = position;
			}
		}
	}

	private void move(bool up) {
		RectTransform rT = GetComponent<RectTransform> ();
		Vector2 position = rT.anchoredPosition;
		if (up) 
			position.y += actualOpenSpeed;
		else 
			position.y -= actualOpenSpeed;
		rT.anchoredPosition = position;
		if (nextChapter != null)
			nextChapter.move (up);
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
