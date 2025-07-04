using UnityEngine;

public class AnimHandler : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    public Animator anim;
    public CatController pc;
    public GameObject deadBody;

    public string _currentState;
    const string PLAYER_RESPAWN = "Respawn";
    const string PLAYER_IDLE = "idle";
    const string PLAYER_IDLE_COMBAT = "Idle_Combat";
    const string PLAYER_RUN_FORWARD = "Run_Forward";
    const string PLAYER_START_JUMP = "Jump";
    const string PLAYER_IN_AIR = "Jump_Loop";
    const string PLAYER_END_JUMP = "Land";
    const string PLAYER_HURT = "Hurt";
    const string PLAYER_ATTACK_1 = "Attack1";
    const string PLAYER_ATTACK_2 = "Attack2";
    const string PLAYER_ATTACK_3 = "Attack3";
    const string PLAYER_ATTACK_4 = "Attack4";

    public bool isEngaged;
    public bool isRespawning;
    public bool isJumping;
    public bool isAttacking;
    public bool isFalling;
    public bool isRunning;
    public bool isLanding;
    public bool isDead;
    public bool triggeredDeadBody;
    public bool triggeredLanding;
    public bool triggeredFalling;
    public bool triggeredGrounding;
    public bool triggeredUnGrounding;
    public bool triggeredHurt;

    public float fallCounter;
    public float fallCD;
    public float landCounter;
    public float landCD;
    public float groundCounter;
    public float groundCD;
    public float hurtCounter;
    public float hurtCD;
    public float engagedCounter;
    public float engagedCD;

    void Awake()
    {
        Respawning();
    }

    void Start()
    {
        if (pc == null)
        {
            if (gameObject.GetComponent<CatController>() != null)
            {
                pc = gameObject.GetComponent<CatController>();
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
    }

    void Update()
    {
        if (isFalling)
        {
            if (pc != null)
            {
                if (pc.groundedPlayer)
                {
                    if (!triggeredLanding)
                    {
                        CheckRecentFall();

                        //Debug.Log("checking recent fall cause still falling yet grounded in pc");
                    }
                }
            }
        }

        if (triggeredFalling)
        {
            if (fallCounter < fallCD)
            {
                fallCounter += Time.deltaTime;
            }
            else
            {
                triggeredFalling = false;
                fallCounter = 0f;
            }
        }

        if (triggeredLanding)
        {
            if (landCounter < landCD)
            {
                landCounter += Time.deltaTime;

                if (pc != null)
                {
                    if (!pc.groundedPlayer)
                    {
                        if (!isJumping)
                        {
                            // Debug.Log("became ungrounded");

                            triggeredUnGrounding = true;

                        }
                    }
                }
            }
            else
            {
                FinishLanding();
            }
        }

        if (triggeredGrounding)
        {
            if (groundCounter < groundCD)
            {
                groundCounter += Time.deltaTime;
            }
            else
            {
                triggeredGrounding = false;
                groundCounter = 0f;
            }
        }

        if (triggeredHurt)
        {
            if (hurtCounter < hurtCD)
            {
                hurtCounter += Time.deltaTime;
            }
            else
            {
                triggeredHurt = false;
                hurtCounter = 0f;
            }
        }

        if (isEngaged)
        {
            if (engagedCounter < engagedCD)
            {
                engagedCounter += Time.deltaTime;
            }
            else
            {
                if (gameManager != null)
                {
                    if (!gameManager.isEngaged)
                    {
                        isEngaged = false;
                    }
                }

                engagedCounter = 0f;
            }
        }
    }

    public void ChangeAnimationState(string newState)
    {
        if (!isDead)
        {
            if (newState == _currentState)
            {
                Debug.Log("trying to change to same state of " + newState);

                return;
            }

            if (anim != null)
            {
                anim.Play(newState);
            }

            _currentState = newState;

            // Debug.Log("Playing state " + newState);
        }
    }

    public bool IsAnimationPlaying(Animator animator, string stateName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
        {
            //Debug.Log("returning true for " + stateName);

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                //Debug.Log("returning true for normalized time");

                return true;
            }
            else
            {

                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void Respawning()
    {
        if (!isRespawning)
        {
            isRespawning = true;

            ChangeAnimationState(PLAYER_RESPAWN);
        }
    }

    public void FinishRespawning()
    {
        if (isRespawning)
        {
            isRespawning = false;

            if (pc != null)
            {
                pc.isRespawning = false;
                pc.canAttack = true;
            }
        }
    }

    public void Engage()
    {
        if (!isEngaged)
        {
            isEngaged = true;
        }
    }

    public void Hurt()
    {
        if (!triggeredHurt)
        {
            triggeredHurt = true;

            engagedCounter = 0f;

            Debug.Log("triggered hurt");
        }
    }

    public void Idle()
    {
        if (!isJumping && !isFalling && !isAttacking && !isRunning && !isLanding && !isRespawning && !triggeredHurt)
        {
            if (!triggeredLanding)
            {
                if (_currentState != PLAYER_IDLE)
                {
                    if (!isEngaged)
                    {
                        ChangeAnimationState(PLAYER_IDLE);
                    }
                    else
                    {
                        CombatIdle();
                    }
                }
            }
        }
    }

    public void CombatIdle()
    {
        if (!isJumping && !isFalling && !isAttacking && !isRunning && !isLanding && !isRespawning && !triggeredHurt)
        {
            if (!triggeredLanding)
            {
                if (_currentState != PLAYER_IDLE_COMBAT)
                {
                    ChangeAnimationState(PLAYER_IDLE_COMBAT);
                }
            }
        }
    }

    public void RunForward()
    {
        if (!isJumping && !isFalling && !isAttacking && !isLanding && !isRespawning && !triggeredHurt)
        {
            if (!triggeredLanding)
            {
                if (_currentState != PLAYER_RUN_FORWARD)
                {
                    ChangeAnimationState(PLAYER_RUN_FORWARD);
                }
            }

            isRunning = true;
        }
    }

    public void StartJump()
    {
        if (!isJumping)
        {
            isJumping = true;

            if (triggeredLanding)
            {
                triggeredLanding = false;
                landCounter = 0f;
                triggeredGrounding = false;
                groundCounter = 0f;
                triggeredUnGrounding = false;
            }

            if (isLanding)
            {
                isLanding = false;
            }

            ChangeAnimationState(PLAYER_START_JUMP);
        }
    }

    public void InAir()
    {
        if (!isJumping && !isAttacking && !isRespawning && !triggeredHurt)
        {
            if (!isFalling)
            {
                if (!triggeredFalling && !triggeredGrounding)
                {
                    if (!triggeredUnGrounding)
                    {
                        isFalling = true;
                        triggeredFalling = true;
                        fallCounter = 0f;

                        ChangeAnimationState(PLAYER_IN_AIR);
                    }
                }
            }

            if (isLanding)
            {
                if (pc != null)
                {
                    if (!pc.groundedPlayer)
                    {
                        if (!triggeredLanding)
                        {
                            isLanding = false;
                        }
                    }
                    else
                    {
                        if (!triggeredLanding)
                        {
                            triggeredLanding = true;
                        }
                    }
                }
            }
        }
    }

    public void EndJump()
    {
        //Debug.Log("called end jump");

        if (!isAttacking)
        {
            if (!isLanding)
            {
                if (!triggeredLanding)
                {
                    triggeredLanding = true;
                    landCounter = 0f;

                    ChangeAnimationState(PLAYER_END_JUMP);
                }
            }
        }

        isFalling = false;
    }

    public void StartHurt()
    {
        if (!isRespawning)
        {
            if (_currentState != PLAYER_HURT)
            {
                if (!triggeredHurt)
                {
                    Hurt();

                    ChangeAnimationState(PLAYER_HURT);
                }

                if (isJumping)
                {
                    isJumping = false;
                }
            }
        }

        if (isAttacking)
        {
            FinishAttack();
        }

        if (!isEngaged)
        {
            isEngaged = true;
        }
    }

    public void EndHurt()
    {
        if (triggeredHurt)
        {
            triggeredHurt = false;
            hurtCounter = 0f;
        }
    }

    public void CheckRecentFall()
    {
        if (isFalling)
        {
            if (!triggeredLanding)
            {
                EndJump();
            }
        }
        else
        {
            if (triggeredGrounding)
            {
                EndJump();

                //Debug.Log("end jump cause check recent fall");
            }
            else
            {
                //Debug.Log("grounding NOT triggered");
            }
        }
    }

    public void CheckRecentAction()
    {
        if (isJumping)
        {
            if (_currentState != PLAYER_START_JUMP)
            {
                isJumping = false;
            }
        }

        if (_currentState != PLAYER_ATTACK_1 && _currentState != PLAYER_ATTACK_2 && _currentState != PLAYER_ATTACK_3 && _currentState != PLAYER_ATTACK_4)
        {
            isAttacking = false;

            if (pc != null)
            {
                pc.EndAttack();
            }
        }

    }

    public void Land()
    {
        isLanding = true;

        //Debug.Log("called Land");
    }

    public void FinishLanding()
    {
        isLanding = false;

        triggeredLanding = false;
        landCounter = 0f;
        triggeredUnGrounding = false;
    }

    public void FinishJumping()
    {
        isJumping = false;

        if (pc != null)
        {
            pc.triggeredJump = false;

            if (!pc.groundedPlayer)
            {
                if (pc.wasGrounded)
                {
                    InAir();
                }
            }
            else
            {
                CheckRecentFall();

                if (!triggeredGrounding)
                {
                    triggeredGrounding = true;

                    //Debug.Log("changed to grounding trigger");
                }
            }
        }
    }

    public void StartAttackOne()
    {
        if (!isAttacking)
        {
            isAttacking = true;

            if (triggeredLanding)
            {
                triggeredLanding = false;
                landCounter = 0f;
                triggeredGrounding = false;
                groundCounter = 0f;
                triggeredUnGrounding = false;
            }

            if (isLanding)
            {
                isLanding = false;
            }

            ChangeAnimationState(PLAYER_ATTACK_1);
        }
    }

    public void StartAttackTwo()
    {
        if (!isAttacking)
        {
            isAttacking = true;

            if (triggeredLanding)
            {
                triggeredLanding = false;
                landCounter = 0f;
                triggeredGrounding = false;
                groundCounter = 0f;
                triggeredUnGrounding = false;
            }

            if (isLanding)
            {
                isLanding = false;
            }

            ChangeAnimationState(PLAYER_ATTACK_2);
        }
    }

    public void StartAttackThree()
    {
        if (!isAttacking)
        {
            isAttacking = true;

            if (triggeredLanding)
            {
                triggeredLanding = false;
                landCounter = 0f;
                triggeredGrounding = false;
                groundCounter = 0f;
                triggeredUnGrounding = false;
            }

            if (isLanding)
            {
                isLanding = false;
            }

            ChangeAnimationState(PLAYER_ATTACK_3);
        }
    }

    public void StartAttackFour()
    {
        if (!isAttacking)
        {
            isAttacking = true;

            if (triggeredLanding)
            {
                triggeredLanding = false;
                landCounter = 0f;
                triggeredGrounding = false;
                groundCounter = 0f;
                triggeredUnGrounding = false;
            }

            if (isLanding)
            {
                isLanding = false;
            }

            ChangeAnimationState(PLAYER_ATTACK_4);
        }
    }

    public void FinishAttack()
    {
        isAttacking = false;

        if (pc != null)
        {
            pc.EndAttack();
        }
    }

    public void SummonAttackOne()
    {
        if (pc != null)
        {
            pc.SummonAttackOne();
        }
    }

    public void SummonAttackOneSecond()
    {
        if (pc != null)
        {
            pc.SummonAttackOneSecond();
        }
    }

    public void SummonAttackTwo()
    {
        if (pc != null)
        {
            pc.SummonAttackTwo();
        }
    }

    public void SummonAttackTwoSecond()
    {
        if (pc != null)
        {
            pc.SummonAttackTwoSecond();
        }
    }

    public void SummonAttackThree()
    {
        if (pc != null)
        {
            pc.SummonAttackThree();
        }
    }

    public void SummonAttackThreeSecond()
    {
        if (pc != null)
        {
            pc.SummonAttackThreeSecond();
        }
    }

    public void SummonAttackFour()
    {
        if (pc != null)
        {
            pc.SummonAttackFour();
        }
    }

    public void SummonAttackFourSecond()
    {
        if (pc != null)
        {
            pc.SummonAttackFourSecond();
        }
    }

    public void StartCharging()
    {
        if (pc != null)
        {
            pc.StartCharging();
        }
    }

    public void EndCharging()
    {
        if (pc != null)
        {
            pc.EndCharging();
        }
    }

    public void ApplyDeath()
    {
        if (!triggeredDeadBody)
        {
            triggeredDeadBody = true;

            if (deadBody != null)
            {
                Instantiate(deadBody, transform.position, transform.rotation);
            }

            if (gameObject != null)
            {
                Destroy(gameObject, 0.1f);
            }
        }
    }
}
