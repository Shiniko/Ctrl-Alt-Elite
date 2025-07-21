using UnityEngine;
[RequireComponent(typeof(Collider))]
public class Spill_Interact : MonoBehaviour
{
    [SerializeField] private bool triggeredInteract;
    [SerializeField] private bool playerInRange;
    [SerializeField] private Animator messAnim;
    [SerializeField] private GameObject interactDisplay;
    [SerializeField] private LevelManager levelManager;

    public void Interact()
    {
        if (playerInRange)
        {
            if (!triggeredInteract)
            {
                triggeredInteract = true;

                if (messAnim != null)
                {
                    messAnim.SetBool("hasMessed", true);
                }

                if (interactDisplay != null)
                {
                    interactDisplay.SetActive(false);
                }

                if (levelManager != null)
                {
                    levelManager.MakeAMess();
                }
            }
        }
        else
        {
            Debug.Log("Tried to interact, but player not in range");
        }
    }

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

            if (col.GetComponent<MakeShiftCatController>() != null)
            {
                col.GetComponent<MakeShiftCatController>().spillTarget = this;
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

            if (col.GetComponent<MakeShiftCatController>() != null)
            {
                col.GetComponent<MakeShiftCatController>().spillTarget = null;
            }
        }
    }
}
