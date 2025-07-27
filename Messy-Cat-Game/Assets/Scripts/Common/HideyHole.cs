using UnityEngine;

public class HideyHole : MonoBehaviour
{
    [SerializeField] private Hide_Interact hint;

    private bool triggerHide;

    public void EnterHole()
    {
        if (!triggerHide)
        {
            triggerHide = true;

            //place your effects here if any

        }
    }

    public void ExitHole()
    {
        // Just in case you want exiting effect
    }
}
