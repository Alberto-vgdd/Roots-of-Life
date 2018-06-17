using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FogTextScript : MonoBehaviour {

    public Text text1;
    public GameObject fogText;
	// Use this for initialization
	void Start () {
        text1 = GetComponent<Text>();
        
        //text1.enabled = false;  
	}
     void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //text1.enabled = true;
            fogText.gameObject.SetActive(true);
            Debug.Log("whyaintuworkin");
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
