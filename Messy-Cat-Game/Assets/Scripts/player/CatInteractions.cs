using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerInput))]
public class CatInteractions : MonoBehaviour
{
    [Tooltip("The max distance the object can be away from the player but still interactable")]
    [SerializeField] private float interactionRange = 2.0f;
    [Tooltip("The layers that the cat will check for interactions with. If an object is outside these layers (even if it has an IIinteractable script on it) it will be ignored.")]
    [SerializeField] private LayerMask interactableLayers;

    //The max interactions allowed at any one time is equal to the size of this array
    private RaycastHit[] hits = new RaycastHit[1];

    //Im thinking this function can be called by an animation event however right now its being called by the PlayerInput system (in other words when you press E)
    public void TryToInteract()
    {
        Physics.SphereCastNonAlloc(transform.right,interactionRange,transform.forward,hits,0,interactableLayers,QueryTriggerInteraction.Collide);

        for (int i = 0; i < hits.Length; i++)
        {
            // Check if the hit object has an IInteractable component
            // TryGetComponent<>() doesn't allocate if a match is not found which is why its used here
            if (hits[i].transform.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interact();
                Debug.Log("Interacted with " + hits[i].transform.name,this);
            }
        }

        //Clears the hits array to ensure it doesn't retain old data
        System.Array.Clear(hits, 0, hits.Length);
    }
}
