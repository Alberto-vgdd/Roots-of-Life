using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Rendering;

public class CaterpillerAI : MonoBehaviour
{

    //variables for enemy 
    public Transform player;
    public Transform head;

    string state = "moving";
    public GameObject[] waypoints;
    int currentWP = 0;
    public float rotSpeed;
    public float speed;
    public float accuracy;
    public float damping;

    Renderer render;
    Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = player.position - this.transform.position;
        direction.y = 0;
        float angle = Vector3.Angle(direction, head.up);

        if (state == "moving" && waypoints.Length > 0)
        {
            if (Vector3.Distance(waypoints[currentWP].transform.position, transform.position) < accuracy)
            {

                currentWP = Random.Range(0, waypoints.Length);
                /*currentWP++;
                if (currentWP >= waypoints.Length)
                {
                    currentWP = 0;
                }*/
            }
            direction = waypoints[currentWP].transform.position - transform.position;
            this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
            this.transform.Translate(0, 0, Time.deltaTime * speed);
        }

        if (Vector3.Distance(player.position, this.transform.position) < 4 || state == "attacking")
        {
            state = "attacking";
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);

            if (direction.magnitude < 5)
            {
                this.transform.Translate(0, 0, Time.deltaTime * speed);
            }

            else
            {
                state = "moving";
            }
        }
        //we need to access when the playerr dies so that the caterpillar knows when to stop attacking
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            state = "moving";
            //rb.detectCollisions = false;
            rb.isKinematic = true;
            rb.WakeUp();
            
        }
        
    }
}

