using UnityEngine;

public class RevealHidden : MonoBehaviour
{
    [SerializeField] private bool revealOnAwake;
private bool triggerEffect;
    [SerializeField] private GameObject objectToReveal;
    [SerializeField] private GameObject revealEffect;

    void Awake()
    {
        if (revealOnAwake)
        {
            if (!triggerEffect)
            {
                triggerEffect = true;

                if (revealEffect != null)
                {
                    Instantiate(revealEffect, transform.position, Quaternion.identity);
                }
            }

            if (objectToReveal != null)
            {
                objectToReveal.SetActive(true);
            }
        }
    }
}
