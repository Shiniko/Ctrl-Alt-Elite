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
    [SerializeField] private bool startRoaming = true;

    //Investigation settings
    [SerializeField] private float investigationTime = 5f;

    //Agro settings
    [SerializeField] private float seeCatTime = 2f;
    [SerializeField] private Image dogAgroMeter;
    [SerializeField] private float barkingRange = 2f;
    public bool isAngry;

    //Gizmo settings
    [SerializeField] private float _size = 0.25f;
    [SerializeField] private Color _gizmoColor = Color.green;



    public Rigidbody rb { get; private set; }
    public Vector3 currentSuspiciousEvent;
    public DogVision dogVision { get; private set; }
    public Transform player { get; private set; }

    private Animator animator;

    // Animator hash variables
    public static readonly int stallingHash = Animator.StringToHash("Stalling");
    public static readonly int distractedHash = Animator.StringToHash("Distracted");
    public static readonly int investigateHash = Animator.StringToHash("Investigate");
    public static readonly int roamingHash = Animator.StringToHash("Roaming");
    public static readonly int barkingHash = Animator.StringToHash("Bark");
    private static readonly string playerHash = "Player";

    //Vector3 cache variables
    Vector3 newLocation;
    //Int cache variables
    int countTime = 0;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireSphere(new(_minRoamDistance, transform.position.y, transform.position.z), _size);
        Gizmos.DrawWireSphere(new(_maxRoamDistance, transform.position.y, transform.position.z), _size);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (!GetComponentInChildren<DogVision>())
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
            dogAgroMeter.type = Image.Type.Filled;
            dogAgroMeter.fillAmount = 0f;
        }
    }

    void Start()
    {
        if (startRoaming)
        {
            //Start roaming
            animator.SetBool(roamingHash, true);
        }

        if (dogAgroMeter.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("<color=yellow>Dog Agro Meter</color> is active in hierarchy! This should be set to <color=yellow>inactive</color> by default and only activated when the dog is suspicious of the player", this);
            dogAgroMeter.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(player == null)
        {
            if (GameObject.FindGameObjectWithTag(playerHash) != null)
            {
                player = GameObject.FindGameObjectWithTag(playerHash).transform;
            }
        }
    }

    /// <summary>
    /// Calculates a new location for the dog to go to based on the min and max roam distances and the specified axis to travel by
    /// </summary>
    /// <returns>Vector3</returns>
    public Vector3 GetNewRoamLocation()
    {
        if(Vector3.Distance(transform.position, new(transform.position.x, transform.position.y, _minRoamDistance)) < _minimumTravelDistance && Vector3.Distance(transform.position, new(transform.position.x, transform.position.y, _maxRoamDistance)) < _minimumTravelDistance)
        {
            Debug.LogWarning("There is no place to move the dog according to the minimum travel distance!! Ignoring it for now...");
            return (Vector3.right * Random.Range(_minRoamDistance, _maxRoamDistance + 0.5f)) + new Vector3(0f, transform.position.y, transform.position.z);
        }
        newLocation = (Vector3.right * Random.Range(_minRoamDistance, _maxRoamDistance + 0.5f)) + new Vector3(0f, transform.position.y, transform.position.z);
        while (Vector3.Distance(newLocation, transform.position) < _minimumTravelDistance)
        {
            countTime++;
            if(countTime > 20)
            {
                Debug.LogWarning("Preventing <color=red>infinite loop</color> (the function has looped for 20 times) and returning a<color=yellow> potentially wrong </color>location...(this is a soft fix for a bug)");
                break; //Prevent infinite loop
            }
            newLocation = (Vector3.right * Random.Range(_minRoamDistance, _maxRoamDistance + 0.5f)) + new Vector3(0f, transform.position.y, transform.position.z);
        }
        countTime = 0; //Reset the count time for the next call
        return newLocation;
    }

    

    public void TestInvestigateState()
    {
        currentSuspiciousEvent =GetNewRoamLocation();
        animator.SetTrigger(investigateHash);
    }

    public float GetBarkingRange()
    {
        return barkingRange;
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

    public Image GetAgroMeter()
    {
        return dogAgroMeter;
    }

    public float GetSeeCatTime()
    {
        return seeCatTime;
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

    public void SetAngry()
    {
        isAngry = true;

        Debug.Log("setting dog as isAngry");

        if (GameObject.FindGameObjectWithTag("LevelManager") != null)
        {
            GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().DogSeesCat();
        }
    }

    /// <summary>
    /// Sets the dog to no longer be angry, resets the animator triggers and sets the chasing bool to false
    /// </summary>
    public void SetNoLongerAngry()
    {
        isAngry = false;
        animator.ResetTrigger("Calm Down");
        animator.SetBool("Chasing", false);
    }

    /// <summary>
    /// Sets the dog to be calm, resets the animator triggers and sets the chasing bool to false
    /// </summary>
    public void CalmDog()
    {
        isAngry = false;                                        //set this so human doesnt re-pet the dog
        animator.SetTrigger("Calm Down");
        animator.ResetTrigger("Bark");
        animator.SetBool("Chasing", false);
    }
}




