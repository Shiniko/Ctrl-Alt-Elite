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
}
