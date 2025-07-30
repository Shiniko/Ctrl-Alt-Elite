using UnityEngine;

public class PulleySystemHelper : MonoBehaviour
{
    [SerializeField] private PulleySystem ps;

    public void KillPulley()
    {
        if(ps != null)
        {
            ps.RemovePulley();
        }
    } 
}
