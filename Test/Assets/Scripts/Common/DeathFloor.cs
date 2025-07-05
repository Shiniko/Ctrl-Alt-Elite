using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFloor : MonoBehaviour
{
    public bool killOnContact;
    public bool dealDamage;

    public float damage;

    public string lastAffected;

    public float rateCounter;
    public float rate;

    void Update()
    {
        if (rateCounter < rate)
        {
            rateCounter += Time.deltaTime;
        }
        else
        {
            rateCounter = 0f;
            lastAffected = null;
        }
    }

    public void OnEnterTrigger(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("collided with " + col.name);

            if (col.name != lastAffected)
            {
                if (col.GetComponent<PlayerHealth>() != null)
                {
                    PlayerHealth ph = col.GetComponent<PlayerHealth>();

                    HandlePlayer(ph);

                    lastAffected = col.name;
                }
            }
        }

        if(col.CompareTag("Enemy"))
        {
            if (col.GetComponent<EnemyHealth>() != null)
            {
                EnemyHealth eh = col.GetComponent<EnemyHealth>();

                HandleEnemy(eh);
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("collided with " + col.name);

            if (col.name != lastAffected)
            {
                if (col.GetComponent<PlayerHealth>() != null)
                {
                    PlayerHealth ph = col.GetComponent<PlayerHealth>();

                    HandlePlayer(ph);

                    lastAffected = col.name;
                }
            }
        }
    }

    public void HandlePlayer(PlayerHealth health)
    {
        if (dealDamage)
        {
            if(health != null)
            {
                health.ApplyDamage(damage);
            }
        }

        if (killOnContact)
        {
            if(health != null)
            {
                health.KillPlayer();

                Debug.Log("sending kill to playerhealth");
            }
        }
    }

    public void HandleEnemy(EnemyHealth health)
    {
        if (dealDamage)
        {
            if (health != null)
            {
                health.ApplyDamage(damage);
            }

            if (killOnContact)
            {
                if (health != null)
                {
                    health.KillEnemy();
                }
            }
        }
    }
}
