using UnityEngine;
[RequireComponent(typeof(Collider))]
public class Hide_Interact : MonoBehaviour, IInteractable
{
    private bool triggeredInteract;
    [SerializeField] private HideyHole hideHole;

    void Interact()
    {
        Debug.Log("Interacted with!");

        if (!triggeredInteract)
        {
            triggeredInteract = true;

            if (hideHole != null)
            {
                hideHole.EnterHole();
            }
        }
    }

    public void ResetTrigger()
    {
        triggeredInteract = false;
    }
}
