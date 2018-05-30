using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleCube : MonoBehaviour {
    public FlagListener listener;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onEvent()
    {
        int value = listener.getValue();
        if (value == 0)
            gameObject.SetActive(false);
        if (value == 1)
            gameObject.SetActive(true);
    }
}
