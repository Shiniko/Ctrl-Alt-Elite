using UnityEngine;

public class Break_Interact : Interactable
{
    [SerializeField] private bool triggeredInteract;
    [SerializeField] private Animator messAnim;
    [SerializeField] private LevelManager levelManager;
    private MakeShiftCatController catController;

    void Update()
    {
        if (levelManager == null)
        {
            if (GameObject.FindGameObjectWithTag("LevelManager") != null)
            {
                levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
            }
        }
    }

    public override void Interact()
    {
        base.Interact();

        if (!triggeredInteract)
        {
            triggeredInteract = true;

            if (messAnim != null)
            {
                messAnim.SetBool("hasMessed", true);
            }

            if (levelManager != null)
            {
                levelManager.MakeAMess();
            }

            if (GetComponent<MakeShiftCatController>() != null)
            {
                GetComponent<MakeShiftCatController>().breakTarget = null;
            }
        }
        else
        {
            Debug.Log("Tried to interact, but player has already interacted with " + this);
        }
    }

    public override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);
        if (col.GetComponent<MakeShiftCatController>() != null)
        {
            col.GetComponent<MakeShiftCatController>().breakTarget = this;
            catController = col.GetComponent<MakeShiftCatController>();
        }
    }

    public override void OnTriggerStay(Collider col)
    {
        base.OnTriggerStay(col);
        if (col.GetComponent<MakeShiftCatController>() != null)
        {
            if (col.GetComponent<MakeShiftCatController>().breakTarget == null)
            {
                col.GetComponent<MakeShiftCatController>().breakTarget = this;
                catController = col.GetComponent<MakeShiftCatController>();
            }
        }
    }

    public override void OnTriggerExit(Collider col)
    {
        base.OnTriggerExit(col);
        if (col.GetComponent<MakeShiftCatController>() != null)
        {
            col.GetComponent<MakeShiftCatController>().breakTarget = null;
        }
    }
}
