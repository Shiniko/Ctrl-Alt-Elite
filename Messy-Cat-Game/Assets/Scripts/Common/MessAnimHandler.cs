using UnityEngine;

public class MessAnimHandler : MonoBehaviour
{
    [SerializeField] private GameObject interact;

    public void TurnOffInteract()
    {
        if (interact != null)
        {
            interact.SetActive(false);
        }
    }
}
