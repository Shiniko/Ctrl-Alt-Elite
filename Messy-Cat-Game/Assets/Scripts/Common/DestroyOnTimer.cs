using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTimer : MonoBehaviour
{
    public float time;

    void Start()
    {

    }


    void Update()
    {
        if (gameObject != null)
        {
            // Do something  
            Destroy(gameObject, time);
        }
    }
}
