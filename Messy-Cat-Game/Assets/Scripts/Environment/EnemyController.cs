using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] bool isWall;

    [SerializeField] private Rigidbody rb;
    public GameObject body;
    public GameObject deadBody;

    [Header("Movement Settings")]
    public bool isDead;
    public bool triggeredDeadBody;
    public bool canMove;
    public bool freezeMove;
    public bool isAttacking;
    public bool canAttack;
    [SerializeField] private float enemySpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravityValue;
    [SerializeField] private float gravityVelocity;
    [SerializeField] private float fallTolerance;

    [SerializeField] private Vector3 enemyVelocity;


    [SerializeField] private float walkSpeed;
    [SerializeField] private float turningSpeed;

    [SerializeField] private Vector2 movement;
    [SerializeField] private Vector3 move;
    public bool isGrounded;
    public bool groundedEnemy;

    [Header("Move")]
    public bool isRespawning;
    public bool isStunned;

    [SerializeField] private Vector3 forwardRelative;
    [SerializeField] private Vector3 rightRelative;
    [SerializeField] private Vector3 moveDir;

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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (!isRespawning)
            {
                if (canKnockback)
                {
                    canMove = true;

                    if (!isAttacking)
                    {
                        freezeMove = false;
                    }
                }

                Movement();
            }
            else
            {
                freezeMove = true;
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

                    ApplyKnockbackReposition(kbEndPos);

                    knockbackNextCounter = 0f;
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
            freezeMove = true;
            canAttack = false;
        }
    }

    private void Movement()
    {
        if (!isWall)
        {

        }
    }

    public void ApplyKnockback(Vector3 kbDir)
    {
        if (!isKnockedback)
        {
            if (canKnockback)
            {
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
                freezeMove = true;

                knockbackCounter = 0f;
                knockbackNextCounter = 0f;

                enemyVelocity = Vector3.zero;

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

                Vector3 targetVelocity = -kbDir * tempkbForce;

                enemyVelocity += targetVelocity;
            }
            else
            {
                Debug.Log("can knockback was false");
            }
        }
        else
        {
            Debug.Log("knockedback was true");
        }
    }

    private void ApplyKnockbackReposition(Vector3 targetPos)
    {
            enemyVelocity = Vector3.zero;      
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
        }
    }
}
