using UnityEngine;
using UnityEngine.UI;
//Required for movement
[RequireComponent(typeof(Rigidbody))]
//Required for collision detection
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]

public class DogContext : MonoBehaviour
{
    //Movement settings
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float canterSpeed = 4f;

    //Roming settings
    [SerializeField] private float _maxRoamDistance = -1;
    [SerializeField] private float _minRoamDistance = 1;
    [SerializeField] private float _minimumTravelDistance = 2f;
    [SerializeField] private float stallTime = 2f;
    [SerializeField] private MovementAxis movementAxis;
    [SerializeField] private bool startRoaming = true;

    //Investigation settings
    [SerializeField] private float investigationTime = 5f;

    //Agro settings
    [SerializeField] private float seeCatTime = 2f;
    [SerializeField] private Slider dogAgroMeter;

    //Gizmo settings
    [SerializeField] private float _size = 0.25f;
    [SerializeField] private Color _gizmoColor = Color.green;



    private Rigidbody rb;
    public SuspiciousEvent currentSuspiciousEvent;
    public DogVision dogVision { get; private set; }
    public GameObject player { get; private set; }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _gizmoColor;
        //Dog roaming distance gizmos
        switch (movementAxis)
        {
            case MovementAxis.X:
                Gizmos.DrawWireSphere(new(_minRoamDistance, transform.position.y, transform.position.z), _size);
                Gizmos.DrawWireSphere(new(_maxRoamDistance, transform.position.y, transform.position.z), _size);
                break;
            case MovementAxis.Z:
                Gizmos.DrawWireSphere(new(transform.position.x, transform.position.y, _minRoamDistance), _size);
                Gizmos.DrawWireSphere(new(transform.position.x, transform.position.y, _maxRoamDistance), _size);
                break;
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if(!GetComponentInChildren<DogVision>())
        {
            Debug.LogError("A <color=lime>DogVision</color> script is <color=yellow>not attached to a child</color> of this object!! <color=lime>DogContext</color> <color=red>requires</color> a <color=lime>DogVision</color> script to be in a child object.", this);
        }
        else
        {
            dogVision = GetComponentInChildren<DogVision>();
        }
        if(dogAgroMeter == null)
        {
            Debug.LogWarning("<color=lime>DogContext</color> is missing a reference to a <color=lime>slider</color> for the <color=yellow>dogAgroMeter</color> variable");
            dogVision.enabled = false;
            Debug.LogWarning("Disabling DogVision to prevent futher errors");
        }
        else
        {
            dogAgroMeter.value = 0f;
            dogAgroMeter.maxValue = seeCatTime;
        }
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        if (startRoaming)
        {
            //Start roaming
            GetComponent<Animator>().SetBool("Roaming", true);
        }
    }

    /// <summary>
    /// Calculates a new location for the dog to go to based on the min and max roam distances and the specified axis to travel by
    /// </summary>
    /// <returns>Vector3</returns>
    public Vector3 GetNewRoamLocation()
    {
        Vector3 newLocation;
        switch (movementAxis) 
        {
            case MovementAxis.X:
                newLocation = (Vector3.right * Random.Range(_minRoamDistance, _maxRoamDistance + 0.5f)) + new Vector3(0, transform.position.y, transform.position.z);
                while (Vector3.Distance(newLocation,transform.position) < _minimumTravelDistance)
                {
                    newLocation = (Vector3.right * Random.Range(_minRoamDistance, _maxRoamDistance + 0.5f)) + new Vector3(0, transform.position.y, transform.position.z);
                }
                break;
            case MovementAxis.Z:
                newLocation = Vector3.forward * Random.Range(_minRoamDistance, _maxRoamDistance + 0.5f) + new Vector3(transform.position.x, transform.position.y, 0);
                while (Vector3.Distance(newLocation, transform.position) < _minimumTravelDistance)
                {
                    newLocation = Vector3.forward * Random.Range(_minRoamDistance, _maxRoamDistance + 0.5f) + new Vector3(transform.position.x, transform.position.y, 0);
                }
                break;
            default:
                Debug.LogError("Movement Axis has not been set to a valid value!!", this);
                newLocation = Vector3.zero;
                break;
        }
        return newLocation;

    }


    public void TestInvestigateState()
    {
        currentSuspiciousEvent = new SuspiciousEvent(GetNewRoamLocation());
        GetComponent<Animator>().SetTrigger("Investigate");
    }


    /// <summary>
    /// Returns the rigidbody that is attached to the same game object as the Dog Context script
    /// </summary>
    /// <returns></returns>
    public Rigidbody GetRigidbody()
    {
        return rb;
    }

    /// <summary>
    /// Returns the speed of a dog
    /// </summary>
    /// <returns>float</returns>
    public float GetWalkSpeed()
    {
        return walkSpeed;
    }

    public float GetStallTime()
    {
        return stallTime;
    }

    public Slider GetAgroMeter()
    {
        return dogAgroMeter;
    }

    public float GetRunSpeed()
    {
        return runSpeed;
    }

    public float GetCanterSpeed()
    {
        return canterSpeed;
    }

    public float GetInvestigationTime()
    {
        return investigationTime;
    }

    enum MovementAxis
    {
        X, Z
    }
}




