using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill : MonoBehaviour {
    public GameObject caterpillar;

     void Start()
    {
        //kill = GetComponent<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(caterpillar);
            Debug.Log("dead");
        }
    }
}
