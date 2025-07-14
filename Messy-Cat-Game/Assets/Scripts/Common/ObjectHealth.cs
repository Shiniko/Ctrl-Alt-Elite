using UnityEngine;

public class ObjectHealth : MonoBehaviour
{
    public float adjustedMaxHealth;
    public float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private float bonusMaxHealth;

    private bool setInitialHealth;

    void Update()
    {
        if(bonusMaxHealth > 0f)
        {
            if (setInitialHealth)
            {
                adjustedMaxHealth += bonusMaxHealth;

                if (currentHealth < adjustedMaxHealth)
                {
                    currentHealth += bonusMaxHealth;

                    ApplyHeal(bonusMaxHealth);

                    Debug.Log("Added bonus max health of " + bonusMaxHealth);
                }

                bonusMaxHealth = 0f;
            }
        }

        if(currentHealth > adjustedMaxHealth)
        {
            currentHealth = adjustedMaxHealth;
        }

        if (!setInitialHealth)
        {
            SetInitialHealth();
        }
    }

    private void SetInitialHealth()
    {
        adjustedMaxHealth = maxHealth;
        currentHealth = maxHealth;

        Debug.Log("set initial health to " + maxHealth);

        setInitialHealth = true;

        ApplyHeal(maxHealth);
    }

    public void GiveBonusMaxHealth(float bonus)
    {
        bonusMaxHealth = bonus;
    }

    public void ApplyDamage(float amount)
    {
        currentHealth -= amount;

        if(currentHealth <= 0f)
        {
            ApplyObjectDeath();
        }

        //if using bars apply that here
    }

    private void ApplyObjectDeath()
    {
        //to do: apply mess count and stuff
    }

    private void ApplyHeal(float amount)
    {
        //if using bars adjusted them here
    }
}
