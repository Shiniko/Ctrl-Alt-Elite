using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
// This script is used to track which objects are currently within the dog's field of vision.
// I recommend changing the layers it collides with to as few as possible in order free up computer resources
public class DogVision : MonoBehaviour
{
    public static List<GameObject> viewableObjects { get; private set; }
    private void OnTriggerEnter(Collider other)
    {
        viewableObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        viewableObjects.Remove(other.gameObject);
    }

    /// <summary>
    /// Returns true if the specified object is within the trigger collider connected to this script
    /// </summary>
    /// <param name="other">The game object to check if its visible</param>
    /// <returns></returns>
    public bool IsVisible(GameObject other)
    {
        return viewableObjects.Contains(other);
    }
}

