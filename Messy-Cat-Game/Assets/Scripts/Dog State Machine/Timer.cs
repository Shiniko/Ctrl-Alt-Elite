using UnityEngine;

public static class Timer
{
    public static void StartTimer(ref bool done, float duration)
    {
        float time = 0f;
        while(time < duration)
        {
            time += Time.deltaTime;
        }
        done = true;
    }
}
