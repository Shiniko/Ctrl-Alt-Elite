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

        if (catController != null)
        {
            catController.ThirtyThreeScratch();
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
