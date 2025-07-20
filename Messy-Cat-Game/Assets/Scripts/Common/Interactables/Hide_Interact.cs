using UnityEngine;
[RequireComponent(typeof(Collider))]
public class Hide_Interact : Interactable
{
    private bool triggeredInteract;
    private MakeShiftCatController catController;

    public override void Interact()
    {
        catController.StartHiding();
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

    public override void OnTriggerExit(Collider col)
    {
        base.OnTriggerExit(col);
        if (col.GetComponent<MakeShiftCatController>() != null)
        {
            col.GetComponent<MakeShiftCatController>().hideTarget = null;
        }
    }
}
