using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hooverScript : MonoBehaviour
{

    public float targetDistance;
    public float suckingRadius;
    public float suckingSpeed;
    public float damping;
    Rigidbody rigidBody;
    public Transform playerTarget;

    public GameObject Suckable;


    // Use this for initialization
    void Start()
    {
        //rigidBody.isKinematic = true;
        //rigidBody.useGravity = true;
        rigidBody = GetComponent<Rigidbody>();
        Suckable = GetComponent<GameObject>();
    }

    // Checks distance for sucking to be allowed and calls their functions
    void FixedUpdate()
    {
        targetDistance = Vector3.Distance(playerTarget.position, transform.position);
        if (targetDistance < suckingRadius && Input.GetKey(KeyCode.Tab))
        {
            rigidBody.isKinematic = false;
            SuckItem();
            lookAt();
            Debug.Log("YE");
            
            // ShakeItem();
        }
    }

    //keeps the objects to be sucked going towards the player
    void lookAt()
    {
        Quaternion rotation = Quaternion.LookRotation(playerTarget.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    //Sucks it -_-
    void SuckItem()
    {
        rigidBody.AddForce(transform.forward * suckingSpeed);
        // transform.Translate(0, 0,suckingSpeed * Time.deltaTime);
    }

    //Not working rn
    void OnCollisionEnter(Collision collision)
    {

        //if (collision.gameObject.CompareTag(""))
        if (collision.gameObject.CompareTag("Player"))
        {
            
            Destroy(Suckable); //destroys 
            

        }
    }
}
