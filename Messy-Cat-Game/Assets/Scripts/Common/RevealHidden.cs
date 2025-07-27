using UnityEngine;

public class RevealHidden : MonoBehaviour
{
    [SerializeField] private bool revealOnAwake;
    [SerializeField] private GameObject objectToReveal;

    void Awake()
    {
        if (revealOnAwake)
        {
            if(objectToReveal != null)
            {
                objectToReveal.SetActive(true);
            }
        }
    }
}
