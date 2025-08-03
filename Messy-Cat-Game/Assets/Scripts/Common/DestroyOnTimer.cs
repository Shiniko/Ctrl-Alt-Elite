using UnityEngine;

public class DestroyOnTimer : MonoBehaviour
{
    public float time;

    void Start()
    {
        if (gameObject != null)
        {
            // Do something  
            Destroy(gameObject, time);
        }
    }
}
