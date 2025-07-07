using UnityEngine;
[RequireComponent(typeof(Collider))]
public class BoxInteractable : MonoBehaviour, IInteractable
{
    void Interact()
    {
        Debug.Log("Interacted with!");
    }
}
