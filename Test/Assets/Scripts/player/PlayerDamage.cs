using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public float damage;
    public bool applyKnockback;
    public bool applyDot;
    public bool moveDamagePos;
    public bool adjustScale;
    public bool removeVisualOnImpact;
    public bool removedVisual;
    public bool combustibleCloud;
    public bool combusted;
    public float scaleAmount;
    public float lastScaleAmount;

    public CapsuleCollider damageCapCollider;
    //public RFX4_RaycastCollision coll;
    public GameObject visual;
    public GameObject combustibleEffect;

    [SerializeField] private float damageRate;
    [SerializeField] private float rateCounter;
    [SerializeField] private bool canAreaDamage;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 targetPosition;

    void Start()
    {
        startPosition = transform.position;
    }


    void Update()
    {
        if (rateCounter < damageRate)
        {
            rateCounter += Time.deltaTime;
            canAreaDamage = false;
        }
        else
        {
            rateCounter = damageRate;
            canAreaDamage = true;
        }

        /*
        if (adjustScale)
        {
            if(damageCapCollider != null)
            {
                if(coll != null)
                {                  
                    damageCapCollider.height = coll.lengthScaleToPass;
                    scaleAmount = coll.lengthScaleToPass;

                    if (lastScaleAmount < scaleAmount)
                    {
                        targetPosition = startPosition;

                        //float direction = transform.forward;
                        targetPosition = transform.position + transform.forward * (coll.lengthScaleToPass * 0.5f);

                        //targetPosition = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z + (coll.lengthScaleToPass * 0.5f));

                        transform.position = targetPosition;

                        lastScaleAmount = scaleAmount;
                    }

                    if(transform.position != targetPosition)
                    {
                        transform.position = targetPosition;
                    }
                }
            }
        }
        */
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Breakable"))
        {
            if (moveDamagePos)
            {
                if (col.GetComponent<EnemyHealth>().textDamagePos != null)
                {
                    col.GetComponent<EnemyHealth>().textDamagePos.position = transform.position;
                }
            }

            col.GetComponent<EnemyHealth>().ApplyDamage(damage);

            if (applyKnockback)
            {
                Vector3 eDir = (transform.position - col.transform.position).normalized;
                col.GetComponent<EnemyController>().ApplyKnockback(eDir);
            }
        }

        if (col.CompareTag("Enemy"))
        {
            if (moveDamagePos)
            {
                if (col.GetComponent<EnemyHealth>().textDamagePos != null)
                {
                    col.GetComponent<EnemyHealth>().textDamagePos.position = transform.position;
                }
            }

            col.GetComponent<EnemyHealth>().ApplyDamage(damage);

            if (applyKnockback)
            {
                Vector3 eDir = (transform.position - col.transform.position).normalized;
                col.GetComponent<EnemyController>().ApplyKnockback(eDir);
            }
        }

        if (removeVisualOnImpact)
        {
            if (!removedVisual)
            {
                removedVisual = true;

                if(visual != null)
                {
                    if (!col.CompareTag("Respawn"))
                    {
                        visual.SetActive(false);

                       // Debug.Log("Removed " + visual + " because of " + col.name);
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (applyDot)
        {
            if (col.CompareTag("Breakable"))
            {
                if (canAreaDamage)
                {
                    canAreaDamage = false;

                    col.GetComponent<EnemyHealth>().ApplyDamage(damage);

                    rateCounter = 0f;
                }
            }

            if (col.CompareTag("Enemy"))
            {
                if (canAreaDamage)
                {
                    canAreaDamage = false;

                    col.GetComponent<EnemyHealth>().ApplyDamage(damage);

                    rateCounter = 0f;
                }
            }
        }

        if (combustibleCloud)
        {
            if (col.CompareTag("Flasher"))
            {
                if (!combusted)
                {
                    combusted = true;

                    if(combustibleEffect != null)
                    {
                        Instantiate(combustibleEffect, transform.position, transform.rotation);
                    }
                }
            }
        }
    }
}
