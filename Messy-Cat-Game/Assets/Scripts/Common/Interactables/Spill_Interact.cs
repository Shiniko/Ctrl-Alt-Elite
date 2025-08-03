using UnityEngine;
[RequireComponent(typeof(Collider))]
public class Spill_Interact : Interactable
{
    [SerializeField] private Animator messAnim;
    [SerializeField] private LevelManager levelManager;
    private MakeShiftCatController catController;
    private bool messTrigger;

    void Update()
    {
        if (levelManager == null)
        {
            if (GameObject.FindGameObjectWithTag("LevelManager") != null)
            {
                levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
            } else
            {
                Debug.Log("Can't find level manager in spill interact " + this);
            }
        }
    }

    public override void Interact()
    {
        base.Interact();

        if (!messTrigger)
        {
            messTrigger = true;

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
                GetComponent<MakeShiftCatController>().spillTarget = null;
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
            col.GetComponent<MakeShiftCatController>().spillTarget = this;
            catController = col.GetComponent<MakeShiftCatController>();
        }
    }

    public override void OnTriggerStay(Collider col)
    {
        base.OnTriggerStay(col);
        if (!interactedWith)
        {
            if (col.GetComponent<MakeShiftCatController>() != null)
            {
                if (col.GetComponent<MakeShiftCatController>().spillTarget == null)
                {
                    col.GetComponent<MakeShiftCatController>().spillTarget = this;
                    catController = col.GetComponent<MakeShiftCatController>();

                    Debug.Log("setting spill target again");
                }
            }
        }
    }

    public override void OnTriggerExit(Collider col)
    {
        base.OnTriggerExit(col);
        if (col.GetComponent<MakeShiftCatController>() != null)
        {
            col.GetComponent<MakeShiftCatController>().spillTarget = null;
        }
    }
}
