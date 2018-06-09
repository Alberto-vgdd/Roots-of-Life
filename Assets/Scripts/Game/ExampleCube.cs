using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleCube : MonoBehaviour {
    private  FlagListener listener;
	// Use this for initialization

	void Start () {
        listener = GameObject.Find("[X] Flag Listener").GetComponent<FlagListener>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onEvent()
    {
        int value = listener.getValue("objectVanish");
        if (value == 0)
            gameObject.SetActive(false);
        if (value == 1)
            gameObject.SetActive(true);
    }
}
