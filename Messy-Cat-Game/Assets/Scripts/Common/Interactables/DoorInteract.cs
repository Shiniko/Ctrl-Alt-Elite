using UnityEngine;
[RequireComponent(typeof(Collider))]
public class DoorInteract : MonoBehaviour, IInteractable
{
    private bool triggeredInteract;
    [SerializeField] private ExitRoom exitRoom;

    void Interact()
    {
        Debug.Log("Interacted with!");

        if (!triggeredInteract)
        {
            triggeredInteract = true;

            if(exitRoom != null)
            {
                exitRoom.EnterExit();
            }
        }
    }
}
