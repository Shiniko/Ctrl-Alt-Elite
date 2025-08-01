using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameObject goalEffect;
    [SerializeField] private GameObject revealObject;
private bool triggerEffect;
private bool triggerReveal;

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Ball"))
        {
            if (!triggerEffect)
            {
                triggerEffect = true;

                if (goalEffect != null)
                {
                    Instantiate(goalEffect, col.transform.position, Quaternion.identity);
                }
            }

            if (!triggerReveal)
            {
                if(revealObject  != null)
                {
                    revealObject.SetActive(true);
                }
            }
        }
    }
}
