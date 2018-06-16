using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour, IInteractable
{

	public void OnPush()
	{
		Debug.Log("Hit!");
	}

	public void OnPull()
	{

	}
}
