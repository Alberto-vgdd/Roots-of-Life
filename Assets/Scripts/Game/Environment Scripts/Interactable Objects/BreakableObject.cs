using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour, IInteractable
{
	private GameObject parentGameObject; 

	void Start()
	{
		parentGameObject = transform.parent.gameObject;
	}


	public void OnPush()
	{
		GlobalData.GameManager.ShakeCamera(0.25f,0.4f);
		parentGameObject.SetActive(false);
	}

	public void OnPull()
	{

	}
}
