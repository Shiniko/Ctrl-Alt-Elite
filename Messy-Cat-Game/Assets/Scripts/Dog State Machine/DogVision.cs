using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
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
    private Slider agroMeter;

    public bool suspicious;
    RaycastHit hit;

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

        int dogLayer = LayerMask.NameToLayer("Dog");
        if(dogLayer == -1)
        {
            Debug.LogError("Dog layer does not exist! Please create a layer named 'Dog' in the Unity editor.", this);
            return;
        }
        if(!(gameObject.layer == dogLayer))
        {
            Debug.LogWarning("This object needs to be on the dog layer! Automatically setting it...", this);
            gameObject.layer = dogLayer;
        }
    }

    private void Start()
    {
        agroMeter = dogContext.GetAgroMeter();
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

    private new void Update()
    {
        base.Update();
        IfCatVisible();
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

    void IfCatVisible()
    {
        if (IsVisible(dogContext.player))
        {
            if (!suspicious)
            {
                suspicious = true;
                dogContext.currentSuspiciousEvent = new SuspiciousEvent(dogContext.transform.position);
                animator.SetTrigger("Suspicious");
            }

            agroMeter.gameObject.SetActive(true);

            agroMeter.value += Time.deltaTime;
        }
        else
        {
            agroMeter.gameObject.SetActive(false);
        }
    }

    private void UpdateViewableObjects()
    {
        foreach (GameObject other in objectsInViewZone)
        {
            if(other == null)
            {
                //Debug.Log("Skipping null object", this);
                continue; // Skip null objects
            }
            //Shoot raycast
            float distance = Vector3.Distance(parentTransform.position, other.transform.position) + 0.1f;
            Vector3 direction = (other.transform.position - parentTransform.position).normalized;
            //If we fail to hit anything with our raycast, then we skip the rest of the code
            if(!Physics.Raycast(parentTransform.position, direction, out hit, distance, layerMask))
            {
                //Debug.Log("Raycast failed to hit anything");
                continue;
            }

            //Check if the raycast hit the object
            if (hit.collider.gameObject == other)
            {
                //Debug.Log("Object is visible");
                Debug.DrawLine(parentTransform.position, other.transform.position, Color.green, lineTime,true);
                if (!viewableObjects.Contains(other))
                {
                    viewableObjects.Add(other);
                }
            }
            else
            {
                //Debug.Log("Object is not visible, instead we hit" + hit.collider.gameObject.name);
                Debug.DrawLine(parentTransform.position, other.transform.position, Color.red, lineTime,true);
            }
        }
    }

}

