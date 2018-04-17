using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogScript : MonoBehaviour {
    public float min;
    public float max ;
    public float t;
    // Use this for initialization
    void Start () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            RenderSettings.fog = true;
            //RenderSettings.fogDensity = Mathf.Lerp(min, max, t);
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
