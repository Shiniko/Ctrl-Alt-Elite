using UnityEngine;

public class PulleySystem : MonoBehaviour
{
    [SerializeField] private GameObject thingToRemove;
    [SerializeField] private GameObject pulleyToRemove;
    [SerializeField] private GameObject removeEffect;

    public void RemoveTheThing()
    {
        if (thingToRemove != null)
        {
            Destroy(thingToRemove);
        }

        if (removeEffect != null)
        {
            Instantiate(removeEffect, thingToRemove.transform.position, Quaternion.identity);
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
