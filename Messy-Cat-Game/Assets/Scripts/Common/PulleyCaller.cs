using UnityEngine;

public class PulleyCaller : MonoBehaviour
{
    [SerializeField] private PulleySystemHelper pulleySystemHelper;

    void Awake()
    {
        if (pulleySystemHelper != null)
        {
            pulleySystemHelper.KillPulley();
        }
    }
}
