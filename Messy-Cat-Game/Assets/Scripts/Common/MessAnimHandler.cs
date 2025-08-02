using UnityEngine;

public class MessAnimHandler : MonoBehaviour
{
    [SerializeField] private GameObject interact;
    [SerializeField] private GameObject interactDisplay;

    public void TurnOffInteract()
    {
        if (interact != null)
        {
            interact.SetActive(false);
        }

        if(interactDisplay != null)
        {
            interactDisplay.SetActive(false);

            Debug.Log("set interact disaplay to false for " + this);
        }
    }
}
