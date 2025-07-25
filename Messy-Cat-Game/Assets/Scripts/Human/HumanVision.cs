using System.Collections.Generic;
using UnityEngine;
using static Ticker;

public class HumanVision : Ticker
{
    [Header("Vision Arrays")]
    [SerializeField] public List<GameObject> viewableObjects;
    [SerializeField] private List<GameObject> objectsInViewZone;
    [Header("Settings")]
    [SerializeField] private LayerMask layerMask;
    [Header("Rerefences")]

    [Header("Gizmo Settings")]
    [Tooltip("The length of time the debug line appears for, set to 0 for the line to update in real time")]
    [SerializeField] private float lineTime;


    private GameObject player;

    RaycastHit hit;

    private void OnEnable()
    {
        OnTickAction += UpdateViewableObjects;
    }

    private void OnDisable()
    {
        OnTickAction -= UpdateViewableObjects;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
        {
            Debug.LogError("<color=red>Player not found</color> in the scene. Please ensure there is a GameObject with the tag<color=cyan> 'Player'</cyan>.", this);
            enabled = false;
        }
    }

    /// <summary>
    /// Adds the object to the list of objects in view zone when it enters the trigger collider.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (objectsInViewZone.Contains(other.gameObject))
        {
            return; // If the object is already in the view zone, do nothing
        }
        objectsInViewZone.Add(other.gameObject);
    }

    /// <summary>
    /// Update the list of objects in view zone and viewable when an object exits the trigger collider.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        objectsInViewZone.Remove(other.gameObject);
        if (viewableObjects.Contains(other.gameObject))
        {
            viewableObjects.Remove(other.gameObject);
        }
    }

    private new void Update()
    {
        //Tick system Update
        base.Update();

        //if (CanSee(dogContext.player.gameObject))
        //{

        //}
    }

    /// <summary>
    /// Returns true if the specified object is within the trigger collider connected to this script
    /// </summary>
    /// <param name="other">The game object to check if its visible</param>
    /// <returns></returns>
    public bool CanSee(GameObject other)
    {
        return viewableObjects.Contains(other);
    }

    /// <summary>
    /// Updates the list of viewable objects by shooting raycasts to each object in the view zone.
    /// </summary>
    private void UpdateViewableObjects()
    {
        foreach (GameObject other in objectsInViewZone)
        {
            if (other == null)
            {
                Debug.Log("Skipping null object", this);
                continue; // Skip null objects
            }
            //Shoot raycast
            //float distance = Vector3.Distance(parentTransform.position, other.transform.position) + 0.1f;
            //Vector3 direction = (other.transform.position - parentTransform.position).normalized;
            Vector3 origin = new(transform.position.x, transform.position.y + 0.1f, transform.position.z);

            //Debug.DrawRay(origin, direction * distance, Color.cyan);
            //If we fail to hit anything with our raycast, then we skip the rest of the code
            //if (!Physics.Raycast(origin, direction, out hit, distance, layerMask))
            //{
            //    //Debug.Log("Raycast failed to hit anything");
            //    continue;
            //}

            //Check if the raycast hit the object
            if (hit.collider.gameObject == other)
            {
                //Debug.Log("Object is visible " + hit.collider.gameObject.name);
                Debug.DrawLine(origin, other.transform.position, Color.green, lineTime, true);
                if (!viewableObjects.Contains(other))
                {
                    viewableObjects.Add(other);
                }
            }
            else
            {
                //Debug.Log("Object is not visible, instead we hit " + hit.collider.gameObject.name);
                Debug.DrawLine(origin, other.transform.position, Color.red, lineTime, true);
            }
        }
    }
}
