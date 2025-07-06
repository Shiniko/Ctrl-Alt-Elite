using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Guirao.UltimateTextDamage;
using UnityEngine.UI;
using DestroyIt;


public class EnemyHealth : MonoBehaviour
{
    [Header("Text Damage Parameters")]
    [SerializeField] private EnemyController enemyController;
    public UltimateTextDamageManager textManager;
    public Transform textDamagePos;
    public Animator anim;
    [SerializeField] private Destructible destructible;
    [SerializeField] private Destructible[] destructibleChildren;
    public int destChildCount;

    public string damageSound;
    public string destroySound;
    public bool useChildren;

    [Header("Damage Parameters")]
    public bool isBoss;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private bool triggeredDeath;
    public bool enemyDead;
    public float deathDelay;

    [SerializeField] private bool isRespawning = true;

    [SerializeField] private bool isInvul;
    [SerializeField] private float invulCounter;
    [SerializeField] private float invulCD;

    [SerializeField] private float regenCounter;
    [SerializeField] private float regenCD;

    [Header("Health Parameters")]
    [SerializeField] private float enemyHP;
    [SerializeField] private float enemyBaseHP;
    [SerializeField] private float enemyMaxHP;
    public float adjustedMaxHP;

    [SerializeField] private float oldEnHP;
    [SerializeField] private float newEnHP;

    [SerializeField] private float amountTaken;
    [SerializeField] private float maxHPDiff;

    [Header("Stats")]
    [SerializeField] private float enemyRegen;
    [SerializeField] private float adjustedRegen;

    [SerializeField] private float enemyArmor;
    [SerializeField] private float adjustedArmor;

    [Header("Bonus Parameters")]
    public float bonusMaxHP;
    public float bonusRegen;
    public float bonusArmor;

    [Header("UI Health")]
    [SerializeField] private GameObject healEffect;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private Image currentEnemyHP;
    [SerializeField] private Image oldEnemyHP;
    [SerializeField] private Image newEnemyHP;
    [SerializeField] private float shortenCounter;
    [SerializeField] private float shortenCD;
    [SerializeField] private float shortenRate;
    [SerializeField] private float growCounter;
    [SerializeField] private float growCD;
    [SerializeField] private float growRate;

