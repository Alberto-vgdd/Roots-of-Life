using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using UnityEditor.Rendering;
using UnityEngine.Analytics;

public class CaterpillerAI : MonoBehaviour, IEnemy
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

    public NavMeshAgent caterpillar;

   // Renderer render;
    Rigidbody rb;

    private string playerTag;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;

        playerTag = GlobalData.PlayerTag;
        caterpillar = GetComponent<NavMeshAgent>();
        caterpillar.autoBraking = false;
        //caterpillar.SetDestination();
        //state == "moving";
    }

    // Update is called once per frame
    void Update()
    {
       // caterpillar.SetDestination(this.transform.position);

        Vector3 direction = player.position - this.transform.position;
        direction.y = 0;
        float angle = Vector3.Angle(direction, head.up);

        if (state == "moving" && waypoints.Length >= 0)
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
            caterpillar.SetDestination(direction);
           // this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
           // this.transform.Translate(0, 0, Time.deltaTime * speed);
        }

        if (Vector3.Distance(player.position, this.transform.position) < 4 || state == "attacking")
        {
            state = "attacking";
            //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
            caterpillar.SetDestination(player.position);
            if (direction.magnitude < 5)
            {
               // this.transform.Translate(0, 0, Time.deltaTime * speed);
            }

            else
            {
                state = "moving";
            }
        }
        //we need to access when the playerr dies so that the caterpillar knows when to stop attacking
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            state = "moving";
            //rb.detectCollisions = false;
            rb.isKinematic = true;
            rb.WakeUp();

            Physics.IgnoreCollision(gameObject.GetComponent<BoxCollider>(),other);
            TakeDamage(0);
            
        }
        
    }

    public void TakeDamage(int damageAmount)
    {
        Destroy(this.gameObject);

        Analytics.CustomEvent("Caterpillar dead");

    }
}

