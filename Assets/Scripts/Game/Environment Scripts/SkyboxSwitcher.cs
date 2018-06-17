using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxSwitcher : MonoBehaviour {
    public Material normalSky;
    public Material fogSky;


    // Use this for initialization
    void Start () {
        RenderSettings.skybox = normalSky;
    }
	
	// Update is called once per frame
	void Update () {
		if(RenderSettings.fog == true)
        {
            RenderSettings.skybox = fogSky;
        }
        else
        {
            RenderSettings.skybox = normalSky;
        }
	}
}
