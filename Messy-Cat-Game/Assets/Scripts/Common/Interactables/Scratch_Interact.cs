using UnityEngine;

public class Scratch_Interact : Interactable
{
    private bool triggeredInteract;
    [SerializeField] private Animator messAnim;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameObject specialEffect;
    public ObjectHealth objectHealth;               //cat controller will access this to do damage
    private MakeShiftCatController catController;

    public bool triggerHurt;        //cat controller will call this for the mperiod to react
    public bool isDead;

    public bool isForward;

    void Update()
    {
        if (levelManager == null)
        {
            if (LevelManager.instance != null)
            {
                levelManager = LevelManager.instance;
            }
        }

        if(triggerHurt)
        {
            if (!isDead)
            {
                triggerHurt = false;

                if (messAnim != null)
                {
                    messAnim.SetTrigger("isHurt");
                }
            }
        }

        if (isDead)
        {
            if (interactDisplay != null)
            {
                interactDisplay.SetActive(false);
            }

            triggeredInteract = true;
        }
    }

    public void ResetTrigger()
    {
        triggeredInteract = false;

        triggerHurt = false;

        if (!isDead)
        {
            if (interactDisplay != null)
            {
                interactDisplay.SetActive(true);
            }
        }
    }

    public void ScratchEffect()
    {
        if (specialEffect != null)
        {
            Instantiate(specialEffect, transform.position, Quaternion.identity);
        }
    }

    public void FinishedScratching()  // called by object health to finish the object
    {
        isDead = true;

        if (interactDisplay != null)
        {
            interactDisplay.SetActive(false);
        }

        if (messAnim != null)
        {
            messAnim.SetBool("isDead", true);
            messAnim.SetTrigger("triggerDead");
        }

        if (levelManager != null)
        {
            levelManager.MakeAMess();
        }
    }

    public override void Interact()
    {
        base.Interact();

        if (!isDead)
        {
            if(catController != null)
            {
                catController.TryToScratch();
            }

            if (messAnim != null)
            {
                messAnim.SetBool("hasMessed", true);
            }
        } 
    }

    public override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);
        if (col.GetComponent<MakeShiftCatController>() != null)
        {
            col.GetComponent<MakeShiftCatController>().scratchTarget = this;
            catController = col.GetComponent<MakeShiftCatController>();
        }

        if (isDead)
        {
            if (interactDisplay != null)
            {
                interactDisplay.SetActive(false);
            }
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

        if (isDead)
        {
            if (interactDisplay != null)
            {
                interactDisplay.SetActive(false);
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
