using UnityEngine;

public class ObjectHealth : MonoBehaviour
{
    public float adjustedMaxHealth;
    public float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private float bonusMaxHealth;

    void Update()
    {
        if(bonusMaxHealth > 0f)
        {
            adjustedMaxHealth = maxHealth + bonusMaxHealth;

            if(currentHealth < adjustedMaxHealth)
            {
                currentHealth += adjustedMaxHealth;
            }

            bonusMaxHealth = 0f;
        }

        if(currentHealth > adjustedMaxHealth)
        {
            currentHealth = adjustedMaxHealth;
        }
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
