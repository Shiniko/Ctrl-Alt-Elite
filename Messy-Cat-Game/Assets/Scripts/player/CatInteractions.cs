using UnityEngine;
using UnityEngine.InputSystem;
//The player input behavior should be set to "Invoke Unity Events"
//Find the interact event Events/Player/Interact and set it to call the TryToInteract function
[RequireComponent(typeof(PlayerInput))]
public class CatInteractions : MonoBehaviour
{
    [Tooltip("The max distance the object can be away from the player but still interactable")]
    [SerializeField] private float interactionRange = 2.0f;
    [SerializeField] private float sphereCastSize = 1;
    [Tooltip("Change so the way the cat is facing matches with one of the axis. TransformRight : Red axis of the transform, TransformUp : Green axis of the transform, TransformForward : Blue Axis of the transform")]
    [SerializeField] private FacingDirection facingDirection;
    [Tooltip("The layers that the cat will check for interactions with. If an object is outside these layers (even if it has an IIinteractable script on it) it will be ignored.")]
    [SerializeField] private LayerMask interactableLayers;
    [Tooltip("Controls whether the player should try interacting with trigger colliders or not(Change to ignore if not needed)")]
    [SerializeField] private QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Collide;
    //The max interactions allowed at any one time is equal to the size of this array
    private RaycastHit[] hits = new RaycastHit[1];

    //Im thinking this function can be called by an animation event however right now its being called by the PlayerInput system (in other words when you press E)
    public void TryToInteract()
    {
        Debug.Log("Trying to interact...", this);
        Vector3 direction;

        switch (facingDirection) 
        {
            case FacingDirection.TransformRight:
                direction = transform.right;
                break;
            case FacingDirection.TransformUp:
                direction = transform.up;
                break;
            case FacingDirection.TransformForward:
                direction = transform.forward;
                break;
            default:
                direction = transform.right; // Default to forward if no valid direction is set
                Debug.LogWarning("Invalid FacingDirection set, defaulting to TransformRight.");
                break;
        }

        // Draw the cast direction
        Debug.DrawLine(transform.position, transform.position + direction * interactionRange, Color.cyan, 1f);

        int hitCount = Physics.SphereCastNonAlloc(transform.position, sphereCastSize, direction, hits, interactionRange, interactableLayers, triggerInteraction);
        //If we didnt hit anything, we can return early
        if (hitCount <= 0)
        {
            return;
        }

        for (int i = 0; i < hits.Length; i++)
        {
            Debug.Log("Hit: " + hits[i].transform.name, hits[i].transform);
            // Check if the hit object has an IInteractable component
            // TryGetComponent<>() doesn't allocate if a match is not found which is why its used here
            if (hits[i].transform.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interact();
                Debug.Log("Interacted with: " + hits[i].transform.name, hits[i].transform);
            }
        }

        // Clears the hits array to ensure it doesn't retain old data
        System.Array.Clear(hits, 0, hits.Length);
    }

    enum FacingDirection 
    {
        TransformRight, TransformUp, TransformForward
    }

}
