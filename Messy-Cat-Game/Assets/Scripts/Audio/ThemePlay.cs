using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemePlay : MonoBehaviour
{
    public EffectManager effectManager;

    public bool started;

    void Start()
    {

    }

    private void Update()
    {
        if (!started)
        {
            if (effectManager != null)
            {
                if(effectManager.hasSetMixer)
                {
                    started = true;

                    AudioManager.instance.Play("MainTheme");

                    AudioManager.instance.playingMainTheme = true;
                }
            }
            else
            {
                started = true;

                AudioManager.instance.Play("MainTheme");

                AudioManager.instance.playingMainTheme = true;
            }
        }
    }
}
