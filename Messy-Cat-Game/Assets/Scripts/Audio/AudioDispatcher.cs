using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDispatcher : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private int clipIndex;

    [Header("Array Params")]
    [SerializeField] private string[] clipNames;
    [SerializeField] private float[] clipCoolDowns;
    [SerializeField] private float[] clipCounters;

    [SerializeField] private int clipNameLength;
    [SerializeField] private int clipCoolDownLength;
    [SerializeField] private int clipCounterLength;

    void Awake()
    {
        clipNameLength = clipNames.Length;
        clipCoolDownLength = clipCoolDowns.Length;
        clipCounterLength = clipCounters.Length;
    }

    void Start()
    {
        if (audioManager == null)
        {
            if (gameObject.GetComponent<AudioManager>() != null)
            {
                audioManager = gameObject.GetComponent<AudioManager>();
            }
        }

        if(audioManager != null)
        {

        }
    }

    void Update()
    {
        for (int i = 0; i < clipNameLength; i++)
        {
            if ((clipCounterLength - 1) >= i)
            {
                if ((clipCoolDownLength - 1) >= i)
                {
                    if (clipCounters[i] < clipCoolDowns[i])
                    {
                        clipCounters[i] += Time.deltaTime;
                    }
                    else
                    {
                        clipCounters[i] = clipCoolDowns[i];
                    }
                }
            }
        }
    }

    public void PlayClip(string clip)
    {
        clipIndex = System.Array.IndexOf(clipNames, clip);

        int tempIndex = clipIndex;

        if (clipIndex >= 0)
        {
            if ((clipCounterLength - 1) >= tempIndex)
            {
                if ((clipCoolDownLength - 1) >= tempIndex)
                {
                    if (clipCounters[tempIndex] >= clipCoolDowns[tempIndex])
                    {
                        if (audioManager != null)
                        {
                            audioManager.Play(clip);

                            clipCounters[tempIndex] = 0f;
                        }
                    }
                }
            }
        }
    }
}
