using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsTriggers : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Main area trigger 1"))
        {
            Debug.Log(Analytics.CustomEvent("time between start and first trigger", new Dictionary<string, object>
            {
                {"time elapsed:", GlobalData.GameManager.timeSinceStart}
            }));
        }
        if (other.CompareTag("Main area second trigger"))
        {
            Analytics.CustomEvent("time between first and second trigger", new Dictionary<string, object>
            {
                {"time elapsed:", GlobalData.GameManager.timeSinceStart}
            });
        }
    }
}
