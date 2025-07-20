using UnityEngine;
[RequireComponent(typeof(Collider))]
public class Hide_Interact : MonoBehaviour, IInteractable
{
    private bool triggeredInteract;
    private bool playerInRange;
    [SerializeField] private HideyHole hideHole;
    [SerializeField] private GameObject interactDisplay;

    public void Interact()
    {
        if (playerInRange)
        {
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
        else
        {
            Debug.Log("Tried to interact, but player not in range");
        }
    }

    public void ResetTrigger()
    {
        triggeredInteract = false;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerInRange = true;

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
            playerInRange = false;

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
