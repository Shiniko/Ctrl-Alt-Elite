using UnityEngine;
using Guirao.UltimateTextDamage;

public class PlayerHealth : MonoBehaviour
{
    [Header("Text Damage Parameters")]
    public UltimateTextDamageManager textManager;
    public Transform textDamagePos;

    public bool playerDead;
    public string damageSound;
    public string destroySound;

    public AnimHandler anim;

    [Header("Initial Parameters")]
    [SerializeField] private CatController pc;
    [SerializeField] private GameManager gm;
    [SerializeField] private bool setInitial;

    [Header("Checks")]
    public bool inCombat;
    public bool fullHP;

    [SerializeField] private bool fastRecover;
    [SerializeField] private float fastRecoverMultiplier;
    [SerializeField] private bool appliedBonus;

    [SerializeField] private bool isRespawning = true;

    [SerializeField] private bool isInvul;
    [SerializeField] private float invulCounter;
    [SerializeField] private float invulCD;

    [SerializeField] private float regenCounter;
    [SerializeField] private float regenCD;

    [SerializeField] private bool triggeredDeath;

    [Header("Health Parameters")]
    [SerializeField] private float playerHP;
    [SerializeField] private float playerBaseHP;
    [SerializeField] private float playerMaxHP;
    [SerializeField] private float adjustedMaxHP;

    [SerializeField] private float oldPlayerHP;
    [SerializeField] private float newPlayerHP;

    [SerializeField] private float amountTaken;
    [SerializeField] private float maxHPDiff;

    [Header("Stats")]
    [SerializeField] private float playerRegen;
    [SerializeField] private float adjustedRegen;

    [SerializeField] private float playerArmor;
    [SerializeField] private float adjustedArmor;

    [Header("Bonus Parameters")]
    public float bonusMaxHP;
    public float bonusRegen;
    public float bonusArmor;

