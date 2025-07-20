using UnityEngine;
public class CatInteractions : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    private bool interacting = false;

    public static CatInteractions Instance { get; private set; }
    public void Awake()
    {
        if(Instance == null) 
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public KeyCode GetInteractKey()
    {
        return interactKey;
    }
}
