using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFalling : MonoBehaviour
{

    public float fallSpeed = 2.5f;
    public float resetTime = 2;
    private float resetTimer = -1;

    private bool isFalling = false;
    private string playerTag;

    private Rigidbody platformRigidbody;
    private BoxCollider platformCollider;
    private Vector3 originalPosition;

    private int environmentLayerMask;
    private int enemiesLayerMask;

    
    
    void Awake()
    {
        platformRigidbody = GetComponent<Rigidbody>();
        platformCollider = GetComponent<BoxCollider>();
        originalPosition = platformRigidbody.position;
    }
    void Start()
    {
        playerTag = GlobalData.PlayerTag;

        environmentLayerMask = 1 << (int) Mathf.Log(GlobalData.EnvironmentLayerMask.value,2);
        enemiesLayerMask = 1 << (int) Mathf.Log(GlobalData.EnemiesLayerMask.value,2);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag) && resetTimer < 0)
        {
            resetTimer = 0f;
            isFalling = true;
        }
        else if (collision.gameObject.layer.Equals(environmentLayerMask | enemiesLayerMask))
        {
            Physics.IgnoreCollision(platformCollider, collision.collider, true);
        }
    }

    void FixedUpdate()
    {
        if (isFalling)
        {
            resetTimer += Time.fixedDeltaTime;
            platformRigidbody.MovePosition( new Vector3(platformRigidbody.position.x, platformRigidbody.position.y - fallSpeed*Time.fixedDeltaTime, platformRigidbody.position.z));

            if (resetTimer > resetTime)
            {
                isFalling = false;
                resetTimer = -1f;
                platformRigidbody.position = originalPosition;
            }
        }
    }
}
