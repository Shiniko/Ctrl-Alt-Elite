using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    public string effectName;

    [SerializeField] private float testCounter;
    [SerializeField] private float testCD;
    [SerializeField] private bool canTest;

    void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    void Update()
    {
        if (testCounter < testCD)
        {
            testCounter += Time.deltaTime;
            canTest = false;
        }
        else
        {
            testCounter = testCD;
            canTest = true;
        }
    }

    public void SetMusicVolume()
    {
        if (musicSlider != null)
        {
            float volume = musicSlider.value;
            audioMixer.SetFloat("music", Mathf.Log10(volume) * 20f);
            PlayerPrefs.SetFloat("MusicVolume", volume);

            //Debug.Log("Set Music volume to " + volume);
        }
    }

    public void SetSFXVolume()
    {
        if (sfxSlider != null)
        {
            float volume = sfxSlider.value;
            audioMixer.SetFloat("sfx", Mathf.Log10(volume) * 20f);
            PlayerPrefs.SetFloat("SFXVolume", volume);

            if (canTest)
            {
                FindFirstObjectByType<AudioManager>().Play(effectName);

                canTest = false;
                testCounter = 0f;
            }
            else
            {
                if (volume == 1f)
                {
                    if (testCounter > (testCD * 0.5f))
                    {
                        FindFirstObjectByType<AudioManager>().Play(effectName);
                    }
                }
            }
        }
    }

    private void LoadVolume()
    {
        if (musicSlider != null)
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            SetMusicVolume();
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
            SetSFXVolume();
        }
    }
}
