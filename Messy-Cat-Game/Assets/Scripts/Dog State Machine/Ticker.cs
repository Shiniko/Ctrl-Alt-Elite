using UnityEngine;

//Runs code slightly slower than every frame in order to safe resources
public class Ticker : MonoBehaviour
{
    protected static float tickTime = 0.2f;

    protected float _tickerTimer;

    public delegate void TickAction();
    //subscribe to this event to use the ticker
    public static event TickAction OnTickAction;

    protected void Update()
    {
        _tickerTimer = Time.time;

        if(_tickerTimer >= tickTime)
        {
            _tickerTimer = 0;
            TickEvent();
        }
    }

    protected void TickEvent()
    {
        OnTickAction?.Invoke();
    }
}
