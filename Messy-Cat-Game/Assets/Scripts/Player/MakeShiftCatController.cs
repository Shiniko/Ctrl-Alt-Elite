using UnityEngine;

public class MakeShiftCatController : MonoBehaviour
{
    [Header("Player Targets")]
    public Exit_Interact exitTarget;
    public Hide_Interact hideTarget;
    public Spill_Interact spillTarget;
    public Scratch_Interact scratchTarget;
    public Break_Interact breakTarget;

    [Header("Player States")]
    [SerializeField] private bool canMove; // when player inputs can move player
    public bool isHidden;
    public bool triggerHide;
    public GameObject hideCoat;
    public GameObject catCoat;

    public bool isEngaged;
    public bool isOverUI;
    [SerializeField] private bool isDead;
    [SerializeField] private bool isInCutscene;

    [Header("Movement Details")]
    [SerializeField] private Vector3 movement = Vector3.zero;
    [SerializeField] private Vector3 velocity = Vector3.zero;
    [SerializeField] private float moveX;
    [SerializeField] private float moveY;
    [SerializeField] private float velocityY;
    [SerializeField] private float topmaxYvelocity;
    [SerializeField] private float topminYvelocity;
    [SerializeField] private float maxYvelocity;
    [SerializeField] private float minYvelocity;

    [Header("Initial Stats")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;
    public GameObject body;
    [SerializeField] private GameObject deadBody;
    [SerializeField] private GameObject model;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ankleCheck;

    [Header("Movement Params")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runfactor;
    [SerializeField] private float gravity;

    [SerializeField] private bool triggeredFall;
    [SerializeField] private float velToTriggerFall;
    [SerializeField] private float velToTriggerHardLand;

    [Header("Smoothing Params")]
    [SerializeField] private float smoothInputSpeed;  //smoothing rate
    private Vector2 currentInputVector;         //used to calculate a input from player to affect smooth velocity
    private Vector2 smoothInputVelocity;        //used as x velocity to give a smooth accleration to player movement

    [Header("Target Params")]
    public GameObject target;                               //game object to pass in as victim to cat scratching
    [SerializeField] private ObjectHealth objectHealth;     //reference to health script of object
    private int currentDamageProgress;                      //generalized progress in increments of 33, 66, and 100, given by animation events of the scratching
    private float damageToInflict;                          //calculated damage to pass on to health script of object

    [Header("GroundChecks")]
    [SerializeField] private float groundDistance; // Distance to check for ground
    [SerializeField] private bool checkingGround; // when actively checking if grounded
    [SerializeField] private bool isGrounded; // Flag to indicate if object is grounded

    [Header("WallChecks")]
    [SerializeField] private float wallDistance; // Distance to check for ground
    [SerializeField] private bool checkingWall; // when actively checking if grounded
    [SerializeField] private bool isNearWall;

    [Header("Input Params")]
    [SerializeField] private bool inputsFrozen;
    [SerializeField] private bool facingRight = true;

    [Header("Respawn Params")]
    public bool isRespawning = true;
    [SerializeField] private float respawnCounter;
    [SerializeField] private float respawnCD;

    public bool triggeredDeath;
    [SerializeField] private float evaporateDelay;

    [Header("Jump Params")]
    [SerializeField] private bool canJump;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool triggeredJump;
    [SerializeField] private float jumpForce;
    [SerializeField] private int jumpCount;
    public int jumpMax;
    [SerializeField] private float jumpCounter;
    [SerializeField] private float jumpCD;

    void Start()
    {
        if (body != null)
        {
            if (rb == null)
            {
                rb = body.GetComponent<Rigidbody>(); //set rb to body rb if null
            }
        }

        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>(); // set animator if null
        }
    }

    void Update()
    {
        if (!isDead)
        {
            if (!isRespawning)
            {
                if (!triggerHide)
                {
                    Debug.Log("setting canmove to true because, not triggerhide");

                    canMove = true;
                }
                else
                {
                    Debug.Log("intial setting canmove to false because, triggerhide");
                    canMove = false;

                    if (isHidden)
                    {
                        Debug.Log("setting canmove to true because, triggerhide and isHidden");
                        canMove = true;
                    }
                }

                checkingGround = true;
                checkingWall = true;
                inputsFrozen = false;
            }
            else
            {
                canMove = false;
                checkingGround = false;
                checkingWall = false;
                inputsFrozen = true;

                if (isRespawning)
                {
                    if (respawnCounter < respawnCD)
                    {
                        respawnCounter += Time.deltaTime;
                    }
                    else
                    {
                        respawnCounter = respawnCD;

                        //isRespawning = false; //This is set in animation handler to set false when done with respawn animation

                        //remove this once animation event is setup
                        isRespawning = false;
                    }
                }
            }

            if (!inputsFrozen)
            {
                CheckInputs();
            }

            if (checkingGround)
            {
                CheckGround();
            }

            if (checkingWall)
            {
                CheckWall();
            }

            if (rb != null)
            {
                HandleYVelocity();
            }
        }
        else
        {
            // deal with dead body?

            canMove = false;

            //knockback
            //ApplyKBSlow();  //optional use; slower knockback method if needed to lerp a Vertical velocity
            //gravity

            ApplyGravity(); //apply gravity so matches dead body instantiate position
        }
    }

    private void HandleYVelocity()
    {
        velocityY = rb.linearVelocity.y; //store rb y velocity

        //check if postitive y velocity above max and set to max if so
        if (velocityY > maxYvelocity)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, maxYvelocity, 0);
        }

