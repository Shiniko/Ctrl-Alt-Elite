using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimDealer : MonoBehaviour
{
    public AnimHandler anim;

    [SerializeField] private bool startLand;

    public void DoneRespawning()
    {
        if (anim != null)
        {
            anim.FinishRespawning();
        }
    }

    public void DoneLanding()
    {
        startLand = false;

        if (anim != null)
        {
            anim.FinishLanding();
        }
    }

    public void DoneJumping()
    {
        if (anim != null)
        {
            anim.FinishJumping();
        }
    }

    public void StartLanding()
    {
        if(startLand)
        {
            Debug.Log("landing animation was started before last one ended");
        }

        if (anim != null)
        {
            anim.Land();
        }

        startLand = true;
    }

    public void DoneHurting()
    {
        if (anim != null)
        {
            anim.EndHurt();
        }
    }

    public void DoneAttacking()
    {
        if(anim != null)
        {
            anim.FinishAttack();
        }
    }

    public void SummonAttackOne()
    {
        if (anim != null)
        {
            anim.SummonAttackOne();
        }
    }

    public void SummonAttackOneSecond()
    {
        if (anim != null)
        {
            anim.SummonAttackOneSecond();
        }
    }

    public void SummonAttackTwo()
    {
        if (anim != null)
        {
            anim.SummonAttackTwo();
        }
    }

    public void SummonAttackTwoSecond()
    {
        if (anim != null)
        {
            anim.SummonAttackTwoSecond();
        }
    }

    public void SummonAttackThree()
    {
        if (anim != null)
        {
            anim.SummonAttackThree();
        }
    }

    public void SummonAttackThreeSecond()
    {
        if (anim != null)
        {
            anim.SummonAttackThreeSecond();
        }
    }

    public void SummonAttackFour()
    {
        if (anim != null)
        {
            anim.SummonAttackFour();
        }
    }

    public void SummonAttackFourSecond()
    {
        if (anim != null)
        {
            anim.SummonAttackFourSecond();
        }
    }

    public void StartCharging()
    {
        if(anim != null)
        {
            anim.StartCharging();
        }
    }

    public void EndCharging()
    {
        if (anim != null)
        {
            anim.EndCharging();
        }
    }
}
