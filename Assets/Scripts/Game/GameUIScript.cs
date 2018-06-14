using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIScript : MonoBehaviour
{
	private Animator animator;
	public Text acornCounter;
	public GameObject message1;

	private const string gameFadeOut = "GameFadeOut";
	private const string gameFadeIn = "GameFadeIn";
	
	// Use this for initialization
	void Awake ()
	{
		GlobalData.GameUIScript = this;
		animator = GetComponent<Animator>();
	}

	void Start()
	{	
		UpdateAcornCounter();
	}
	

	public void StartGameFadeOut()
	{
		animator.SetTrigger(gameFadeOut);
	}

	public void StartGameFadeIn()
	{
		animator.SetTrigger(gameFadeIn);
	}

	public void UpdateAcornCounter()
	{
		acornCounter.text = "" + GlobalData.AcornCount;
	}

}
