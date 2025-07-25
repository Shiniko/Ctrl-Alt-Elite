using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Testing
/// </summary>
public class SuspiciousEvent
{
    public Vector3 origin { get; private set; }
    public SuspiciousEvent(Vector3 origin)
    {
        this.origin = origin;
    }
}
