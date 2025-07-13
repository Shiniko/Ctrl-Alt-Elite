using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class DogContext : MonoBehaviour
{
    public SphereCollider dogHearing { get; private set; }
    public DogVision vision { get; private set; }


    [Header("Required References")]
    [Tooltip("For smoother movement change the interpolation setting to 'interpolate' ")]
    [SerializeField] private Rigidbody rb;
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;

    [Header("Roaming Settings")]
    [SerializeField] private float _maxRoamX;
    [SerializeField] private float _minRoamX;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new(_minRoamX,transform.position.y,transform.position.z),1);
        Gizmos.DrawWireSphere(new(_maxRoamX, transform.position.y, transform.position.z), 1);
    }

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }



    public float GetMaxRoamRange()
    {
        return _maxRoamX;
    }

    public float GetMinRoamRange()
    {
        return _minRoamX;
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
}




