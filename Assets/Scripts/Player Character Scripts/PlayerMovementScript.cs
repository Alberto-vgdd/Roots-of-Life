﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
  
    [Header("General Movement Parameters")]
    [Tooltip("Maximum speed the character can achieve walking.")]
    public float baseMovementSpeed = 4f;
    [Tooltip("The boost in movement speed the character will get while running.")]
    public float runSpeedMultiplier = 1.5f;
    [Tooltip("The speed measured in Degrees/seconds.")]
    public float turnSpeed = 720f;
    [Tooltip("Multiplier value used to increase or decrease the gravity effect.")]
    public float gravityScale = 2f;
    [Tooltip("Vertical speed asigned to the player when jumping.")]
    public float jumpSpeed = 7.5f;
    [Tooltip("Maximum speed the character reach get to while falling.")]
    public float maximumFallingSpeed = 10f;


    [Header("Step Climbing")]
    [Tooltip("Maximum step height the character can climb to. (THIS VALE MUST BE EQUAL TO THE CAPSULE COLLIDER'S RADIUS)")]
    public float stepMaxHeight = 0.25f;
    [Tooltip("Minimum free of collider depth that a step must have on top to be climbable.")]
    public float stepMinDepth = 0.4f;

    [Header("Steep Slope Sliding")]
    [Tooltip("Minimum angle between the slope and the ground to consider it a steep.")]
    public float steepAngle = 50f;
    [Tooltip("Minimum speed the character will get while sliding.")]
    public float steepSlidingSpeed = 8f;
    
    
    // Input variables.
    // Transform to calculate the direction of the movement.
    // Movement Axes for the player. M = V + H
    // Maximum Movement Speed = base Movement Speed (* run Speed Multiplier while running)
    Transform cameraTransform;
    Vector2 movementInput;
    bool jumpInput;
    bool runInput;
    Vector3 movementDirection;
    Vector3 horizontalDirection;
    Vector3 verticalDirection;
    float maximumMovementSpeed;

    // Physics variables (Raycasts, Capsulecasts, etc.)
    int environmentLayerMask;
    int enemiesLayerMask;
    RaycastHit[] capsulecastHitArray;
    Vector3 point1;
    Vector3 point2;
    float radius;
    float radiusScale = 0.95f;

    private bool playerCloseToGround;
    private bool playerJumping;
    private bool playerDoubleJumping;
    private bool playerSliding;
    private bool playerGrounded;

    Vector3 groundNormal;
    float groundAngle;

    
    // Player variables
    private Animator playerAnimator;
    private Rigidbody playerRigidbody;
    private CapsuleCollider playerCapsuleCollider;
    private ParticleSystem landingParticles;


    // Variables to manage push
    private bool playerPushed;
    private float pushTime = 0.6f;
    private float pushTimer;


    
    // Use this for initialization
    void Awake ()
    {
        landingParticles = transform.Find("Particle System").GetComponent<ParticleSystem>();
        playerAnimator = transform.GetComponentInChildren<Animator>();
        playerRigidbody = transform.GetComponent<Rigidbody>();
        playerCapsuleCollider = transform.GetComponent<CapsuleCollider>();
        radius = playerCapsuleCollider.radius;
    }

    void Start()
    {
        cameraTransform = GlobalData.PlayerCamera.transform;
        environmentLayerMask = 1 << (int) Mathf.Log(GlobalData.EnvironmentLayerMask.value,2);
        enemiesLayerMask = 1 << (int) Mathf.Log(GlobalData.EnemiesLayerMask.value,2);
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!GlobalData.PlayerDeath || playerPushed)
        {
            // Update movement input and normalize the vector to avoid diagonal acceleration.
            movementInput = new Vector2(GlobalData.GetHorizontalInput(),GlobalData.GetVerticalInput()) ;

            // m_MovementInput.magnitude?
            if (Mathf.Abs(movementInput.x)+Mathf.Abs(movementInput.y) > 1 )
            {
                movementInput.Normalize();
            }

            // Jump Input
            if (GlobalData.GetJumpButtonDown()) 
            {
                jumpInput = true;
            }

            // Run Input 
            runInput = (GlobalData.GetRunButton() > 0.1f) ? true : false;
            
        }
        else
        {
            movementInput = Vector2.zero;
            jumpInput = false;
            runInput = false;
        }

               
        // Update movement directions 
        if (!GlobalData.IsEnemyLocked)
        {
            horizontalDirection = Vector3.Scale(cameraTransform.right, new Vector3(1f, 0f, 1f)).normalized;
            verticalDirection = Vector3.Scale(cameraTransform.forward, new Vector3(1f, 0f, 1f)).normalized;
        }
        else
        {

            verticalDirection = Vector3.Scale(GlobalData.LockedEnemyTransform.position - transform.position, new Vector3(1f, 0f, 1f)).normalized;
            horizontalDirection = Vector3.Cross(verticalDirection, -transform.up).normalized;
        }

        movementDirection = horizontalDirection * movementInput.x + verticalDirection * movementInput.y;

        

        //Rotate the player (Why is this here? Because unity can't interpolate rotations in no-kinematic objects).
        if (playerRigidbody.velocity.magnitude > 0f)
        {
            Vector3 velocityDirection = Vector3.Scale(playerRigidbody.velocity,new Vector3(1,0,1)).normalized;
            
            if (playerSliding)
            {
                playerRigidbody.MoveRotation( Quaternion.RotateTowards(playerRigidbody.rotation,Quaternion.LookRotation(velocityDirection,Vector3.up),turnSpeed*2*Time.deltaTime) );
            }
            else
            {
                if (!GlobalData.IsEnemyLocked)
                {                    
                    if ( movementInput.magnitude > 0.01f && playerRigidbody.velocity.magnitude > 0.01f)
                    {
                        if( Vector3.Angle(transform.forward,velocityDirection) > 135)
                        {
                            playerRigidbody.MoveRotation( Quaternion.RotateTowards(playerRigidbody.rotation,Quaternion.LookRotation(velocityDirection,Vector3.up),turnSpeed*2*Time.deltaTime ));
                        }
                        else
                        {
                            playerRigidbody.MoveRotation( Quaternion.RotateTowards(playerRigidbody.rotation,Quaternion.LookRotation(velocityDirection,Vector3.up),turnSpeed*Time.deltaTime) );
                        }
                    }
                }
                else
                {
                    playerRigidbody.MoveRotation( Quaternion.RotateTowards(playerRigidbody.rotation,Quaternion.LookRotation(velocityDirection,Vector3.up),turnSpeed*Time.deltaTime) );
                }
            }      
        }


        // Animations
        playerAnimator.SetBool("Fall", !playerCloseToGround);
        playerAnimator.SetBool("Slide", playerSliding);
        playerAnimator.SetFloat("Walk Speed",movementInput.magnitude*maximumMovementSpeed/(baseMovementSpeed*runSpeedMultiplier) ); 



        if (playerPushed)
        {
            pushTimer += Time.deltaTime;
            if (pushTimer >= pushTime)
            {
                playerPushed = false;
            }
        }


    }


    void FixedUpdate()
    {
        // Modify parameters based on character's state ( run/walk -> maximumMovementSpeed)
        UpdateParameters();

        // This is used to update variables for the capsule casts.
        UpdatePlayerCapsulePosition();

        // Check If the character is close to the ground, grounded, sliding or falling.
        // Set groundNormal and groundAngle values.
        CheckIfGroundedOrSliding();

        Jump();

        // Project movementDirection on the floor's normal to properly constraint movement direction later.
        ProjectMovementDirection();

        // If the player is in front of a wall/steep, constraint the movement direction
        // If the player is in front of a step, jump it
        // If the player is in front of a different terrain, project the movement direction.
        ConstraintMovementDirection();

        // Set the velocity of the character.
        // If the character is not grounded, prevent velocity.y from increase, to avoid the character"flying" when walking up slopes. (This happens because playerGrounded works with a small offset)
        // Clamp velocity.y to avoid constant falling acceleration
        SetVelocity();
        
        // If the character is grounded and not jumping, project the velocity to groundNormal.
        // If the character is sliding, constrain the velocity to make the it slide properly
        // If the character is not on a steep, constraint the velocity magnitude
        ProjectVelocityDirection();

        //Add gravity the player.
        playerRigidbody.AddForce(Physics.gravity*gravityScale,ForceMode.Acceleration);

        
    }

    void UpdateParameters()
    {
        maximumMovementSpeed = (runInput) ? baseMovementSpeed*runSpeedMultiplier : baseMovementSpeed;
    }
    void UpdatePlayerCapsulePosition()
    {
        point1 = playerRigidbody.position + playerCapsuleCollider.center + transform.up *( playerCapsuleCollider.height / 2 - radius );
        point2 = playerRigidbody.position + playerCapsuleCollider.center - transform.up *( playerCapsuleCollider.height / 2 - radius);
    }

    RaycastHit[] CapsuleCastFromPlayer(float radiusScale,Vector3 direction, float distance, int layerMask)
    {
        return Physics.CapsuleCastAll(point1,point2, radius*radiusScale, direction, distance, layerMask);
    }
    RaycastHit[] OptimizedCapsuleCastFromPlayer(float radiusScale,Vector3 direction, float distance, int layerMask)
    {
        return OptimizedCast.CapsuleCastAll(point1,point2, radius*radiusScale, direction, distance, layerMask);
    }

    bool Vector3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.0001;
    }

    void ProjectMovementDirection()
    {
        if (!Vector3Equal(groundNormal, Vector3.zero) && !playerJumping)
        {
            float oldMovementMagnitude = movementDirection.magnitude;
            movementDirection = Vector3.ProjectOnPlane(movementDirection, groundNormal);
            
            // If the new movementDirection isn't 0, scale the movementDirection vector.
            if (!Vector3Equal(movementDirection,Vector3.zero)) 
            {
                movementDirection *= oldMovementMagnitude/movementDirection.magnitude;
            }
        }        
    }



    void CheckIfGroundedOrSliding()
    {
        // CapsuleCast below the player with a distance at least as big as the stepMaxHeight.
        // 2 different grounded booleans:
        //  playerGrounded is true when the player is actually on the ground
        //  playerCloseToGround is true when the character is not grounded but between the distance (Used when climbing down steps, used by the animator.) 


        capsulecastHitArray = OptimizedCapsuleCastFromPlayer(radiusScale,Vector3.down, Mathf.Abs(Mathf.Min(playerRigidbody.velocity.y*Time.fixedDeltaTime,-stepMaxHeight)),environmentLayerMask| enemiesLayerMask);

        if (capsulecastHitArray.Length > 0 ) 
        {   
            playerCloseToGround = true;
            playerSliding = true;
            playerGrounded = false;     
            
            for (int i = capsulecastHitArray.Length-1; i >= 0 ; i--)
            {
                RaycastHit hitInfo;
                groundNormal = capsulecastHitArray[i].normal;
                groundAngle = Vector3.Angle(groundNormal, Vector3.up);

                // Check if the ground's surface isn't a slope and the normal isn't "pointing downwards"
                // The raycast avoid sliding when the character is close to the edge of a slope.
                if ( groundAngle <= steepAngle && groundNormal.y >= 0 )
                {
                    playerSliding = false;
                    //break;

                }
                else if (Physics.Raycast(capsulecastHitArray[i].point+capsulecastHitArray[i].normal*capsulecastHitArray[i].distance,-capsulecastHitArray[i].normal,out hitInfo,capsulecastHitArray[i].distance*1.1f,environmentLayerMask| enemiesLayerMask) )
                {
                    if (  hitInfo.normal != groundNormal  )
                    {
                        playerSliding = false;
                        //break;

                    }
                   
                }

                // Check if the character is grounded
                if ( !playerGrounded && (capsulecastHitArray[i].distance*groundNormal).y < 0.05f)
                {
                    playerGrounded = true;
                }

                if (playerGrounded && !playerSliding)
                {
                    break;
                }
                
            }
        }
        else
        {
            playerCloseToGround = false; 
            playerSliding = false;
            playerGrounded = false;
            
            
            groundNormal = Vector3.zero;
            groundAngle = float.MinValue;
            
        }
    }

    void Jump()
    {
        
        if (playerJumping || playerDoubleJumping)
        {
            if (playerGrounded)
            {
                playerJumping = false;
                playerDoubleJumping = false;
                landingParticles.Play();
                return;
            }
        }

        // TEST JUMP
        if (jumpInput && playerGrounded && !playerSliding && !playerJumping && !playerDoubleJumping )
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x,jumpSpeed,playerRigidbody.velocity.z);
            playerJumping = true;
        }
        else if (jumpInput && !playerSliding && !playerDoubleJumping )
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x,jumpSpeed,playerRigidbody.velocity.z);
            playerJumping = true;
            playerDoubleJumping = true;
        }

        
        jumpInput = false;
    }

    void ConstraintMovementDirection()
    {
        // If there is a movementDirection, capsuleCast in that direction to avoid the player to stick on walls and avoid small terrain variations.
        if (!Vector3Equal(movementDirection, Vector3.zero))
        {
           
            capsulecastHitArray = OptimizedCapsuleCastFromPlayer(radiusScale,movementDirection.normalized,maximumMovementSpeed*Time.fixedDeltaTime,environmentLayerMask | enemiesLayerMask);
        
            // This value is used to mantain the input value after constraining the movementDirection.
            float oldMovementMagnitude = movementDirection.magnitude;

            //foreach (RaycastHit capsulecastHit in capsulecastHitArray)
            for (int i = capsulecastHitArray.Length-1; i >= 0; i--)
            {
                //For colliders that overlap the capsule at the start of the sweep, to avoid problems.
                if (Vector3Equal(Vector3.zero,capsulecastHitArray[i].point))
                {
                    continue;
                }
                
                // If the capsulecast hit a wall/steep:
                //      and the hit height is not allowed
                //      or the normal is "pointing downwards"
                //      or another capsule collider hits any object
                //      or the character is jumping.
                // Constraint movementDirection
                if ( capsulecastHitArray[i].normal.y < 0 || Vector3.Angle(capsulecastHitArray[i].normal, Vector3.up) > steepAngle )
                {
                    float distanceToGround = Mathf.Max(0f,Vector3.Project(capsulecastHitArray[i].point -(point2 -Vector3.up*radius),groundNormal).y); 
                    
                    if ( playerJumping || distanceToGround > stepMaxHeight || capsulecastHitArray[i].normal.y < 0 ||Physics.CapsuleCast(point1+Vector3.up*stepMaxHeight,point2+Vector3.up*stepMaxHeight,radius,movementDirection.normalized,Mathf.Max(capsulecastHitArray[i].normal.y,stepMinDepth),environmentLayerMask | enemiesLayerMask) )
                    {
                        movementDirection -= Vector3.Project(movementDirection, Vector3.Scale(capsulecastHitArray[i].normal,new Vector3(1,0,1)).normalized);
                        //break; 
                    }
                    else
                    {
                        if (playerGrounded)
                        {
                            playerRigidbody.MovePosition(playerRigidbody.position+Vector3.up*distanceToGround);
                            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x,0f,playerRigidbody.velocity.z);
                            break;
                        }
                        continue;
                    }
                }      
                else
                {

                    if (playerGrounded)
                    {
                        // If the gameObject in front isn't a slope or wall, and the character isn't falling, just use its normal as floor normal.
                        groundNormal = capsulecastHitArray[i].normal;
                        groundAngle = Vector3.Angle(groundNormal,Vector3.up);


                        // And project the movement Direction Again
                        ProjectMovementDirection();

                        
                    }

                    continue;

                }  
                
            }
            
  
                // If the new movementDirection isn't 0, scale the movementDirection vector.
                if (!Vector3Equal(movementDirection,Vector3.zero) ) 
                {
                    movementDirection *= oldMovementMagnitude/movementDirection.magnitude;
                }
            
           

        }
    }

    void ProjectVelocityDirection()
    {
        // If the player is grounded in a slope and not jumping, adjust the movement direction. If it is on a steep, it should fall.
        if ( playerGrounded && !playerJumping)
        {
            playerRigidbody.velocity = Vector3.ProjectOnPlane(playerRigidbody.velocity, groundNormal);  
    
            if (playerSliding)
            {  
                playerRigidbody.velocity -= Vector3.Project(playerRigidbody.velocity, Vector3.Scale(groundNormal,new Vector3(1,0,1)).normalized);
                playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x,  -steepSlidingSpeed, playerRigidbody.velocity.z);
            }
            else
            {
                if (!playerPushed )
                {
                    //If the user isn't giving any input, prevent the character from sliding.
                    if ( movementInput.magnitude < 0.1f )
                    {
                        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, Mathf.Max(playerRigidbody.velocity.y, Physics.gravity.magnitude*gravityScale*Time.fixedDeltaTime), playerRigidbody.velocity.z);
                    }
                    //Otherwiswe, clamp the velocity.
                    else
                    {
                        playerRigidbody.velocity = Vector3.ClampMagnitude(playerRigidbody.velocity,maximumMovementSpeed);
                    }
                }
                
            }
        }
    }

    void SetVelocity()
    {
        if (!playerPushed)
        {
            float velocityY = playerRigidbody.velocity.y;
            
            playerRigidbody.velocity = movementDirection*maximumMovementSpeed + Vector3.up*velocityY;
          

            if (!playerGrounded)
            {
                playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x,Mathf.Min(velocityY,playerRigidbody.velocity.y),playerRigidbody.velocity.z);
            }


            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x,Mathf.Max(playerRigidbody.velocity.y,-maximumFallingSpeed),playerRigidbody.velocity.z);
        }
        
    }

    public void Push(Vector3 direction, float force)
    {
        if (playerGrounded && !Vector3Equal(groundNormal,Vector3.up))
        {
            playerRigidbody.velocity = Vector3.up*playerRigidbody.velocity.y + Vector3.ProjectOnPlane(direction,groundNormal)*force;
        }
        else
        {
            playerRigidbody.velocity = Vector3.up*playerRigidbody.velocity.y + direction*force;
        }
        playerPushed = true;
        pushTimer = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SpringJump"))
        {
            playerRigidbody.AddForce(0, 500, 0);
            Debug.Log("jump");
        }
    }
}
