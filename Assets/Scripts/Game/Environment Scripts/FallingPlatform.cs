using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public GameObject artAsset;
    public float fallSpeed = 2.5f;
    public float fallTime = 2;
    private float fallTimer = -1;
    public float waitTime = 1;
    private float waitTimer = -1f;


    private bool isUsed = false;
    private bool isFalling = false;
    private string playerTag;

    private Rigidbody platformRigidbody;
    private BoxCollider platformCollider;
    private Vector3 originalPosition;
    private Rigidbody assetRigidbody;

    private int environmentLayerMask;
    private int enemiesLayerMask;

    
    
    void Awake()
    {
        platformRigidbody = GetComponent<Rigidbody>();
        platformCollider = GetComponent<BoxCollider>();
        if (artAsset != null)
            assetRigidbody = artAsset.GetComponent<Rigidbody>();
    }

    void Start()
    {
        playerTag = GlobalData.PlayerTag;

        environmentLayerMask = 1 << (int) Mathf.Log(GlobalData.EnvironmentLayerMask.value,2);
        enemiesLayerMask = 1 << (int) Mathf.Log(GlobalData.EnemiesLayerMask.value,2);

        originalPosition = platformRigidbody.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag) && !isUsed)
        {
            waitTimer = 0f;
            isUsed = true;
        }
        else if (collision.gameObject.layer.Equals(environmentLayerMask | enemiesLayerMask))
        {
            Physics.IgnoreCollision(platformCollider, collision.collider, true);
        }
    }

    void FixedUpdate()
    {
        if (isUsed)
        {
            if (isFalling)
            {
                fallTimer += Time.fixedDeltaTime;
                platformRigidbody.MovePosition( platformRigidbody.position + Vector3.down*fallSpeed*Time.fixedDeltaTime);
                if (assetRigidbody != null)
                    assetRigidbody.MovePosition(platformRigidbody.position + Vector3.down * fallSpeed * Time.fixedDeltaTime);

                if (fallTimer > fallTime)
                {
                    platformRigidbody.MovePosition(originalPosition);
                    if (assetRigidbody != null)
                        assetRigidbody.MovePosition(originalPosition);
                    isUsed = false;
                    isFalling = false;
                    waitTimer = -1f;
                    fallTimer = -1f;
 
                }
            }
            else
            {
                waitTimer += Time.fixedDeltaTime;

                if (waitTimer > waitTime)
                {
                    isFalling = true;
                    fallTimer = 0f;
                }
            }
            
        }


    }


}
