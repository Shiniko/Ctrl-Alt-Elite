using UnityEngine;

public class Cat_Anim_Dealer : MonoBehaviour
{
    [SerializeField] private MakeShiftCatController catController;

    public void EndVictory()
    {
        if (catController != null)
        {
            catController.EndVictory();
        }
    }

    public void ThirtyThreeScratch()
    {
        Debug.Log("calling 33 percent from anim handler");

        if (catController != null)
        {
            Debug.Log("cat controller wasnt null so calling 33 from anim dealer to cat controller");
            catController.ThirtyThreeScratch();

            Debug.Log("AFTER calling 33 from anim dealer to cat controller");
        }
        else
        {
            Debug.Log("cat controller in null");
        }
    }

    public void SixtySixScratch()
    {
        if (catController != null)
        {
            catController.SixtySixScratch();
        }
    }

    public void FullScratch()
    {
        if (catController != null)
        {
            catController.FullScratch();
        }
    }

    public void DoneHiding()
    {
        if (catController != null)
        {
            catController.FinishedHiding();
        }
    }

    public void DonePreppingScratch()
    {
        if (catController != null)
        {
            catController.PreparedScratching();
        }
    }

    public void DoneLanding()
    {
        if (catController != null)
        {
            catController.DoneLanding();
        }
    }

    public void LandedHard()
    {
        if (catController != null)
        {
            catController.LandedHard();
        }
    }

    public void JumpAscended()
    {
        if (catController != null)
        {
            catController.JumpAscended();
        }
    }
}
