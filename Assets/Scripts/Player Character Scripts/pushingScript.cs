using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushingScript : MonoBehaviour {
    public float targetPush;
    public float pushRadius;
    public float pushSpeed;
    Rigidbody rb;
    public Transform player;
    public Transform direction;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        targetPush = Vector3.Distance(player.position, transform.position);
        if (targetPush < pushRadius && Input.GetKey(KeyCode.F))
        {
            Push();
        }
    }
    void Push()
    {
        rb.AddForce(direction.forward * pushSpeed);
    }
    void OnCollisionEnter(Collision c)
    {
        // force is how forcefully we will push the player away from the enemy.
        float force = 5f;

        // If the object we hit is the enemy
        if (c.gameObject.tag == "enemy" && Input.GetKey(KeyCode.F)) 
        {
            // Calculate Angle Between the collision point and the player
            Vector3 dir = c.contacts[0].point - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = -dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player
            GetComponent<Rigidbody>().AddForce(dir * force);
        }
    }
}
