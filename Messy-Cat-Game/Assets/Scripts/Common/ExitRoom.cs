using UnityEngine;

public class ExitRoom : MonoBehaviour
{
    public GameObject interactItem;
    private bool triggerOpen;

    [SerializeField] private GameManager gm;

    public void ActivateExit()
    {
        if (interactItem != null)
        {
            if (!triggerOpen)
            {
                triggerOpen = true;

                interactItem.SetActive(true);
            }
        }
    }

    public void EnterExit()
    {
        if (gm != null)
        {
            if (triggerOpen)
            {
                //to do have Game manager display win con
            }
        }
    }
}
