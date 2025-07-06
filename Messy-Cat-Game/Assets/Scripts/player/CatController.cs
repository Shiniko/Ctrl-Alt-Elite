using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class CatController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] bool isArcher;
    [SerializeField] bool isBattleMage;
    [SerializeField] bool isGunner;
    [SerializeField] bool isMage;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider cc;
    [SerializeField] private float colliderSize;
    public GameObject body;

    private CharacterController controller;
    public Transform focusCamera;
    public CinemachineCamera centerCam;
    public bool xcenterActive;
    public bool ycenterActive;

    public AnimHandler anim;

    [SerializeField] private GameManager gameManager;

    [Header("Attack References")]
    public Sprite[] attackIcons;
    [SerializeField] private GameObject attack1;
    [SerializeField] private GameObject attack1s;
    [SerializeField] private GameObject attack2;
    [SerializeField] private GameObject attack2s;
    [SerializeField] private GameObject attack3;
    [SerializeField] private GameObject attack3s;
    [SerializeField] private GameObject attack4;
    [SerializeField] private GameObject attack4s;

    [Header("Attack Settings")]
    public Transform frontFirePoint;
    public Transform frontFirePointProjection;

    public LayerMask attackLayer;
    public int skipCount;
    public int skipThreshold;
    public bool wasAttacking;
    public bool skipRotateFrames;

    public int attackSelection;

    public float attackCounter;
    public float attackCD;
    public float attackOneCD;
    public float attackTwoCD;
    public float attackThreeCD;
    public float attackFourCD;

    [Header("Movement Settings")]
    public bool isDead;
    public bool isOverUI;
    public bool canMove;
    public bool freezeInput;
    public bool chargingStill;
    public bool isAttacking;
    public bool canAttack;
    public bool inCutScene;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravityValue;
    [SerializeField] private float gravityVelocity;
    [SerializeField] private float fallTolerance;

    [SerializeField] private Vector3 playerVelocity;
    [SerializeField] private Vector3 rigidBodyVelocity;
    [SerializeField] private Vector3 targetVelocity;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float turningSpeed;

    [SerializeField] private Vector2 movement;
    [SerializeField] private Vector3 move;
    [SerializeField] private Vector3 mousePosition;


    [Header("Input")]
    public bool isRespawning;
    public bool isStunned;
    public bool isPaused;
    [SerializeField] private float coyoteCD;
    [SerializeField] private float coyoteCounter;
    [SerializeField] private bool coyoteInEffect;

    private float moveInput;
    private float turnInput;
    [SerializeField] private Vector3 forwardRelative;
    [SerializeField] private Vector3 rightRelative;
    [SerializeField] private Vector3 moveDir;

    public bool applyJump;
    public bool triggeredJump;
    public bool canJump;

    [Header("Rotation")]
    [SerializeField] private Quaternion targetRotation;
    [SerializeField] private float targetAngle;

    [SerializeField] protected Vector3 currentTargetRotation;
    [SerializeField] protected Vector3 durationToReachTargetRotation;
    [SerializeField] protected Vector3 dampedTargetRotationCurrentVelocity;
    [SerializeField] protected Vector3 dampedTargetRotationPassedDuration;

    [Header("Knockback Params")]
    [SerializeField] private bool canKnockback;
    [SerializeField] private bool isKnockedback;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float tempkbForce;
    [SerializeField] private float knockbackCounter;
    [SerializeField] private float knockbackCD;
    [SerializeField] private float knockbackNextCounter;
    [SerializeField] private float knockbackNextCD;

    [SerializeField] private Vector3 kbStartPos;
    [SerializeField] private Vector3 kbEndPos;

    [Header("New")]
    public LayerMask groundLayer;
    public float groundCheckDistance;
    public float gravityScale = 1f;

    public bool tempGrounded;
    public bool wasGrounded;
    public bool groundedPlayerRays;
    public bool groundedPlayer;

    public Transform frontGroundCheck;
    public Transform leftGroundCheck;
    public Transform rightGroundCheck;
    public float rayLength = 0.3f; // Length of each ray

    public Vector3 slopeVelocity;
    public Vector3 newForce;
    public float slopeCheckDistance;
    public bool onSlope;
    public float slopeModifier;

    void Awake()
    {
        durationToReachTargetRotation.y = 0.14f;

        if (gameObject.GetComponent<CapsuleCollider>() != null)
        {
            if (cc == null)
            {
                cc = gameObject.GetComponent<CapsuleCollider>();
            }

            if (cc != null)
            {
                colliderSize = cc.height;
            }
        }

        if (focusCamera == null)
        {
            focusCamera = Camera.main.transform;
        }

        if (centerCam == null)
        {
            centerCam = gameObject.GetComponentInChildren<CinemachineCamera>();
        }

        if (controller == null)
        {
            if (gameObject.GetComponent<CharacterController>() != null)
            {
                controller = gameObject.GetComponent<CharacterController>();
            }
        }

        if (gameManager == null)
        {
            if (GameObject.FindGameObjectWithTag("GameController") != null)
            {
                if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>() != null)
                {
                    gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
                }
            }
        }

        if (anim == null)
        {
            if (gameObject.GetComponent<AnimHandler>() != null)
            {
                anim = gameObject.GetComponent<AnimHandler>();
            }
        }

        if (rb == null)
        {
            rb = gameObject.GetComponent<Rigidbody>();

        }

        attackCounter = attackCD;
        attackSelection = 1;

        if (gameManager != null)
        {
            //gameManager.ChangeAttackSelection(attackSelection);
        }
        else
        {
            ChangeAttackSelection(attackSelection);
        }

        SetupAttackIcons();
    }

    void Update()
    {

        if (gameManager != null)
        {
            isPaused = gameManager.isPaused;
        }

        if (!inCutScene)
        {
            /*
            bool forwardProjection = Physics.Raycast(transform.position, playerVelocity, knockbackForce, groundLayer);

            Debug.DrawRay(transform.position, playerVelocity * (knockbackForce * Time.deltaTime * 30f), forwardProjection ? Color.green : Color.red);
            */
            bool slopeHit = Physics.Raycast(transform.position, Vector3.down, slopeCheckDistance, groundLayer);
            Debug.DrawRay(transform.position, Vector3.down * slopeCheckDistance, slopeHit ? Color.green : Color.red);

            if (focusCamera == null)
            {
                if (GameObject.FindGameObjectWithTag("MainCamera") != null)
                {
                    focusCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
                }
            }

            if (!isDead)
            {
                if (!isRespawning)
                {
                    if (canKnockback)
                    {
                        canMove = true;

                        if (!isAttacking)
                        {
                            freezeInput = false;
                        }
                    }

                    HandleMovement();
                }
                else
                {
                    freezeInput = true;
                }

                if (isKnockedback)
                {
                    if (knockbackCounter < knockbackCD)
                    {
                        knockbackCounter += Time.deltaTime;
                    }
                    else
                    {
                        isKnockedback = false;

                        knockbackCounter = 0f;
                    }
                }

                if (!canKnockback)
                {
                    if (knockbackNextCounter < knockbackNextCD)
                    {
                        knockbackNextCounter += Time.deltaTime;
                    }
                    else
                    {
                        canKnockback = true;

                        kbEndPos = transform.position;

                        //ApplyKnockbackReposition(kbEndPos);

                        knockbackNextCounter = 0f;
                    }
                }

                if (attackCounter < attackCD)
                {
                    canAttack = false;

                    attackCounter += Time.deltaTime;
                }
                else
                {
                    canAttack = true;

                    attackCounter = attackCD;
                }

                if (!isPaused)
                {
                    if (Input.GetButtonDown("Cancel"))
                    {
                        isPaused = true;

                        if (gameManager != null)
                        {
                            gameManager.GamePausedEsc();
                        }
                    }
                }
            }
            else
            {
                if (isKnockedback)
                {
                    if (knockbackCounter < knockbackCD)
                    {
                        knockbackCounter += Time.deltaTime;
                    }
                    else
                    {
                        isKnockedback = false;

                        knockbackCounter = 0f;
                    }
                }

                canMove = false;
                freezeInput = true;
                canAttack = false;
            }
        }
    }

    private float Rotate(Vector3 mdir)
    {
        float directionAngle = UpdateTargetRotation(mdir);

        RotateTowardsTargetRotation();

        return directionAngle;
    }

    private void UpdateTargetRotationData(float targetAngle)
    {
        currentTargetRotation.y = targetAngle;

        dampedTargetRotationPassedDuration.y = 0f;
    }

    private float GetDirectionAngle(Vector3 dir)
    {
        float directionAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        if (directionAngle < 0f)
        {
            directionAngle += 360f;
        }

        return directionAngle;
    }

    private float AddCameraRotationToAngle(float angle)
    {
        if (focusCamera != null)
        {
            angle += focusCamera.eulerAngles.y;
        }

        if (angle > 360f)
        {
            angle -= 360f;
        }

        return angle;
    }

    protected void RotateTowardsTargetRotation()
    {
        if (body != null)
        {
            float currentYAngle = rb.transform.rotation.eulerAngles.y;

            if (currentYAngle == currentTargetRotation.y)
            {
                return;
            }

            float smoothYAngle = Mathf.SmoothDampAngle(currentYAngle, currentTargetRotation.y, ref dampedTargetRotationCurrentVelocity.y, durationToReachTargetRotation.y - dampedTargetRotationPassedDuration.y);

            dampedTargetRotationPassedDuration.y += Time.deltaTime;

            Quaternion targetRot = Quaternion.Euler(0f, smoothYAngle, 0f);

            body.transform.rotation = Quaternion.Slerp(body.transform.rotation, targetRot, turningSpeed * Time.deltaTime); //rotate player
        }
    }

    protected float UpdateTargetRotation(Vector3 dir, bool shouldCOnsiderCameraRotation = true)
    {
        float directionAngle = GetDirectionAngle(dir);

        if (shouldCOnsiderCameraRotation)
        {
            directionAngle = AddCameraRotationToAngle(directionAngle);
        }

        if (directionAngle != currentTargetRotation.y)
        {
            UpdateTargetRotationData(directionAngle);
        }

        return directionAngle;
    }

    protected Vector3 GetTargetRotationDirection(float targetAngle)
    {
        return Quaternion.Euler(0f, targetAngle, 0f) * rb.transform.forward;
    }

    private void Movement()
    {
        //second thing
        if (!freezeInput && !chargingStill)
        {
            movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        move = new Vector3(movement.x, 0f, movement.y);

        if (focusCamera != null)
        {
            //move = focusCamera.forward * move.z + focusCamera.right * move.x;
            move = Quaternion.AngleAxis(focusCamera.rotation.eulerAngles.y, Vector3.up) * move;
        }

        move.Normalize();

        move.y = 0f;

        /* 
        if (canMove)
        {
            controller.Move(move * Time.deltaTime * playerSpeed);
        }

        //rotate player instant
        /* 
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
        */
    }

    public void FreezeVelocity()
    {
        playerVelocity = Vector3.zero;

        if (rb != null)
        {
            rb.linearVelocity = playerVelocity;
        }

        if (anim != null)
        {
            anim.isRunning = false;

            anim.Idle();
        }
    }

    private void HandleMovement()
    {
        rigidBodyVelocity = rb.linearVelocity;

        tempGrounded = groundedPlayer;

        CheckGrounded();

        SlopeCheck();

        /*
        if (tempGrounded)
        {
            if (!groundedPlayer)
            {
                if (!wasGrounded)
                {
                    wasGrounded = true;
                }

                triggeredJump = false;

                tempGrounded = false;

                Debug.Log("turning on was Grounded");
            }
        }
        */

        if (groundedPlayer)
        {
            coyoteCounter = 0f;
            coyoteInEffect = false;

            if (anim != null)
            {
                anim.CheckRecentAction();
            }

        }
        else
        {
            if (canJump)
            {
                if (coyoteCounter < coyoteCD)
                {
                    coyoteCounter += Time.deltaTime;

                    coyoteInEffect = true;
                }
                else
                {
                    coyoteInEffect = false;

                    coyoteCounter = coyoteCD;
                }
            }
        }

        // first thing
        if (groundedPlayer)
        {
            if (playerVelocity.y < fallTolerance)
            {
                if (anim != null)
                {
                    anim.CheckRecentFall();

                    // Debug.Log("Checking recent fall because grounded yet y vel below tolerance");
                }
            }

            //playerVelocity.y = -fallTolerance * Time.deltaTime;
        }

        //Movement();

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 forward = focusCamera.forward;
        Vector3 right = focusCamera.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * vertical + right * horizontal).normalized;
        //Vector3 moveDirection = (forward).normalized;
        //Vector3 moveDirection = (forward * vertical + horizontal * right).normalized;

        if (moveDirection != Vector3.zero && !chargingStill)
        {
            if (anim != null)
            {
                anim.RunForward();
            }

            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turningSpeed * Time.deltaTime);
            //transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, turningSpeed * Time.deltaTime); //rotate player
            //transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, turningSpeed * Time.deltaTime); //rotate player

            if (body != null)
            {
                bool tempAttack = isAttacking;

                if (!wasAttacking && tempAttack)
                {
                    skipRotateFrames = true;
                }

                wasAttacking = isAttacking;

                //my method
                if (!skipRotateFrames)
                {
                    body.transform.rotation = Quaternion.Slerp(body.transform.rotation, toRotation, turningSpeed * Time.deltaTime); //rotate player
                }
                else
                {
                    skipCount++;

                    if (skipCount >= skipThreshold)
                    {
                        skipCount = 0;
                        skipRotateFrames = false;
                    }
                }

                //transform.rotation = Quaternion.Slerp(body.transform.rotation, toRotation, turningSpeed * Time.deltaTime); //rotate player

                /*
                //Genshin Method

                float targetRotationYAngle = Rotate(moveDirection);

                Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

                playerVelocity.x = targetRotationDirection.x * playerSpeed;
                playerVelocity.z = targetRotationDirection.z * playerSpeed;
                //End Genshin Method
                */

                //my method

                playerVelocity.x = moveDirection.x * playerSpeed;
                playerVelocity.z = moveDirection.z * playerSpeed;
            }

            //Vector3 velocity = moveDirection * playerSpeed;


            //Debug.Log("settingplayer velocity x and y");

            if (canMove)
            {
                rb.linearVelocity = new Vector3(playerVelocity.x, rb.linearVelocity.y, playerVelocity.z);
            }
        }
        else
        {
            if (!isKnockedback)
            {
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            }

            if (anim != null && groundedPlayer)
            {
                anim.Idle();

                anim.isRunning = false;
            }
        }

        if (!freezeInput)
        {
            CheckInputs();
        }

        ApplyGravity();

        if (centerCam != null)
        {
            if (Mathf.Abs(horizontal) >= 0.6f)
            {
                if (!xcenterActive)
                {
                    xcenterActive = true;
                    EnableXRecenter();
                }
            }
            else
            {
                if (!chargingStill)
                {
                    if (xcenterActive)
                    {
                        xcenterActive = false;
                        DisableXRecenter();
                    }
                }
            }

            CinemachinePanTilt panTilt = centerCam.GetComponent<CinemachinePanTilt>();

            if (panTilt != null)
            {
                if (panTilt.TiltAxis.Value > 0f)
                {
                    DisableYRecenter();
                }
                else
                {
                    if (panTilt.TiltAxis.Value < -0.5f)
                    {
                        EnableYRecenter();
                    }
                }
            }
        }

    }

    public void SlopeCheck()
    {
        Vector3 checkPos = transform.position + new Vector3(0.0f, colliderSize * 0.5f, 0.0f);

        VertSlopeCheck(checkPos);
    }

    public void HorizSlopeCheck(Vector3 checkPos)
    {

    }

    public void VertSlopeCheck(Vector3 checkPos)
    {
        RaycastHit hit;

        bool slopeHit = Physics.Raycast(checkPos, Vector3.down, slopeCheckDistance, groundLayer);

        if (Physics.Raycast(checkPos, Vector3.down, out hit, slopeCheckDistance, groundLayer))
        {
            if (groundedPlayer && hit.normal.y != 1f)
            {
                onSlope = true;

                slopeModifier = hit.normal.y * 9.81f;

                Debug.DrawRay(checkPos, hit.normal * slopeCheckDistance, slopeHit ? Color.blue : Color.red);

                slopeVelocity = -hit.normal;

                newForce = new Vector3(slopeVelocity.x, slopeVelocity.y * slopeModifier, slopeVelocity.z);

            }
            else
            {
                onSlope = false;

                slopeModifier = 0f;
            }
        }
    }

    public void EnableYRecenter()
    {
        if (centerCam != null)
        {
            centerCam.GetComponent<CinemachinePanTilt>().TiltAxis.Recentering.Enabled = true;
        }
    }

    public void DisableYRecenter()
    {
        if (centerCam != null)
        {
            centerCam.GetComponent<CinemachinePanTilt>().TiltAxis.Recentering.Enabled = false;
        }
    }

    public void EnableXRecenter()
    {
        if (centerCam != null)
        {
            centerCam.GetComponent<CinemachinePanTilt>().PanAxis.Recentering.Enabled = true;
        }
    }

    public void DisableXRecenter()
    {
        if (centerCam != null)
        {
            centerCam.GetComponent<CinemachinePanTilt>().PanAxis.Recentering.Enabled = false;
        }
    }

    public void CheckGrounded()
    {
        if (frontGroundCheck != null && leftGroundCheck != null && rightGroundCheck != null)
        {
            // Perform raycasts from the specified transforms
            bool frontHit = Physics.Raycast(frontGroundCheck.position, Vector3.down, groundCheckDistance, groundLayer);
            bool leftHit = Physics.Raycast(leftGroundCheck.position, Vector3.down, groundCheckDistance, groundLayer);
            bool rightHit = Physics.Raycast(rightGroundCheck.position, Vector3.down, groundCheckDistance, groundLayer);

            bool frontHitRay = Physics.Raycast(frontGroundCheck.position, Vector3.down, rayLength, groundLayer);
            bool leftHitRay = Physics.Raycast(leftGroundCheck.position, Vector3.down, rayLength, groundLayer);
            bool rightHitRay = Physics.Raycast(rightGroundCheck.position, Vector3.down, rayLength, groundLayer);

            // Determine grounded status

            groundedPlayerRays = frontHitRay || leftHitRay || rightHitRay;

            groundedPlayer = frontHit || leftHit || rightHit;

            if (groundedPlayer && groundedPlayerRays)
            {
                wasGrounded = false;

                if (!triggeredJump)
                {
                    canJump = true;
                }
            }

            if (!groundedPlayer && !groundedPlayerRays)
            {
                wasGrounded = true;
                triggeredJump = false;
            }

            Debug.DrawRay(frontGroundCheck.position, Vector3.down * groundCheckDistance, frontHit ? Color.green : Color.red);
            Debug.DrawRay(leftGroundCheck.position, Vector3.down * groundCheckDistance, leftHit ? Color.green : Color.red);
            Debug.DrawRay(rightGroundCheck.position, Vector3.down * groundCheckDistance, rightHit ? Color.green : Color.red);

            Debug.DrawRay(frontGroundCheck.position, Vector3.down * rayLength, frontHitRay ? Color.green : Color.red);
            Debug.DrawRay(leftGroundCheck.position, Vector3.down * rayLength, leftHitRay ? Color.green : Color.red);
            Debug.DrawRay(rightGroundCheck.position, Vector3.down * rayLength, rightHitRay ? Color.green : Color.red);

            //OnDrawGizmos();
        }
    }

    private void ApplyGravity()
    {
        if (groundedPlayerRays && !triggeredJump)
        {
            // Ensure no vertical velocity when grounded
            playerVelocity.y = 0f;
        }
        else
        {
            // Apply gravity when not grounded, not during coyote time or during knockback (jump buffering)
            if (!coyoteInEffect || isKnockedback)
            {
                if (!groundedPlayer)
                {
                    playerVelocity.y += gravityValue * Time.deltaTime;
                }
                else
                {
                    if (onSlope && !triggeredJump)
                    {
                        playerVelocity.y += newForce.y * 0.15f;
                    }
                }
            }

            // Trigger falling animation if below the fall tolerance and not grounded
            if (playerVelocity.y < fallTolerance)
            {
                // Extra gravity applied for more pronounced fall


                if (!groundedPlayer)
                {
                    playerVelocity.y += gravityValue * Time.deltaTime;

                    if (anim != null)
                    {
                        anim.InAir();
                    }
                }
                else
                {
                    if (onSlope && !triggeredJump)
                    {
                        playerVelocity.y += newForce.y * 0.15f;
                    }
                }

                if (playerVelocity.y < gravityValue * 3.5f)
                {
                    playerVelocity.y = gravityValue * 3.5f;
                }
            }
        }

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, playerVelocity.y, rb.linearVelocity.z);
    }

    private void CheckInputs()
    {
        // Makes the player jump
        if (canJump)
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (groundedPlayer || coyoteInEffect)
                {
                    coyoteInEffect = false;
                    coyoteCounter = coyoteCD;

                    if (!applyJump)
                    {
                        applyJump = true;
                    }
                }
            }
        }

        if (applyJump && !triggeredJump)
        {
            triggeredJump = true;
            applyJump = false;
            canJump = false;

            //Debug.Log("set grounded to false cause jump");

            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);

            if (anim != null)
            {
                anim.StartJump();
            }

            EndAttack();
        }

        // Makes the player Attack

        if (!isOverUI)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (!isAttacking)
                {
                    if (canAttack)
                    {
                        isAttacking = true;
                        canAttack = false;

                        if (triggeredJump)
                        {
                            triggeredJump = false;
                        }

                        AttackRotate();

                        if (attackSelection <= 0)
                        {
                            attackSelection = 1;
                        }

                        if (attackSelection == 1)
                        {
                            ApplyAttackOne();
                        }

                        if (attackSelection == 2)
                        {
                            ApplyAttackTwo();
                        }

                        if (attackSelection == 3)
                        {
                            ApplyAttackThree();
                        }

                        if (attackSelection >= 5)
                        {
                            attackSelection = 4;
                        }

                        if (attackSelection == 4)
                        {
                            ApplyAttackFour();
                        }
                    }
                }
            }
        }

        //Checking attackSwitch

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (gameManager != null)
            {
                //gameManager.ChangeAttackSelection(1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (gameManager != null)
            {
                //gameManager.ChangeAttackSelection(2);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (gameManager != null)
            {
                //gameManager.ChangeAttackSelection(3);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (gameManager != null)
            {
                //gameManager.ChangeAttackSelection(4);
            }
        }
    }

    private void AttackRotate()
    {
        movement = Vector2.zero;
        move = Vector3.zero;

        playerVelocity.x = 0f;
        playerVelocity.z = 0f;

        mousePosition = Input.mousePosition;
        mousePosition.z = 120f;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        //Vector2 direction = new Vector2(mousePosition.x - body.transform.position.x, mousePosition.z - body.transform.position.z);
        Vector3 direction = new Vector3(mousePosition.x - body.transform.position.x, mousePosition.y - body.transform.position.y, mousePosition.z - body.transform.position.z);

        direction = new Vector3(0.01f, mousePosition.y - body.transform.position.y, 0.01f);

        Debug.DrawRay(frontFirePoint.transform.position, mousePosition - frontFirePoint.transform.position, Color.blue);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200, attackLayer))
        {
            Vector3 hitPos = hit.point;

            Debug.Log("hitpoint " + hit.point);

            body.transform.LookAt(new Vector3(hit.point.x, body.transform.position.y, hit.point.z));

            frontFirePoint.transform.LookAt(hit.point);
        }

        //body.transform.up = -direction;

    }

    public void ApplyKnockback(Vector3 kbDir)
    {
        if (!isKnockedback)
        {
            if (canKnockback)
            {
                canMove = false;
                //kbDir.z = 0;
                //kbDir.Normalize();

                //Debug.Log("kb dir is " + kbDir);
                Vector3 startPos = transform.position;


                if (kbDir.y > 0.4f)
                {
                    kbDir = new Vector3(kbDir.x, kbDir.y - 0.75f, kbDir.z).normalized;
                }

                isKnockedback = true;
                canKnockback = false;
                freezeInput = true;

                knockbackCounter = 0f;
                knockbackNextCounter = 0f;

                playerVelocity = Vector3.zero;

                tempkbForce = 0f;

                if (kbDir.y > 0.6f)
                {
                    tempkbForce = knockbackForce * 0.9f;

                    if (kbDir.y > 0.8f)
                    {
                        kbDir.y *= 0.75f;
                    }
                    else
                    {
                        kbDir.y *= 0.9f;
                    }
                }
                else
                {
                    tempkbForce = knockbackForce;
                }

                targetVelocity = -kbDir * tempkbForce;

                playerVelocity += targetVelocity;

                kbStartPos = (transform.position + playerVelocity);

                rb.linearVelocity = new Vector3(playerVelocity.x, rb.linearVelocity.y, playerVelocity.z);
            }
            else
            {
                //Debug.Log("can knockback was false");
            }
        }
        else
        {
            // Debug.Log("knockedback was true");
        }
    }

    private void ApplyKnockbackReposition(Vector3 targetPos)
    {
        if (controller != null)
        {
            controller.enabled = false;
            controller.transform.position = targetPos;
            playerVelocity = Vector3.zero;
            controller.enabled = true;
        }
    }

    public void ApplyDeath()
    {
        isDead = true;
    }

    public void ChangeAttackSelection(int selection)
    {
        if (selection <= 0)
        {
            selection = 1;
        }

        if (selection >= 5)
        {
            selection = 4;
        }

        attackSelection = selection;
    }

    public void SetupAttackIcons()
    {
        for (int i = 0; i < attackIcons.Length; i++)
        {
            if (gameManager != null && attackIcons[i] != null)
            {
                //gameManager.attackIcons[i].sprite = attackIcons[i];
            }
        }
    }

    public void ApplyGMAttackCounter()
    {
        if (gameManager != null)
        {
            //gameManager.AdjustAttackCountdown(attackCD, attackCounter);

            //gameManager.StartAttackCounter();
        }
    }

    public void ApplyAttackOne()
    {
        //freezeInput = true;

        if (anim != null)
        {
            anim.StartAttackOne();
        }

        attackCounter = 0f;
        attackCD = attackOneCD;

        ApplyGMAttackCounter();
    }

    public void ApplyAttackTwo()
    {
        if (anim != null)
        {
            anim.StartAttackTwo();
        }

        attackCounter = 0f;
        attackCD = attackTwoCD;

        ApplyGMAttackCounter();
    }

    public void ApplyAttackThree()
    {
        if (anim != null)
        {
            anim.StartAttackThree();
        }

        if (isBattleMage || isMage)
        {
            chargingStill = true;

            move = Vector2.zero;
            movement = Vector3.zero;
        }

        attackCounter = 0f;
        attackCD = attackThreeCD;

        ApplyGMAttackCounter();
    }

    public void ApplyAttackFour()
    {
        if (anim != null)
        {
            anim.StartAttackFour();
        }

        if (isBattleMage || isMage)
        {
            chargingStill = true;

            move = Vector2.zero;
            movement = Vector3.zero;
        }

        attackCounter = 0f;
        attackCD = attackFourCD;

        ApplyGMAttackCounter();
    }

    public void EndAttack()
    {
        if (!isStunned)
        {
            //freezeInput = false;
            canAttack = true;
        }

        if (chargingStill)
        {
            chargingStill = false;
        }

        if (skipRotateFrames)
        {
            skipRotateFrames = false;
            skipCount = 0;
        }

        isAttacking = false;
    }

    public void StartCharging()
    {
        chargingStill = true;

        move = Vector2.zero;
        movement = Vector3.zero;
    }

    public void EndCharging()
    {
        chargingStill = false;
    }

    public void SummonAttackOne()
    {
        if (attack1 != null)
        {
            if (frontFirePoint != null)
            {
                GameObject a1 = Instantiate(attack1, frontFirePoint.position, frontFirePoint.rotation);
            }
        }
    }

    public void SummonAttackOneSecond()
    {
        if (attack1s != null)
        {
            if (frontFirePoint != null)
            {
                GameObject a1s = Instantiate(attack1, frontFirePoint.position, frontFirePoint.rotation);
            }
        }
    }

    public void SummonAttackTwo()
    {
        if (attack2 != null)
        {
            if (frontFirePoint != null)
            {
                GameObject a2 = Instantiate(attack2, frontFirePoint.position, frontFirePoint.rotation);
            }
        }
    }

    public void SummonAttackTwoSecond()
    {
        if (attack2s != null)
        {
            if (frontFirePoint != null)
            {
                GameObject a2s = Instantiate(attack2s, frontFirePoint.position, frontFirePoint.rotation);
            }
        }
    }

    public void SummonAttackThree()
    {
        if (attack3 != null)
        {

            if (frontFirePoint != null)
            {
                GameObject a3 = Instantiate(attack3, frontFirePoint.position, frontFirePoint.rotation);
            }

        }
    }

    public void SummonAttackThreeSecond()
    {
        if (attack3s != null)
        {
            if (frontFirePoint != null)
            {
                GameObject a3s = Instantiate(attack3s, frontFirePoint.position, frontFirePoint.rotation);
            }
        }
    }

    public void SummonAttackFour()
    {
        if (attack4 != null)
        {
            if (isBattleMage)
            {
                if (frontFirePointProjection != null)
                {
                    GameObject a4 = Instantiate(attack4, frontFirePointProjection.position, frontFirePointProjection.rotation);
                }
            }
            else
            {
                if (frontFirePoint != null)
                {
                    GameObject a4 = Instantiate(attack4, frontFirePoint.position, frontFirePoint.rotation);
                }
            }
        }
    }

    public void SummonAttackFourSecond()
    {
        if (attack4 != null)
        {
            if (frontFirePoint != null)
            {
                GameObject a4s = Instantiate(attack4s, frontFirePoint.position, frontFirePoint.rotation);
            }
        }
    }

    private void OnDrawGizmos()
    {

        if (frontGroundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(frontGroundCheck.position, frontGroundCheck.position + Vector3.down * groundCheckDistance);
        }

        if (leftGroundCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(leftGroundCheck.position, leftGroundCheck.position + Vector3.down * groundCheckDistance);
        }

        if (rightGroundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(rightGroundCheck.position, rightGroundCheck.position + Vector3.down * groundCheckDistance);
        }
    }

}
