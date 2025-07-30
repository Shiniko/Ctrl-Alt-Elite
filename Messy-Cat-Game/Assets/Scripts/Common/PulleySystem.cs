using UnityEngine;

public class PulleySystem : MonoBehaviour
{
    [SerializeField] private GameObject thingToRemove;
    [SerializeField] private GameObject pulleyToRemove;

    public void RemoveTheThing()
    {
        if (thingToRemove != null)
        {
            Destroy(thingToRemove);
        }
    }

    public void RemovePulley()
    {
        if (pulleyToRemove != null)
        {
            Destroy(pulleyToRemove);
        }

        RemoveTheThing();
    }
}
