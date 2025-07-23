using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
// This script is used to track which objects are currently within the dog's field of vision.
// I recommend changing the layers it collides with to as few as possible in order free up computer resources
//Should be on a child object of the dog
public class DogVision : Ticker
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

    private Transform parentTransform;
    private Animator animator;
    private DogContext dogContext;


    private bool suspicious;
    private bool chasing;
    RaycastHit hit;

    //String Hashes
    private int _suspiciousBool = Animator.StringToHash("Suspicious");
    private int _chasingBool = Animator.StringToHash("Chasing");

    private void OnEnable()
    {
        OnTickAction += UpdateViewableObjects;
    }

    private void OnDisable()
    {
        OnTickAction -= UpdateViewableObjects;
    }

    private void Awake()
    {
        parentTransform = GetComponentInParent<Transform>();
        animator = GetComponentInParent<Animator>();
        dogContext = GetComponentInParent<DogContext>();

        SetUpCheck();
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

        suspicious = animator.GetBool(_suspiciousBool);
        chasing = animator.GetBool(_chasingBool);

        if (CanSee(dogContext.player))
        {
            if (!suspicious && !chasing)
            {
                dogContext.currentSuspiciousEvent = new SuspiciousEvent(dogContext.transform.position);
                animator.SetBool(_suspiciousBool, true);
            }
        }
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
            if(other == null)
            {
                Debug.Log("Skipping null object", this);
                continue; // Skip null objects
            }
            //Shoot raycast
            float distance = Vector3.Distance(parentTransform.position, other.transform.position) + 0.1f;
            Vector3 direction = (other.transform.position - parentTransform.position).normalized;
            Vector3 origin = new(transform.position.x,transform.position.y + 0.1f,transform.position.z);
            //If we fail to hit anything with our raycast, then we skip the rest of the code
            if (!Physics.Raycast(origin, direction, out hit, distance, layerMask))
            {
                //Debug.Log("Raycast failed to hit anything");
                continue;
            }

            //Check if the raycast hit the object
            if (hit.collider.gameObject == other)
            {
                //Debug.Log("Object is visible " + hit.collider.gameObject.name);
                Debug.DrawLine(origin, other.transform.position, Color.green, lineTime,true);
                if (!viewableObjects.Contains(other))
                {
                    viewableObjects.Add(other);
                }
            }
            else
            {
                //Debug.Log("Object is not visible, instead we hit " + hit.collider.gameObject.name);
                Debug.DrawLine(origin, other.transform.position, Color.red, lineTime,true);
            }
        }
    }

    /// <summary>
    /// Checks if the script has been set up correctly.
    /// </summary>
    private void SetUpCheck()
    {
        int dogLayer = LayerMask.NameToLayer("Dog");
        if (dogLayer == -1)
        {
            Debug.LogError("'Dog' layer does not exist! Please create a layer named 'Dog' in the Unity editor.", this);
            return;
        }
        if (!(gameObject.layer == dogLayer))
        {
            Debug.LogWarning("This object (and all other children and parents) needs to be on the dog layer! Automatically setting it for this object...", this);
            gameObject.layer = dogLayer;
        }
    }
}

