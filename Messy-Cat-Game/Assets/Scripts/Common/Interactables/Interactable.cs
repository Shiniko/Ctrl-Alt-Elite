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
                Debug.LogWarning("The collider was not set to trigger! Setting it to trigger.",this);
                col.isTrigger = true;
            }
        }
    }



    /// <summary>
    /// Gets called when the player presses the interact key(specified in CatInteractions) while in range of this interactable.
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

    public virtual void OnTriggerStay(Collider col)
    {
        if (!col.CompareTag("Player"))
        {
            return;
        }

        if (Input.GetKeyDown(CatInteractions.Instance.GetInteractKey()))
        {
            Interact();
        }
    }

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
