using UnityEngine;
[RequireComponent(typeof(Collider))]
public class DoorInteract : IInteractable
{
    private bool triggeredInteract;
    [SerializeField] private ExitRoom exitRoom;

    public override void Interact()
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
