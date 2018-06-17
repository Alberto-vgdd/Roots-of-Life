using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour, IInteractable
{
	private GameObject parentGameObject;
    public GameObject artAsset;

	void Start()
	{
		parentGameObject = transform.parent.gameObject;
        if (artAsset != null)
            artAsset.transform.SetParent(parentGameObject.transform);
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