    void Start()
    {
        if (textManager == null)
        {
            if (GameObject.FindGameObjectWithTag("TextDamageManager").GetComponent<UltimateTextDamageManager>() != null)
            {
                textManager = GameObject.FindGameObjectWithTag("TextDamageManager").GetComponent<UltimateTextDamageManager>();
            }
        }

        if(gameObject.GetComponent<Destructible>() != null)
        {
            destructible = gameObject.GetComponent<Destructible>();
        }

        if(destructible != null)
        {
            destructible._totalHitPoints = adjustedMaxHP;
            destructible._currentHitPoints = adjustedMaxHP;
        }

        if (useChildren)
        {
            destructibleChildren = GetComponentsInChildren<Destructible>();
            destChildCount = destructibleChildren.Length;

            if (destChildCount > 0 && destChildCount <= destructibleChildren.Length)
            {
                for (int i = 0; i < destChildCount; i++)
                {
                    if (destructibleChildren[i] != null)
                    {
                        destructibleChildren[i]._totalHitPoints = adjustedMaxHP / destChildCount;
                        destructibleChildren[i]._currentHitPoints = adjustedMaxHP / destChildCount;
                    }
                }
            }
        }

        if (isBoss)
        {
            if (gameManager == null)
            {
                if (GameObject.FindGameObjectWithTag("GameController") != null)
                {
                    gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRespawning)
        {
            if (!enemyDead)
            {
                if (isInvul)
                {
                    if (invulCounter < invulCD)
                    {
                        invulCounter += Time.deltaTime;
                    }
                    else
                    {
                        isInvul = false;

                        invulCounter = 0f;
                    }
                }
            }
        }

        if (healthBar != null)
        {
            if (shortenCounter < shortenCD)
            {
                shortenCounter += Time.deltaTime;
            }
            else
            {
                shortenCounter = shortenCD;

                if (currentEnemyHP != null && oldEnemyHP != null)
                {
                    if (currentEnemyHP.fillAmount < oldEnemyHP.fillAmount)
                    {
                        float shortenAmount = shortenRate * Time.deltaTime;
                        oldEnemyHP.fillAmount -= shortenAmount;
                    }
                    else
                    {
                        if (currentEnemyHP.fillAmount > oldEnemyHP.fillAmount)
                        {
                            oldEnemyHP.fillAmount = currentEnemyHP.fillAmount;
                        }
                    }
                }
            }

            if (growCounter < growCD)
            {
                growCounter += Time.deltaTime;
            }
            else
            {
                growCounter = growCD;

                if (currentEnemyHP != null && newEnemyHP != null)
                {
                    if (currentEnemyHP.fillAmount < newEnemyHP.fillAmount)
                    {
                        float growAmount = growRate * Time.deltaTime;
                        currentEnemyHP.fillAmount += growAmount;
                    }
                    else
                    {
                        if (currentEnemyHP.fillAmount > newEnemyHP.fillAmount)
                        {
                            newEnemyHP.fillAmount = currentEnemyHP.fillAmount;
                        }
                    }
                }
            }
        }
    }

    public void KillEnemy()
    {
        if (gameObject != null)
        {
            Destroy(gameObject, deathDelay);
        }
    }

    public void ApplyDamage(float damageTaken)
    {
        if (!isRespawning)
        {
            if (!enemyDead)
            {
                if (!isInvul)
                {
                    amountTaken = damageTaken - adjustedArmor;

                    if (amountTaken < 0f)
                    {
                        amountTaken = 0.25f;
                    }

                    isInvul = true;

                    oldEnHP = enemyHP;
                    enemyHP -= amountTaken;

                    DisplayText(amountTaken);

                    if (enemyHP <= 0f)
                    {
                        enemyHP = 0f;

                        enemyDead = true;
                    }

                    if (healthBar != null)
                    {
                        AdjustHealthUIDamage(enemyHP, adjustedMaxHP);
                    }

                    if (isBoss)
                    {
                        if(gameManager != null)
                        {
                            //gameManager.AdjustHealthUIBossDamage(enemyHP, adjustedMaxHP);
                        }
                    }

                    if (enemyDead)
                    {
                        if (!triggeredDeath)
                        {
                            triggeredDeath = true;

                            ApplyDeath();

                            KillEnemy();

                            if (destroySound != "")
                            {
                                GameObject.FindGameObjectWithTag("AudioDispatcher").GetComponent<AudioDispatcher>().PlayClip(destroySound);
                            }
                        }
                    }
                    else
                    {
                        if (anim != null)
                        {
                            //anim.StartHurt();
                        }

                        if (damageSound != "")
                        {
                            GameObject.FindGameObjectWithTag("AudioDispatcher").GetComponent<AudioDispatcher>().PlayClip(damageSound);
                        }
                    }

                    if (destructible != null)
                    {
                        destructible.ApplyDamage(amountTaken);
                    }

                    if (useChildren)
                    {
                        if (destChildCount > 0 && destChildCount <= destructibleChildren.Length)
                        {
                            for (int i = 0; i < destChildCount; i++)
                            {
                                if (destructibleChildren[i] != null)
                                {
                                    if (!enemyDead)
                                    {
                                        destructibleChildren[i].ApplyDamage(amountTaken / destChildCount);
                                    }
                                    else
                                    {
                                        float tempHP = destructibleChildren[i]._currentHitPoints + 1f;
                                        destructibleChildren[i].ApplyDamage(tempHP);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Debug.Log("enemy hit while invulnerable");
                }
            }
            else
            {
                //Debug.Log("enemy hit while dead");
            }
        }
        else
        {
            //Debug.Log("enemy hit while respawning");
        }
    }

    public void AdjustHealthUIDamage(float currentHP, float maxHP)
    {
        if (currentHP < 0f)
        {
            currentHP = 0f;
        }

        if (maxHP != 0f)
        {
            currentEnemyHP.fillAmount = (currentHP / maxHP);
            newEnemyHP.fillAmount = currentEnemyHP.fillAmount;
        }

        //do the damage set
        shortenCounter = 0f;
    }

    public void AdjustHealthUIHeal(float currentHP, float maxHP)
    {
        if (currentHP < 0f)
        {
            currentHP = 0f;
        }

        if (maxHP != 0f)
        {
            newEnemyHP.fillAmount = (currentHP / maxHP);
        }

        //do the damage set
        growCounter = 0f;
    }

    public void PlayHealEffect()
    {
        if (healEffect != null)
        {

        }
    }

    public void DisplayText(float amount)
    {
        int displayAmount = Mathf.RoundToInt((amount));

        if (textManager != null)
        {
            textManager.Add(displayAmount.ToString(), textDamagePos, "default");
        }
    }

    public void DisplayTextHeal(float amount)
    {
        int displayAmount = Mathf.RoundToInt((amount));

        if (textManager != null)
        {
            textManager.Add(displayAmount.ToString(), textDamagePos, "heal");
        }
    }

    public void ApplyDeath()
    {
        if (enemyController != null)
        {
            enemyController.ApplyDeath();
        }
    }
}
