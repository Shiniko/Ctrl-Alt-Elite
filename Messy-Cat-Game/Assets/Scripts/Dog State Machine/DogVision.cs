using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
// This script is used to track which objects are currently within the dog's field of vision.
// I recommend changing the layers it collides with to as few as possible in order free up computer resources
//Should be on a child object of the dog
public class DogVision : Ticker
{
    [SerializeField] public List<GameObject> viewableObjects;
    [SerializeField] private List<GameObject> objectsInViewZone;
    [SerializeField] private LayerMask layerMask;
    RaycastHit hit;

    [Header("Gizmo Settings")]
    [Tooltip("The length of time the debug line appears for, set to 0 for the line to update in real time")]
    [SerializeField] private float lineTime;
    private void OnEnable()
    {
        OnTickAction += UpdateViewableObjects;
    }

    private void OnDisable()
    {
        OnTickAction -= UpdateViewableObjects;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (objectsInViewZone.Contains(other.gameObject))
        {
            return; // If the object is already in the view zone, do nothing
        }
        objectsInViewZone.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        objectsInViewZone.Remove(other.gameObject);
        if (viewableObjects.Contains(other.gameObject))
        {
            viewableObjects.Remove(other.gameObject);
        }
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


    private void UpdateViewableObjects()
    {
        foreach (GameObject other in objectsInViewZone)
        {
            if(other == null)
            {
                continue; // Skip null objects
            }
            //Shoot raycast
            float distance = Vector3.Distance(transform.position, other.transform.position) + 0.1f;
            Vector3 direction = (other.transform.position - transform.position).normalized;
            //If we failt to hit anything with our raycast, then we skip the rest of the code
            if(!Physics.Raycast(transform.position, direction, out hit, distance, layerMask))
            {
                continue;
            }

            //Check if the raycast hit the object
            if (hit.collider.gameObject == other.gameObject)
            {
                Debug.DrawLine(transform.position, other.transform.position, Color.green, lineTime,true);
                if (!viewableObjects.Contains(other))
                {
                    viewableObjects.Add(other.gameObject);
                }
            }
            else
            {
                Debug.DrawLine(transform.position, other.transform.position, Color.red, lineTime,true);
            }
        }

    }
}

