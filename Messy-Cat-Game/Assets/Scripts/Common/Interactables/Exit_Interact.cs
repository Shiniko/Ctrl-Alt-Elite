using UnityEngine;

public class Exit_Interact : MonoBehaviour
{
    [SerializeField] private bool triggeredInteract;
    [SerializeField] private bool playerInRange;
    [SerializeField] private Animator exitAnim;
    [SerializeField] private DoorScript.Door door;
    [SerializeField] private GameObject interactDisplay;
    [SerializeField] private LevelManager levelManager;

    public bool openOnReveal;

    void Awake()
    {
        if (openOnReveal)
        {
            if(door != null)
            {
                if (door != null)
                {
                    door.OpenDoor();
                }
            }
        }
    }

    void Update()
    {
        if (levelManager == null)
        {
            if (LevelManager.instance != null)
            {
                levelManager = LevelManager.instance;
            }
        }
    }

    public void Interact()
    {
        if (playerInRange)
        {
            if (!triggeredInteract)
            {
                triggeredInteract = true;

                if (exitAnim != null)
                {
                    exitAnim.SetBool("revealExit", true);
                }

                if (interactDisplay != null)
                {
                    interactDisplay.SetActive(false);
                }

                if (levelManager != null)
                {
                    levelManager.LevelVictory();
                }
            }
        }
        else
        {
            Debug.Log("Tried to interact, but player not in range");
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerInRange = true;
        }

        if (door != null)
        {
            if (!door.open)
            {
                door.OpenDoor();
            }
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
                col.GetComponent<MakeShiftCatController>().exitTarget = this;
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

            if (door != null)
            {
                if (door.open)
                {
                    door.OpenDoor();
                }
            }

            if (col.GetComponent<MakeShiftCatController>() != null)
            {
                col.GetComponent<MakeShiftCatController>().exitTarget = null;
            }
        }
    }
}