    private void Awake()
    {
        playerHP = playerBaseHP;
        playerMaxHP = playerBaseHP;
        adjustedMaxHP = playerBaseHP;
    }

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("GameController") != null)
        {
            gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }

        if (anim == null)
        {
            if (gameObject.GetComponent<AnimHandler>() != null)
            {
                anim = gameObject.GetComponent<AnimHandler>();
            }
        }

        if (pc == null)
        {
            if (gameObject.GetComponent<CatController>() != null)
            {
                pc = gameObject.GetComponent<CatController>();
            }
        }

        if (textManager == null)
        {
            if (GameObject.FindGameObjectWithTag("TextDamageManager") != null)
            {
                if (GameObject.FindGameObjectWithTag("TextDamageManager").GetComponent<UltimateTextDamageManager>() != null)
                {
                    textManager = GameObject.FindGameObjectWithTag("TextDamageManager").GetComponent<UltimateTextDamageManager>();
                }
            }
        }
    }

    void Update()
    {
        if (!isRespawning)
        {
            if (!playerDead)
            {
                if (!appliedBonus)
                {
                    ApplyBonuses();
                }

                if (appliedBonus)
                {
                    if (playerHP >= adjustedMaxHP)
                    {
                        fullHP = true;
                    }
                    else
                    {
                        fullHP = false;
                        fastRecover = false;
                    }

                    if (anim != null)
                    {
                        inCombat = anim.isEngaged;
                    }

                    if (!fullHP)
                    {
                        if (!inCombat)
                        {
                            fastRecover = true;
                        }

                        if (!fastRecover)
                        {
                            if (regenCounter < regenCD)
                            {
                                regenCounter += Time.deltaTime;
                            }
                            else
                            {
                                ApplyRegen(adjustedRegen);

                                regenCounter = 0f;
                            }
                        }
                        else
                        {
                            if (regenCounter < (regenCD * 0.5))
                            {
                                regenCounter += Time.deltaTime;
                            }
                            else
                            {
                                ApplyRegen(adjustedRegen);

                                regenCounter = 0f;
                            }
                        }
                    }
                }

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
        else
        {
            if (pc != null)
            {
                isRespawning = pc.isRespawning;
            }
        }
    }

    public void ApplyBonuses()
    {
        adjustedMaxHP = playerBaseHP + bonusMaxHP;
        adjustedRegen = playerRegen + bonusRegen;
        adjustedArmor = playerArmor + bonusArmor;

        maxHPDiff = adjustedMaxHP - playerBaseHP;
        newPlayerHP = adjustedMaxHP;

        appliedBonus = true;

        if (!setInitial)
        {
            setInitial = true;

            ApplyHeal(adjustedMaxHP);
        }
    }

    public void KillPlayer()
    {
        Debug.Log("Killed Player");

        playerDead = true;

        if (anim != null)
        {
            anim.isDead = true;

            anim.ApplyDeath();
        }
    }

    public void ApplyDamage(float damage)
    {
        if (!isRespawning)
        {
            if (!playerDead)
            {
                if (!isInvul)
                {
                    amountTaken = damage - adjustedArmor;

                    if (amountTaken < 0f)
                    {
                        amountTaken = 0.25f;
                    }

                    isInvul = true;

                    oldPlayerHP = playerHP;
                    playerHP -= amountTaken;

                    DisplayText(amountTaken);

                    if (playerHP <= 0f)
                    {
                        playerHP = 0f;

                        playerDead = true;
                    }

                    if (gm != null)
                    {
                        gm.AdjustHealthUIDamage(playerHP, adjustedMaxHP);

                        if (playerDead)
                        {
                            if (!triggeredDeath)
                            {
                                triggeredDeath = true;

                                if (gm != null)
                                {
                                    gm.ApplyDeath();
                                }

                                KillPlayer();

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
                                anim.StartHurt();
                            }

                            if (damageSound != "")
                            {
                                GameObject.FindGameObjectWithTag("AudioDispatcher").GetComponent<AudioDispatcher>().PlayClip(damageSound);
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("player hit while invulnerable");
                }
            }
            else
            {
                Debug.Log("player hit while dead");
            }
        }
        else
        {
            Debug.Log("player hit while respawning");
        }
    }

    public void ApplyHeal(float heal)
    {
        if (!isRespawning)
        {
            if (!playerDead)
            {
                if (heal < 0)
                {
                    heal = 0;
                }
                else
                {
                    if (playerHP <= adjustedMaxHP)
                    {
                        DisplayTextHeal(heal);
                    }

                    playerHP += heal;

                    if (playerHP > adjustedMaxHP)
                    {
                        playerHP = adjustedMaxHP;
                    }

                    if (gm != null)
                    {
                        gm.AdjustHealthUIHeal(playerHP, adjustedMaxHP);

                        gm.PlayHealEffect();
                    }
                }
            }
        }
    }

    public void ApplyRegen(float regen)
    {
        if (!isRespawning)
        {
            if (!playerDead)
            {
                if (regen < 0)
                {
                    regen = 0;
                }
                else
                {
                    if (fastRecover)
                    {
                        regen *= fastRecoverMultiplier;
                    }

                    if (playerHP < adjustedMaxHP)
                    {
                        DisplayTextHeal(regen);
                    }

                    playerHP += regen;



                    if (playerHP > adjustedMaxHP)
                    {
                        playerHP = adjustedMaxHP;
                    }

                    if (gm != null)
                    {
                        gm.AdjustHealthUIHeal(playerHP, adjustedMaxHP);
                    }
                }
            }
        }
    }

    public void ApplyInstantDeath()
    {
        if (!isRespawning)
        {
            if (!playerDead)
            {
                float instaDamage = adjustedMaxHP * 2f;

                amountTaken = instaDamage;

                if (amountTaken < 0f)
                {
                    amountTaken = 0.25f;
                }

                isInvul = true;

                oldPlayerHP = playerHP;
                playerHP -= amountTaken;

                if (playerHP <= 0f)
                {
                    playerHP = 0f;

                    playerDead = true;
                }

                if (gm != null)
                {
                    gm.AdjustHealthUIDamage(playerHP, adjustedMaxHP);

                    if (playerDead)
                    {
                        if (!triggeredDeath)
                        {
                            triggeredDeath = true;

                            if (gm != null)
                            {
                                gm.ApplyDeath();
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.Log("player trying to die while dead");
            }
        }
        else
        {
            Debug.Log("player trying to die while while respawning");
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

}
