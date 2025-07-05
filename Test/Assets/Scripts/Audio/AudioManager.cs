using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [Header("References")]
    public Sound[] sounds;
    public static AudioManager instance;
    public AudioMixer audioMixer;
    public AudioMixerGroup audioMixerMusic;
    public AudioMixerGroup audioMixerSFX;

    [Header("Volume Params")]
    [SerializeField] private float mainStartVol;

    [Header("Settings")]
    [SerializeField] private float swapDelay;
    public bool isPaused;
    public bool playingMainTheme;

    [Header("Fade Params")]
    public float fadeInDuration;
    public float fadeOutDuration;
    [SerializeField] private float fadeInElapsed;
    [SerializeField] private float fadeOutElapsed;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;

            if (audioMixer != null)
            {
                if (s.music)
                {
                    if (audioMixerMusic != null)
                    {
                        s.source.outputAudioMixerGroup = audioMixerMusic;
                    }
                }
                else
                {
                    if (s.sfx)
                    {
                        if (audioMixerSFX != null)
                        {
                            s.source.outputAudioMixerGroup = audioMixerSFX;
                        }
                    }
                }
            }
        }

        Sound m = Array.Find(sounds, sound => sound.name == "MainTheme");

        if (m == null)
        {
            Debug.LogWarning("Sound: MainTheme not found!");
            return;
        }
        else
        {
            mainStartVol = m.volume;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void SwapMusic(string clip)
    {
        Sound s = Array.Find(sounds, sound => sound.name == clip);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + clip + " not found!");
            return;
        }

        Sound m = Array.Find(sounds, sound => sound.name == "MainTheme");
        if (m == null)
        {
            Debug.LogWarning("Sound: MainTheme not found!");
            return;
        }
        else
        {
            playingMainTheme = false;
        }

        StartCoroutine(FadeTrackIn(clip));
        StartCoroutine(FadeTrackOut("MainTheme"));
    }

    public void MainTheme(string name)
    {
        Sound m = Array.Find(sounds, sound => sound.name == "MainTheme");
        if (m == null)
        {
            Debug.LogWarning("Sound: MainTheme not found!");
            return;
        }
        else
        {
            playingMainTheme = true;
        }

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        StartCoroutine(FadeTrackIn("MainTheme"));
        StartCoroutine(FadeTrackOut(name));
    }

    IEnumerator FadeTrackIn(string name)
    {
        fadeInElapsed = 0f;

        //Debug.Log("Fade In " + name);
        yield return new WaitForSeconds(swapDelay);

        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s != null)
        {
            float v = s.volume;
            s.volume = 0.01f;
            s.source.volume = 0.01f;

            s.source.Play();

            if (fadeInDuration < 0.25f)
            {
                fadeInDuration = 0.25f;
            }

            while (fadeInElapsed < fadeInDuration)
            {
                s.volume = Mathf.Lerp(0.01f, v, fadeInElapsed / fadeInDuration);
                s.source.volume = Mathf.Lerp(0.01f, v, fadeInElapsed / fadeInDuration);
                fadeInElapsed += Time.deltaTime;
                //Testing while for pause
                if (isPaused)
                {
                    // Debug.Log("Fade in While has Time of " + Time.deltaTime);
                }

                yield return null;
            }

            if (s.name == "MainTheme")
            {
                if (s.source.volume < mainStartVol)
                {
                    //Debug.Log("Adjusting main theme volume");
                    //Debug.Log("Main source vol was: " + s.source.volume);

                    s.source.volume = mainStartVol;
                    s.volume = mainStartVol;
                }
            }
        }
    }

    IEnumerator FadeTrackOut(string clip)
    {
        fadeOutElapsed = 0f;

        //Debug.Log("Fade Out " + clip);
        Sound s = Array.Find(sounds, sound => sound.name == clip);

        yield return new WaitForSeconds(0.5f);

        if (s != null)
        {
            float v = s.volume;

            if (fadeOutDuration < 0.25f)
            {
                fadeOutDuration = 0.25f;
            }

            while (fadeOutElapsed < fadeOutDuration)
            {
                s.volume = Mathf.Lerp(v, 0.01f, fadeOutElapsed / fadeOutDuration);
                s.source.volume = Mathf.Lerp(v, 0.01f, fadeOutElapsed / fadeOutDuration);
                fadeOutElapsed += Time.deltaTime;
                yield return null;
            }

            s.source.Stop();

            yield return new WaitForSeconds(0.1f);

            s.volume = v;
            s.source.volume = v;
        }
    }
}
