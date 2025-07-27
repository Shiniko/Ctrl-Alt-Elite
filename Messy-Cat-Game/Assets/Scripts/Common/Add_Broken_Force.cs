using UnityEngine;

public class Add_Broken_Force : MonoBehaviour
{
    public float explosionForce; // Adjust this value for the desired force
    public float explosionRadius; // Adjust this for the area of effect
    public float upwardsModifier; // Add a slight upward lift
    public float explosionDelay = 0.1f; // Add a slight delay to allow children to activate
    private float splodeCounter; // timer realated to delay
    [SerializeField] private GameObject child; // timer realated to delay
    private bool triggeredExplosion;

    void Awake()
    {
        if(child != null)
        {
            child.SetActive(true);
        }
    }

    void Update()
    {
        if (!triggeredExplosion)
        {
            if (splodeCounter < explosionDelay)
            {
                splodeCounter += Time.deltaTime;
            }
            else
            {
                triggeredExplosion = true;

                Debug.Log("Sploded");

                Splode();
            }
        }
    }

    private void Splode()
    {
        // Iterate through all child GameObjects of this parent
        foreach (Transform child in transform)
        {
            // Get the Rigidbody component of each child
            Rigidbody rb = child.GetComponent<Rigidbody>();

            // Only apply force if a Rigidbody exists and the GameObject is active
            if (rb != null && child.gameObject.activeInHierarchy)
            {
                // Calculate the direction from the parent's position to the child's position
                Vector3 direction = (child.position - transform.position).normalized;

                // Add an explosion force to each piece
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier);
            }
        }
    }
}
