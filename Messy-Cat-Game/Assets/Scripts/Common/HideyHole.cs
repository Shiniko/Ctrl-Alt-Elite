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
        }
    }

    public void ExitHole()
    {
        if(hint != null)
        {
            hint.ResetTrigger();
        }
    }
}
