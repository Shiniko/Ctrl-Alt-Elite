using UnityEngine;

public class Scratch_Interact : Interactable
{
    private bool triggeredInteract;
    private MakeShiftCatController catController;
    [SerializeField] private GameObject interactDisplay;


    public void ResetTrigger()
    {
        triggeredInteract = false;

        if(interactDisplay != null)
        {
            interactDisplay.SetActive(true);
        }
    }

    public override void Interact()
    {
        base.Interact();
        catController.TryToScratch();
    }

    public override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);
        if (col.GetComponent<MakeShiftCatController>() != null)
        {
            col.GetComponent<MakeShiftCatController>().scratchTarget = this;
            catController = col.GetComponent<MakeShiftCatController>();
        }
    }

    public override void OnTriggerStay(Collider col)
    {
        base.OnTriggerStay(col);
        if (col.GetComponent<MakeShiftCatController>() != null)
        {
            if (col.GetComponent<MakeShiftCatController>().scratchTarget == null)
            {
                col.GetComponent<MakeShiftCatController>().scratchTarget = this;
                catController = col.GetComponent<MakeShiftCatController>();
            }
        }
    }

    public override void OnTriggerExit(Collider col)
    {
        base.OnTriggerExit(col);
        if (col.GetComponent<MakeShiftCatController>() != null)
        {
            col.GetComponent<MakeShiftCatController>().scratchTarget = null;
        }
    }
}
