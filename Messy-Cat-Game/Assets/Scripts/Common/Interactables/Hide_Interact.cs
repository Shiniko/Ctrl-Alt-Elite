using UnityEngine;
[RequireComponent(typeof(Collider))]
public class Hide_Interact : MonoBehaviour, IInteractable
{
    private bool triggeredInteract;
    [SerializeField] private HideyHole hideHole;
    [SerializeField] private GameObject interactDisplay;

    public void Interact()
    {
        Debug.Log("Interacted with!");

        if (!triggeredInteract)
        {
            triggeredInteract = true;

            if (hideHole != null)
            {
                hideHole.EnterHole();
            }

            if (interactDisplay != null)
            {
                interactDisplay.SetActive(false);
            }
        }
    }

    public void ResetTrigger()
    {
        triggeredInteract = false;
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (interactDisplay != null)
            {
                if (!triggeredInteract)
                {
                    interactDisplay.SetActive(true);
                }
            }

            if(col.GetComponent<MakeShiftCatController>() != null)
            {
                col.GetComponent<MakeShiftCatController>().hideTarget = this;
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (interactDisplay != null)
            {
                interactDisplay.SetActive(false);              
            }

            if (triggeredInteract)
            {
                ResetTrigger();
            }

            if (col.GetComponent<MakeShiftCatController>() != null)
            {
                col.GetComponent<MakeShiftCatController>().hideTarget = null;
            }
        }
    }
}
