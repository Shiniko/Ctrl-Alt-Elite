using UnityEngine;

//Change the layer overrides so only the player can collide with it.
[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    /// <summary>
    /// The button prompt to enable when the player is within range of the object
    /// </summary>
    [SerializeField] protected GameObject interactDisplay;
    /// <summary>
    /// returns true if the player is staying inside the trigger collider, and false otherwise
    /// </summary>
    [SerializeField] protected bool playerInRange;

    /// <summary>
    /// If true when this script's interact function gets called for the first time it'll deactive the script after the function completes. By default it is true.
    /// </summary>
    /// <seealso cref="Interact"> </seealso>
    [SerializeField] protected bool oneUse;

    protected bool interactedWith;
    public virtual void Awake()
    {
        if(TryGetComponent<Collider>(out Collider col))
        {
            if(col.isTrigger == false)
            {
                Debug.LogWarning("The collider was not set to <color=yellow> trigger! </color> Setting it to trigger...",this);
                col.isTrigger = true;
            }
        }
        oneUse = true;
    }

    /// <summary>
    /// Gets called when the player presses the interact key(specified in CatInteractions) while in a trigger collider attached to this object.
    /// </summary>
    public virtual void Interact()
    {
        if (oneUse && interactedWith)
        {
            return;
        }
        interactedWith = true;
        Debug.Log("Interacted with!",this);
        if (interactDisplay == null)
        {
            return;
        }
        interactDisplay.SetActive(false);
        if (oneUse)
        {
            Debug.Log("<color=yellow>Disabling interaction object</color> because <color=green>oneUse</color> is set to<color=aqua> true</color>,if this was a mistake please set the variable to false.", this);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>This method checks if the specified <paramref name="col"/> has the "Player" tag and, if so,
    /// sets the interactDisplay to active</remarks>
    /// <param name="col"></param>
    public virtual void OnTriggerEnter(Collider col)
    {
        if (oneUse && interactedWith)
        {
            return;
        }
        if (!col.CompareTag("Player"))
        {
            return;
        }
        playerInRange = true;
        if (interactDisplay == null)
        {
            return;
        }
        interactDisplay.SetActive(true);
        Debug.Log("Activating interactDisplay : " + interactDisplay.name,this);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>This method checks if the specified <paramref name="col"/> has the "Player" tag and, if so,
    /// listens for the interaction key press to invoke the interaction logic.</remarks>
    /// <param name="col">The <see cref="Collider"/> that is staying within the trigger. Must have the tag "Player" to trigger interaction
    /// logic.</param>
    public virtual void OnTriggerStay(Collider col)
    {
        if (oneUse && interactedWith)
        {
            return;
        }
        if (!col.CompareTag("Player"))
        {
            return;
        }

        if (CatInteractions.Instance == null)
        {
            Debug.LogError("No instance of the <color=lime> CatInteractions </color> script is present in the scene!! Interactions will be <color=red>disabled...</color>", this);
            this.enabled = false;
            return;
        }

        if (Input.GetKeyDown(CatInteractions.Instance.GetInteractKey()))
        {
            Interact();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>This method checks if the specified <paramref name="col"/> has the "Player" tag and, if so,
    /// sets the interactDisplay to innactive</remarks>
    /// <param name="col"></param>
    public virtual void OnTriggerExit(Collider col)
    {
        if (oneUse && interactedWith)
        {
            return;
        }
        if (!col.CompareTag("Player"))
        {
            return;
        }
        playerInRange = false;
        if (interactDisplay == null)
        {
            return;
        }
        interactDisplay.SetActive(false);
        Debug.Log("Deactivating interactDisplay : " + interactDisplay.name, this);
    }
}
