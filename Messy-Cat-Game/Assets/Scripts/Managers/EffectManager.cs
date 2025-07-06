using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private AudioManager audioManager;

    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private bool prefsReady;
    public bool hasLoadedPrefs;
    public bool hasSetMixer;

    public float sfxVolume;
    public float musicVolume;

    void Start()
    {
        if (audioManager == null)
        {
            if (FindAnyObjectByType<AudioManager>() != null)
            {
                audioManager = FindAnyObjectByType<AudioManager>();
            }
        }

        if (gameManager == null)
        {
            if (GameObject.FindGameObjectWithTag("GameController") != null)
            {
                gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            }
        }
    }

    void Update()
    {
        if (gameManager != null)
        {
            prefsReady = true;
        }

        if (!hasLoadedPrefs)
        {
            if (prefsReady)
            {
                if (PlayerPrefs.HasKey("MusicVolume"))
                {
                    GrabPrefs();

                    hasLoadedPrefs = true;

                    if (audioMixer != null)
                    {
                        SetMixer();
                    }

                }
                else
                {
                    float volume = 0.4f;

                    PlayerPrefs.SetFloat("MusicVolume", volume);
                    PlayerPrefs.SetFloat("SFXVolume", volume);
                }
            }
        }
    }

    void GrabPrefs()
    {
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");

        if (sfxVolume <= 0)
        {
            sfxVolume = 0.001f;
        }

        if (musicVolume <= 0)
        {
            musicVolume = 0.001f;
        }

        if (sfxVolume >= 1)
        {
            sfxVolume = 1f;
        }

        if (musicVolume >= 1)
        {
            musicVolume = 1f;
        }
    }

    void SetMixer()
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat("music", Mathf.Log10(musicVolume) * 20f);
            audioMixer.SetFloat("sfx", Mathf.Log10(sfxVolume) * 20f);

            hasSetMixer = true;
        }
    }
}
