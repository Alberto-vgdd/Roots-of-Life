using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOff : MonoBehaviour {


	// Use this for initialization
	void Start () {
        
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RenderSettings.fog = false;
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
