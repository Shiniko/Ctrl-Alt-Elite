using UnityEngine;
//Required for movement
[RequireComponent(typeof(Rigidbody))]
//Required for collision detection
[RequireComponent(typeof(Collider))]
public class DogContext : MonoBehaviour
{
    public SphereCollider dogHearing { get; private set; }
    public DogVision vision { get; private set; }


    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float _maxRoamDistance;
    [SerializeField] private float _minRoamDistance;
    [SerializeField] private MovementAxis movementAxis;
    [SerializeField] private float size = 0.25f;
    [SerializeField] private Color gizmoColor = Color.green;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(new(_minRoamDistance,transform.position.y,transform.position.z),size);
        Gizmos.DrawWireSphere(new(_maxRoamDistance, transform.position.y, transform.position.z), size);
    }

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
            case MovementAxis.Y:
                return (Vector3.up * Random.Range(_minRoamDistance, _maxRoamDistance + 0.5f)) + new Vector3(transform.position.x,0, transform.position.z);
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

    enum MovementAxis
    {
        X, Y, Z
    }
}




