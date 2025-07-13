using UnityEngine;
using System.Collections.Generic;
public class SuspiciousEvent
{
    public static List<SuspiciousEvent> suspiciousEvents { get; private set; }
    public Vector3 origin { get; private set; }
    public SuspiciousEvent(Vector3 origin)
    {
        this.origin = origin;
        suspiciousEvents.Add(this);
    }

    public void RemoveSuspiciousEvent(SuspiciousEvent suspiciousEvent)
    {
        suspiciousEvents.Remove(suspiciousEvent);
    }

}
