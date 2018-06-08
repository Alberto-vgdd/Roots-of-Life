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

	private IEnumerator movingCoroutine;
	private float collectTime = 0.5f;
	private bool acornCollected;

	private float pushTime = 0.5f;
	private float pushSpeed = 10f;


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
		if (movingCoroutine == null)
		{
			movingCoroutine = MoveToPlayer();
			StartCoroutine(movingCoroutine);
		}
	}

	IEnumerator MoveToPlayer()
	{
		Vector3 originalPosition = transform.position;
		Vector3 playerOriginalPosition =  GlobalData.PlayerTransform.position;

		float timer = 0;

		while ( timer <= collectTime )
		{
			timer += Time.deltaTime;
			transform.position = Vector3.Lerp(originalPosition,playerOriginalPosition,timer/collectTime);
			yield return new WaitForEndOfFrame();
		}

		StopCoroutine(movingCoroutine);
		movingCoroutine = null;
	}


	public void OnPush()
	{
		if (movingCoroutine == null)
		{
			movingCoroutine = MoveAway();
			StartCoroutine(movingCoroutine);
		}
	}

	IEnumerator MoveAway()
	{
		Vector3 direction =  (transform.position - GlobalData.PlayerTransform.position).normalized;

		float timer = 0;

		while ( timer <= pushTime )
		{
			timer += Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position,transform.position+direction,pushSpeed*Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}

		StopCoroutine(movingCoroutine);
		movingCoroutine = null;

	}


}
