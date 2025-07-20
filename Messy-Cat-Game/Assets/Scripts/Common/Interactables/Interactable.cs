using UnityEngine;

//Change the layeroverrides so only the player can collide with it.
[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    [SerializeField] protected GameObject interactDisplay;
    public virtual void Awake()
    {
        if(TryGetComponent<Collider>(out Collider col))
        {
            if(col.isTrigger == false)
            {
                Debug.LogWarning("The collider was not set to <color=yellow> trigger! </color> Setting it to trigger.",this);
                col.isTrigger = true;
            }
        }
    }

    /// <summary>
    /// Gets called when the player presses the interact key(specified in CatInteractions) while in a trigger collider attached to this object.
    /// </summary>
    public virtual void Interact()
    {
        Debug.Log("Interacted with!",this);
        if (interactDisplay == null)
        {
            return;
        }
        interactDisplay.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>This method checks if the specified <paramref name="col"/> has the "Player" tag and, if so,
    /// sets the interactDisplay to active</remarks>
    /// <param name="col"></param>
    public virtual void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player"))
        {
            return;
        }
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
        if (!col.CompareTag("Player"))
        {
            return;
        }

        if (CatInteractions.Instance == null)
        {
            Debug.LogError("No instance of the <color=lime> CatInteractions </color> script is present in the scene!! Interactions will be <color=red>disabled...</color>", this);
            this.enabled = false;
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
        if (!col.CompareTag("Player"))
        {
            return;
        }
        if (interactDisplay == null)
        {
            return;
        }
        interactDisplay.SetActive(false);
        Debug.Log("Deactivating interactDisplay : " + interactDisplay.name, this);
    }
}
