using UnityEngine;

public class Add_Broken_Force : MonoBehaviour
{
    public float explosionForce; // Adjust this value for the desired force
    public float explosionRadius; // Adjust this for the area of effect
    public float upwardsModifier; // Add a slight upward lift
    public float explosionDelay = 0.1f; // Add a slight delay to allow children to activate
    private float splodeCounter; // timer realated to delay
    public float explosionDuration = 1f; // Add a slight delay to allow children to activate
    private float durationCounter; // timer realated to delay
    [SerializeField] private GameObject child; // timer realated to delay
    private Rigidbody[] rigidChildrens;
    private bool triggeredExplosion;

    void Awake()
    {
        if(child != null)
        {
            child.SetActive(true);

            rigidChildrens = child.GetComponentsInChildren<Rigidbody>();
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
        else
        {
            if (durationCounter < explosionDuration)
            {
                durationCounter += Time.deltaTime;

                Splode();
            }
        }
    }

    private void Splode()
    {
        // Iterate through all child rigids
        for(int i = 0; i < rigidChildrens.Length; ++i)
        {
            if(rigidChildrens[i] != null)
            {
                if (child.gameObject.activeInHierarchy)
                {
                    Rigidbody rb = rigidChildrens[i];

                    Vector3 direction = (rb.transform.position - transform.position).normalized;
                    direction = new Vector3(direction.x, direction.y - 0.5f, direction.z + 0.25f);

                    if (!triggeredExplosion)
                    {
                        // Add an explosion force to each piece in a relative direction from this
                        rb.AddExplosionForce(explosionForce, direction, explosionRadius, upwardsModifier);
                    }

                    // The apply force to each piece in a an impulse over time if still in radius
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier, ForceMode.Impulse);
                }
            }
        }
    }
}
