using UnityEngine;
//Required for movement
[RequireComponent(typeof(Rigidbody))]
//Required for collision detection
[RequireComponent(typeof(Collider))]

public class DogContext : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    [SerializeField] private float _maxRoamDistance = -1;
    [SerializeField] private float _minRoamDistance = 1;
    [SerializeField] private float stallTime = 2f;
    [SerializeField] private MovementAxis movementAxis;

    [SerializeField] private float _size = 0.25f;
    [SerializeField] private Color _gizmoColor = Color.green;

    private Rigidbody rb;
    public SphereCollider dogHearing { get; private set; }
    public DogVision vision { get; private set; }
    public SuspiciousEvent currentSuspiciousEvent { get; private set; }
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

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if(!GetComponentInChildren<DogVision>())
        {
            Debug.LogError("A DogVision script is not attached to a child of this object!! DogContext requires a DogVision script to be in a child object",this);
        }
    }

    /// <summary>
    /// Calculates a new location for the dog to go to based on the min and max roam distances and the specified axis to travel by
    /// </summary>
    /// <returns>Vector3</returns>
    public Vector3 GetNewRoamLocation()
    {
        switch (movementAxis) 
        {
            case MovementAxis.X:
                return (Vector3.right * Random.Range(_minRoamDistance,_maxRoamDistance + 0.5f)) + new Vector3(0,transform.position.y, transform.position.z);
            case MovementAxis.Z:
                return Vector3.forward * Random.Range(_minRoamDistance, _maxRoamDistance + 0.5f) + new Vector3(transform.position.x, transform.position.y,0);
            default:
                Debug.LogError("Movement Axis has not been set to a valid value!!", this);
                return Vector3.zero;
        }

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
    public float GetSpeed()
    {
        return speed;
    }

    public float GetStallTime()
    {
        return stallTime;
    }

    enum MovementAxis
    {
        X, Z
    }
}




