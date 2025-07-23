using UnityEngine;
public class Hide_Interact : Interactable
{
    private bool triggeredInteract;
    private MakeShiftCatController catController;

    public void ResetTrigger()
    {
        triggeredInteract = false;

        if (interactDisplay != null)
        {
            interactDisplay.SetActive(true);
        }
    }

    public override void Interact()
    {
        base.Interact();
        catController.TryToHide();
    }

    public override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);
        if (col.GetComponent<MakeShiftCatController>() != null)
        {
            col.GetComponent<MakeShiftCatController>().hideTarget = this;
            catController = col.GetComponent<MakeShiftCatController>();
        }
    }

    public override void OnTriggerStay(Collider col)
    {
        base.OnTriggerStay(col);
        if (col.GetComponent<MakeShiftCatController>() != null)
        {
            if(col.GetComponent<MakeShiftCatController>().hideTarget == null)
            {
                col.GetComponent<MakeShiftCatController>().hideTarget = this;
                catController = col.GetComponent<MakeShiftCatController>();
            }
        }
    }

    public override void OnTriggerExit(Collider col)
    {
        base.OnTriggerExit(col);
        if (col.GetComponent<MakeShiftCatController>() != null)
        {
            col.GetComponent<MakeShiftCatController>().hideTarget = null;
        }
    }
}
