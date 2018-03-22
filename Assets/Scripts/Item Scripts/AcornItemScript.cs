using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornItemScript : MonoBehaviour
{
	private string playerTag;
	private string collect = "collect";

	public GameObject acornMesh;
	public Animator acornAnimator;

	private bool acornCollected;

	void Awake()
	{
		acornMesh.SetActive(false);
	}

	void OnEnable()
	{
		Invoke("EnableAcorn", 1f + Random.value);
	}

	void Start () 
	{
		playerTag = GlobalData.PlayerTag;
	}
	
	void EnableAcorn()
	{
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




}
