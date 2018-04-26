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
	public static float openSpeed = 20f;
	[Range(0.0f,1.0f)]
	public float size = 0.0f;
	private float opensize;
	private float maskIncrement;
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

		float r = opensize / openSpeed;
		int n = (int) Mathf.Round (r);
		if (r > 0.5)
			n++;
		maskIncrement = 1f / n;
	}
	
	// Update is called once per frame
	void Update () {
		RectTransform rT = GetComponent<RectTransform> ();
		if (state == State.opening) {
			rT.sizeDelta = new Vector2(rT.sizeDelta.x,rT.sizeDelta.y + openSpeed);

			Vector2 position = rT.anchoredPosition;
			position.y -= (openSpeed/2);
			rT.anchoredPosition = position;

			if (nextChapter != null)
				nextChapter.move (false);

			chapterMask.fillAmount += maskIncrement;

			if (rT.sizeDelta.y >= opensize) {
				rT.sizeDelta = new Vector2 (rT.sizeDelta.x, opensize);
				state = State.open;
			}
		}
		if (state == State.closing) {
			rT.sizeDelta = new Vector2(rT.sizeDelta.x,rT.sizeDelta.y - openSpeed);

			Vector2 position = rT.anchoredPosition;
			position.y += (openSpeed/2);
			rT.anchoredPosition = position;

			if (nextChapter != null)
				nextChapter.move (true);

			chapterMask.fillAmount -= maskIncrement;
			
			if (rT.sizeDelta.y <= 200) {
				rT.sizeDelta = new Vector2 (rT.sizeDelta.x, 200);
				state = State.closed;
			}
		}
	}

	private void move(bool up) {
		RectTransform rT = GetComponent<RectTransform> ();
		Vector2 position = rT.anchoredPosition;
		if (up) 
			position.y += openSpeed;
		else 
			position.y -= openSpeed;
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
