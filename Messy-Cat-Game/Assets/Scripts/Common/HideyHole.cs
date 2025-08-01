using UnityEngine;

public class HideyHole : MonoBehaviour
{
    [SerializeField] private Hide_Interact hint;
    public Transform hidePosition;

    private bool triggerHide;

    [SerializeField] private GameObject enterEffect;
    [SerializeField] private GameObject exitEffect;

    public void EnterHole()
    {
        if (!triggerHide)
        {
            triggerHide = true;

            //place your effects here if any

            if (enterEffect != null)
            {
                Instantiate(enterEffect, transform.position, Quaternion.identity);
            }
        }
    }

    public void ExitHole()
    {
        triggerHide = false;
        // Just in case you want exiting effect

        if(exitEffect != null )
        {
            Instantiate(exitEffect, transform.position, Quaternion.identity);
        }

    }
}
