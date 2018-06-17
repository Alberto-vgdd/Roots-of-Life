using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour {

    public GameObject caterpillar;

	// Use this for initialization
	void Start () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            caterpillar.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
