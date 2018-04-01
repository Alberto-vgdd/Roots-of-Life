using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveBehaviour : MonoBehaviour {
	public AppBehaviour other;

	void Awake() {
		other = GameObject.Find ("AppManager").GetComponent<AppBehaviour> ();
	}

	public void clear() {
		Destroy(gameObject);
		other.sortObjectives ();
	}
}
