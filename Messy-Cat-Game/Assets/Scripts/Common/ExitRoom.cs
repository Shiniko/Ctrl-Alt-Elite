using UnityEngine;

public class ExitRoom : MonoBehaviour
{
    public GameObject interactItem;
    private bool triggerOpen;
    private bool hasCheckedExitInteractItem;

    [SerializeField] private GameManager gm;
    [SerializeField] private LevelManager levelManager;

    private void Start()
    {
        levelManager = LevelManager.instance;
    }

    void Update()
    {
        if (interactItem != null)
        {
            if (levelManager != null)
            {
                levelManager.exitPortal = interactItem;
            }
        }
        else
        {
            if (!hasCheckedExitInteractItem)
            {
                if (gameObject.GetComponentInChildren<Exit_Interact>() != null)
                {
                    interactItem = gameObject.GetComponentInChildren<Exit_Interact>().gameObject;
                }

                hasCheckedExitInteractItem = true;
            }
        }
    }

    public void ActivateExit()
    {
        if (interactItem != null)
        {
            if (!triggerOpen)
            {
                triggerOpen = true;

                interactItem.SetActive(true);
            }
        }
    }

    public void EnterExit()
    {
        if (gm != null)
        {
            if (triggerOpen)
            {
                //to do have Game manager display win con
            }
        }
    }
}
