using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardDamage : MonoBehaviour
{
    public float damage;
    public bool applyKnockback;
    public bool applyToBreakables;

    [SerializeField] private float damageRate;
    [SerializeField] private float rateCounter;
    [SerializeField] private bool canAreaDamage;

    void Start()
    {
        
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
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<PlayerHealth>().ApplyDamage(damage);
            
            if (applyKnockback)
            {
                Vector3 eDir = (transform.position - col.transform.position).normalized;
                col.GetComponent<CatController>().ApplyKnockback(eDir);
            }      
        }

        if (applyToBreakables)
        {
            if (col.CompareTag("Breakable"))
            {
                col.GetComponent<EnemyHealth>().ApplyDamage(damage);

                if (applyKnockback)
                {
                    Vector3 eDir = (transform.position - col.transform.position).normalized;
                    //col.GetComponent<PlayerController>().ApplyKnockback(eDir);
                }
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (canAreaDamage)
            {
                canAreaDamage = false;

                col.GetComponent<PlayerHealth>().ApplyDamage(damage);

                rateCounter = 0f;
            }

            if (applyKnockback)
            {
                Vector3 eDir = (transform.position - col.transform.position).normalized;
                col.GetComponent<CatController>().ApplyKnockback(eDir);
            }
        }

        if (applyToBreakables)
        {
            if (col.CompareTag("Breakable"))
            {

                if (canAreaDamage)
                {
                    canAreaDamage = false;

                    col.GetComponent<EnemyHealth>().ApplyDamage(damage);

                    rateCounter = 0f;
                }

                if (applyKnockback)
                {
                    Vector3 eDir = (transform.position - col.transform.position).normalized;
                    //col.GetComponent<PlayerController>().ApplyKnockback(eDir);
                }
            }
        }
    }
}
