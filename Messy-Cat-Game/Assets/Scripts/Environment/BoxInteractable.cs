using UnityEngine;
[RequireComponent(typeof(Collider))]
public class BoxInteractable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Interacted with!!!");
    }
}
