using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private CreditScroller creditScroller;
    private bool triggerCredits;

    [SerializeField] private float creditsDelay;
    private float creditsCounter;

    void Start()
    {
        if( creditScroller == null)
        {
            if(GameObject.FindGameObjectWithTag("CreditScroller") != null)
            {
                creditScroller = GameObject.FindGameObjectWithTag("CreditScroller").GetComponent<CreditScroller>();
            }
        }
    }

    void Update()
    {
        if(!triggerCredits && creditScroller != null)
        {
            if (creditsCounter < creditsDelay)
            {
                creditsCounter += Time.deltaTime;
            }
            else
            {
                triggerCredits = true;

                creditScroller.StartCredits();
            }
        }
    }
}