        //check if falling too hard and set to max negative y velocity
        if (velocityY < minYvelocity)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, minYvelocity, 0);
        }

        velocityY = rb.linearVelocity.y;

        //set topmax Vertical for debugging - no movement value -  Note: this value does not reflect current rb velocity, but rather the stored velocity before max/min adjusting
        if (velocityY > topmaxYvelocity)
        {
            topmaxYvelocity = velocityY;
        }

        //set topmin Vertical for debugging - no movement value -  Note: this value does not reflect current rb velocity, but rather the stored velocity before max/min adjusting
        if (velocityY < topminYvelocity)
        {
            topminYvelocity = velocityY;
        }

        //check if falling hard enough for hardland animation
        if (anim != null)
        {
            anim.SetFloat("velocityY", velocityY);

            if (!isGrounded) // if grounded no reason to check for velocityY regarding hard landing animation
            {
                if (velocityY < velToTriggerHardLand)
                {
                    anim.SetBool("hardLand", true);
                }
                else
                {
                    anim.SetBool("hardLand", false);
                }
            }
        }
    }

    private void CheckInputs()
    {

        if (canMove)
        {
            // Get input and set animator parameters
            moveX = Input.GetAxis("Horizontal");
            moveY = Input.GetAxis("Vertical");

            //smoothing start
            Vector2 input = new Vector2(moveX, moveY);
            currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref smoothInputVelocity, smoothInputSpeed);

            //smoothing end

            movement = new Vector3(currentInputVector.x, 0, 0).normalized;

            if (moveX > 0.01f && !facingRight)
            {
                FlipFace();

                movement = new Vector3(moveX, 0, 0).normalized;
                currentInputVector = new Vector2(moveX, 0).normalized;
            }
            else if (moveX < -0.01f && facingRight)
            {
                FlipFace();

                movement = new Vector3(moveX, 0, 0).normalized;
                currentInputVector = new Vector2(moveX, 0).normalized;
            }

            if (Mathf.Abs(moveX) > 0.01f)
            {
                if (anim != null)
                {
                    anim.SetBool("isMoving", true);
                    anim.SetFloat("moveX", Mathf.Abs(moveX));
                    anim.SetFloat("moveY", moveY);
                }

                if (triggerHide || isHidden)
                {
                    Debug.Log("calling stop hiding because, triggerHide true or isHidden true, and moveX >0.01f");
                    StopHiding();
                }
            }
            else
            {
                if (anim != null)
                {
                    anim.SetBool("isMoving", false);
                    anim.SetFloat("moveX", Mathf.Abs(moveX));
                    anim.SetFloat("moveY", moveY);
                }

                movement = new Vector3(moveX, 0, 0).normalized;
                currentInputVector = new Vector2(moveX, 0).normalized;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                CheckInteracts();
            }

            /*
            if (canJump && Input.GetButtonDown("Jump"))
            {
                // because multiple jumps can happen, set rb velocity of y to dimishing amount
                if (rb != null)
                {
                    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.25f, 0);

                    rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
                }

                jumpCount++;

                isJumping = true;

                if (isHanging)
                {
                    Debug.Log("player was able to jump while hanging");

                    isHanging = false;
                    triggeredClimb = false;
                    triggeredHang = false;

                    if (anim != null)
                    {
                        anim.ResetTrigger("triggerHang");
                        anim.SetBool("isHanging", false);

                        anim.ResetTrigger("climUp");
                    }
                }

                if (isWallGrabbing)
                {
                    // change wall grab stuffs
                }

                if (anim != null)
                {
                    anim.SetBool("isJumping", true);

                    if (!triggeredJump)
                    {
                        anim.SetTrigger("triggerJump");
                    }

                    anim.SetBool("isGliding", false);
                    anim.SetBool("isFalling", false);

                    if (triggeredFall)
                    {
                        triggeredFall = false;
                        anim.ResetTrigger("triggerFall");

                        Debug.Log("Reset triggerfall");
                    }
                }

                if (jumpCount >= jumpMax)
                {
                    canJump = false;
                    jumpCounter = 0f;
                }
                else
                {
                    canJump = true;
                    jumpCounter = 0f;
                }
            }

            if (isJumping)
            {
                if (Input.GetButtonUp("Jump"))
                {
                    if (rb != null)
                    {
                        if (!isGliding)
                        {
                            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, 0);
                        }
                    }
                }
            }

            if (hasGlide)
            {
                if (canGlide && Input.GetButton("Jump"))
                {
                    if (!isJumping)
                    {
                        if (!isHanging)
                        {
                            if (!isGliding)
                            {
                                if (!initialDescent)
                                {
                                    if (rb != null)
                                    {
                                        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, 0);
                                        initialDescent = true;
                                    }
                                }

                                isGliding = true;

                                if (anim != null)
                                {
                                    anim.SetBool("isGliding", true);
                                    anim.SetBool("isFalling", false);

                                    anim.ResetTrigger("triggerJump");

                                    triggeredFall = false;
                                }
                            }
                        }
                    }
                }

                if (isGliding && Input.GetButtonUp("Jump"))
                {
                    isGliding = false;
                    initialDescent = false;

                    if (anim != null)
                    {
                        anim.SetBool("isGliding", false);
                    }
                }
            }

            if (hasSlide)
            {
                // check for slide input
            }

            if (hasGrapple)
            {
                // check for grapple input
            }

            if (hasWallGrab)
            {
                // check for wall grab input, similar to 
            }
        */

            if (rb != null)
            {
                MoveCharacter();
            }
        }
        else
        {
            ApplyGravity();
        }

    }

    private void CheckInteracts()
    {
        if (hideTarget != null)
        {
            TryToHide();
            return;
        }

        if (spillTarget != null)
        {
            TryToSpill();
            return;
        }
    }

    public void TryToHide()
    {
        if (!triggerHide)
        {
            triggerHide = true;

            if (!isHidden)
            {
                if (hideTarget != null)
                {
                    //Debug.Log("calling start hiding because, pressed E when triggerHide false, is Hidden false, and hideTarget not null");
                    StartHiding();
                }
                else
                {
                    Debug.Log("hideTarget is null so setting triggerhide to false");
                    triggerHide = false;

                }
            }
        }
    }

    public void TryToSpill()
    {
        // other logic
        if (spillTarget != null)
        {
            spillTarget.Interact();
        }

        StartSpilling();
    }

    void MoveCharacter()
    {
        if (!isNearWall)
        {
            velocity = movement * moveSpeed;

            if (Mathf.Abs(moveX) > 0.71f)
            {
                velocity = movement * (moveSpeed * runfactor);
            }
        }
        else
        {
            velocity = new Vector3(0, rb.linearVelocity.y, 0);
        }

        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, 0);

        ApplyGravity();
    }

    void FlipFace()
    {
        if (body != null)
        {
            facingRight = !facingRight;

            if (model != null)
            {
                Vector3 scaler = model.transform.localScale;
                scaler.z *= -1; // Flip the character by inverting the scale on the Z axis
                model.transform.localScale = scaler;

                //Debug.Log("Flipped Model");
            }
        }
    }

    void CheckGround()
    {

        if (groundCheck != null)
        {
            if (Physics.CheckSphere(groundCheck.position, groundDistance, groundMask))
            {
                isGrounded = true;
                //midGrounded = true;

                Debug.DrawRay(groundCheck.position, Vector3.down * groundDistance, Color.green);
            }
            else
            {
                //midGrounded = false;

                Debug.DrawRay(groundCheck.position, Vector3.down * groundDistance, Color.red);
            }
        }

        /*
        if (edgeCheck != null)
        {
            if (Physics.CheckSphere(edgeCheck.position, groundDistance, groundMask))
            {
                isGrounded = true;
                frontGrounded = true;

                Debug.DrawRay(edgeCheck.position, Vector3.down * groundDistance, Color.green);
            }
            else
            {
                frontGrounded = false;

                Debug.DrawRay(edgeCheck.position, Vector3.down * groundDistance, Color.red);
            }
        }

        if (edgeCheckRear != null)
        {
            if (Physics.CheckSphere(edgeCheckRear.position, groundDistance, groundMask))
            {
                isGrounded = true;
                rearGrounded = true;

                Debug.DrawRay(edgeCheckRear.position, Vector3.down * groundDistance, Color.green);
            }
            else
            {
                rearGrounded = false;

                Debug.DrawRay(edgeCheckRear.position, Vector3.down * groundDistance, Color.red);
            }
        }

        if (!frontGrounded && !midGrounded && !rearGrounded)
        {
            isGrounded = false;
        }

        if (!frontGrounded && !midGrounded && rearGrounded)
        {
            nearEdgeRear = true;
        }
        else
        {
            nearEdgeRear = false;
        }

        if (frontGrounded && !midGrounded && !rearGrounded)
        {
            nearEdgeFront = true;
        }
        else
        {
            nearEdgeFront = false;
        }

        if (!frontGrounded && midGrounded && !rearGrounded)
        {
            isTeetering = true;
        }
        else
        {
            isTeetering = false;
        }

        */

        ApplyGroundState();
    }

    void CheckWall()
    {
        if (ankleCheck != null)
        {
            if (Physics.CheckSphere(ankleCheck.position, wallDistance, groundMask))
            {
                isNearWall = true;
            }
            else
            {
                isNearWall = false;
            }
        }

        /*
        if (ankleCheck != null)
        {
            if (Physics.CheckSphere(ankleCheck.position, wallDistance, groundMask))
            {
                isNearWall = true;
                ankleWall = true;
            }
            else
            {
                ankleWall = false;
            }
        }

        if (midCheck != null)
        {
            if (Physics.CheckSphere(midCheck.position, wallDistance, groundMask))
            {
                isNearWall = true;
                midWall = true;
            }
            else
            {
                midWall = false;
            }
        }

        if (thighCheck != null)
        {
            if (Physics.CheckSphere(thighCheck.position, wallDistance, groundMask))
            {
                isNearWall = true;
                thighWall = true;
            }
            else
            {
                thighWall = false;
            }
        }

        if (faceCheck != null)
        {
            if (Physics.CheckSphere(faceCheck.position, wallDistance, groundMask))
            {
                isNearWall = true;
                faceWall = true;
            }
            else
            {
                faceWall = false;
            }
        }

        if (!ankleWall && !midWall && !thighWall && !faceWall)
        {
            isNearWall = false;
        }

        if (isNearWall)
        {
            if (!faceWall && midWall && !isGrounded)
            {
                hangStart = true;
            }
            else
            {
                hangStart = false;
            }

            if (!faceWall && !midWall && !isGrounded)
            {
                miniClimb = true;
            }
            else
            {
                miniClimb = false;
            }

            if (hangStart)
            {
                if (!triggeredHang)
                {
                    triggeredHang = true;

                    StartHangSequence();
                }
            }
        }
        */
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            rb.linearVelocity += new Vector3(0, -(gravity * Time.deltaTime), 0);

            //Debug.Log("appplying regular gravity");

            if (rb.linearVelocity.y > velToTriggerFall)
            {
                if (rb.linearVelocity.y < -0.1f)
                {
                    rb.linearVelocity += new Vector3(0, -(gravity * Time.deltaTime), 0);
                }
            }
        }
        else
        {
            if (!isJumping)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);

                velocityY = rb.linearVelocity.y;
            }
        }

        /*
        if (!isGrounded)
        {
            if (!isHanging)
            {
                if (!isWallGrabbing)
                {
                    if (!isGliding)
                    {
                        rb.velocity += new Vector3(0, -(gravity * Time.deltaTime), 0);

                        //Debug.Log("appplying regular gravity");

                        if (rb.velocity.y > velToTriggerFall)
                        {
                            if (rb.velocity.y < -0.1f)
                            {
                                rb.velocity += new Vector3(0, -(gravity * Time.deltaTime), 0);
                            }
                        }
                    }
                    else
                    {
                        if ((velocityY + gravity) >= (gravity * 0.9f))
                        {
                            rb.velocity += new Vector3(0, -(gravity * glideModifier * Time.deltaTime), 0); //glide modifier should be between 0 and 1
                        }
                        else
                        {
                            if ((velocityY + gravity) >= (gravity * 0.75f))
                            {
                                rb.velocity += new Vector3(0, gravity * glideModifier * 0.5f * Time.deltaTime, 0);
                            }
                            else
                            {
                                rb.velocity += new Vector3(0, 0, 0); //glide modifier should be between 0 and 1
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (!isJumping)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, 0);

                velocityY = rb.velocity.y;
            }
        }

        if (velocityY < velToTriggerFall)
        {
            if (!isHanging && !isGliding && !isAttacking && !isWallGrabbing)
            {
                if (anim != null)
                {
                    if (!triggeredFall)
                    {
                        triggeredFall = true;
                        triggeredJump = false;

                        anim.ResetTrigger("triggerJump");

                        anim.SetBool("isFalling", true);
                        anim.SetTrigger("triggerFall");

                        Debug.Log("Triggered Fall");// and vel is " + velocityY);
                    }
                }
            }
            else
            {
                // check things here is fall not triggering correctly
                Debug.Log("something stopped falling to trigger");
            }
        }
        else
        {
            if (!isJumping)
            {
                if (triggeredFall)
                {
                    if (anim != null)
                    {
                        anim.SetBool("isFalling", false);
                        anim.ResetTrigger("triggerFall");

                        Debug.Log("Reset Triggered Fall cause velocity above threshold");
                    }

                    triggeredFall = false;
                }
            }
        }
        */
    }

    void ApplyGroundState()
    {

        if (isGrounded)
        {
            //Debug.Log("isGrounded");

            if (rb != null)
            {
                if (rb.linearVelocity.y < -0.1f || rb.linearVelocity.y > 0.1f)
                {
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);

                    //Debug.Log("Reset rb velocity to zero in ground state");
                }
            }
        }

        /*
            initialDescent = false;

            if (isGliding)
            {
                isGliding = false;
            }

            if (jumpCounter >= jumpCD)
            {
                canJump = true;
                jumpCount = 0;
                isJumping = false;

                if (anim != null)
                {
                    anim.SetBool("isJumping", false);
                }
            }

            if (slideCounter >= slideCD)
            {
                canSlide = true;
                slideCount = 0;
            }

            canGlide = false;
            triggeredFall = false;

            if (anim != null)
            {
                if (!isJumping)
                {
                    anim.SetBool("isGrounded", true);
                    anim.ResetTrigger("triggerFall");
                }

                anim.SetBool("isGliding", false);
                anim.SetBool("isFalling", false);
            }
        }
        else
        {
            if (!isHanging)
            {
                canGlide = true;
            }

            if (anim != null)
            {
                anim.SetBool("isGrounded", false);
            }
        }
        */
    }

    //Anim Dealer Collabs

    public void StartHiding()
    {
        movement = new Vector3(0f, 0f, 0f).normalized;
        moveX = 0f;

        if(rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }

        if (!facingRight)
        {
            FlipFace();
        }

        if (hideTarget != null)
        {
            float hideX = hideTarget.transform.position.x;
            Vector3 newPosition = new Vector3(hideX, transform.position.y, transform.position.z);

            transform.position = newPosition;
        }

        if (anim != null)
        {
            anim.SetBool("isHiding", true);
            anim.Play("Start_Hide");
        }
    }

    public void StopHiding()
    {
        isHidden = false;
        triggerHide = false;

        if (anim != null)
        {
            anim.SetBool("isHiding", false);
        }

        if (hideCoat != null)
        {
            hideCoat.SetActive(false);
        }

        if(catCoat != null)
        {
            catCoat.SetActive(true);
        }
    }

    public void FinishedHiding()
    {
        if (!isHidden)
        {
            if (hideCoat != null)
            {
                hideCoat.SetActive(true);
            }

            if (catCoat != null)
            {
                catCoat.SetActive(false);
            }

            isHidden = true;
        }
    }

    public void StartSpilling()
    {
        if (spillTarget != null)
        {
            float spillX = spillTarget.transform.position.x;
            Vector3 newPosition = new Vector3(spillX, transform.position.y, transform.position.z);

            transform.position = newPosition;
        }

        if (anim != null)
        {
            anim.SetBool("isSpilling", true);
            anim.Play("Start_Spill");
        }
    }

    public void EndVictory()
    {
        if (anim != null)
        {
            anim.SetBool("isVictorious", false);
        }
    }

    public void ThirtyThreeScratch()
    {
        // add 33 progress to target

        if (target != null)
        {
            currentDamageProgress += 33;

            CheckScratchProgress();
        }
    }

    public void SixtySixScratch()
    {
        // add another 33 progress to target

        if (target != null)
        {
            currentDamageProgress += 33;

            CheckScratchProgress();
        }
    }

    public void FullScratch()
    {
        // add 100 progress to target

        if (target != null)
        {
            currentDamageProgress += 100;

            CheckScratchProgress();
        }
    }

    private void CheckScratchProgress()
    {
        if (currentDamageProgress >= 100)
        {
            if (target != null)
            {
                if (objectHealth != null)
                {
                    float damageLeft = objectHealth.currentHealth;
                    damageToInflict = damageLeft * 1.05f;

                    ApplyObjectDamage(damageToInflict);
                }

                ResetObjectTarget();
            }
            else
            {
                ResetObjectTarget();
            }
        }
        else
        {
            if (objectHealth != null)
            {
                float damageLeft = objectHealth.currentHealth;
                float damageMax = objectHealth.adjustedMaxHealth;
                float damageCompare = damageMax * 0.333f;

                if (damageCompare > damageLeft)
                {
                    damageToInflict = damageLeft * 1.05f;
                }
                else
                {
                    damageToInflict = damageMax;
                }

                ApplyObjectDamage(damageToInflict);

                if (damageToInflict >= damageCompare)
                {
                    ResetObjectTarget();
                }
            }
            else
            {
                ResetObjectTarget();
            }
        }
    }

    private void ApplyObjectDamage(float damage)
    {
        if (objectHealth != null)
        {
            objectHealth.ApplyDamage(damage);
        }
    }

    public void ResetObjectTarget()
    {
        currentDamageProgress = 0;
        target = null;
        objectHealth = null;
        damageToInflict = 0f;
    }

    public void SetObjectTarget(GameObject ot)
    {
        if (ot == null)
        {
            return;
        }

        target = ot;

        if (ot.GetComponentInChildren<ObjectHealth>() != null)
        {
            objectHealth = ot.GetComponentInChildren<ObjectHealth>();

            float currentObjectHealth = objectHealth.currentHealth;
            float maxObjectHealth = objectHealth.adjustedMaxHealth;
            float healthRatio = 0f;

            if (maxObjectHealth > 0.1f)
            {
                healthRatio = currentObjectHealth / maxObjectHealth;
            }

            if (healthRatio > 0.99f)
            {
                currentDamageProgress = 0;
            }
            else
            {
                int ratioToInt = Mathf.CeilToInt((1f - healthRatio) * 100f);
                currentDamageProgress = ratioToInt;
            }

        }
    }

    public void ApplyDeath()
    {
        // called from game manager
    }

    //optional

    void ApplyKBSlow()
    {
        if (!isGrounded)
        {

        }
        else
        {
            Vector3 newVel;
            Vector3 oldVel = rb.linearVelocity;
            newVel = new Vector3(0, rb.linearVelocity.y, 0);
            rb.linearVelocity = Vector3.Lerp(oldVel, newVel, 0.025f);
        }
    }
}
