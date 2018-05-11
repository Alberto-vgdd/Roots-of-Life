using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornItemScript : MonoBehaviour, IInteractable
{
	private string playerTag;
	private string collect = "collect";

	public GameObject acornMesh;
	public Animator acornAnimator;
	private Vector3 acornOriginalPosition;

	private Coroutine collectCoroutine;
	private float collectTime = 1f;
	private bool acornCollected;

	void Awake()
	{
		acornMesh.SetActive(false);
		acornOriginalPosition = transform.position;
	}

	void OnEnable()
	{
		Invoke("EnableAcorn", 1f + Random.value*2f);
	}

	void Start () 
	{
		playerTag = GlobalData.PlayerTag;
	}
	
	void EnableAcorn()
	{
		transform.position = acornOriginalPosition;
		acornCollected = false;
		acornMesh.SetActive(true);
		acornAnimator.enabled = true;
	}

	void OnTriggerEnter(Collider other)
	{
		if (!acornCollected && other.CompareTag(playerTag))
		{
			CollectAcorn();
		}
	}

	void CollectAcorn()
	{
		GlobalData.AcornCount++;
		GlobalData.SoundManagerScript.PlayAcornSound();
		GlobalData.GameUIScript.UpdateAcornCounter();
		acornAnimator.SetTrigger(collect);
		acornCollected = true;

		Invoke("DisableAcorn",1f);
	}

	void DisableAcorn()
	{
		acornAnimator.enabled = false;
		acornMesh.SetActive(false);
		gameObject.SetActive(false);
	}

	public void OnPull()
	{
		if (collectCoroutine == null)
		{
			StartCoroutine(MoveToPlayer());
		}
	}

	IEnumerator MoveToPlayer()
	{
		Transform player = GlobalData.PlayerTransform;
		float timer = 0;

		while ( timer <= collectTime )
		{
			timer += Time.deltaTime;
			transform.position = Vector3.Lerp(transform.position,player.position,timer);
			yield return new WaitForEndOfFrame();
		}

		yield return null;
	}


	public void OnPush()
	{

	}




}
