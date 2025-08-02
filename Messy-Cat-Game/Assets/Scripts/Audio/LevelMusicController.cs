using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.ProBuilder.MeshOperations;

public class LevelMusicController : MonoBehaviour
{
    public bool triggerChange;
    public bool beingInvestigated;

    [Header("Music Layers")]
    public AudioSource musicLayer1;  // always present
    public AudioSource musicLayer2;  // ignored if 1 mess to make, fades in at second mess if 2-3 messes to make, fades in at 33% if 4+ messes to make
    public AudioSource musicLayer3;  // ignored if 1-3 messes to make, fades in at 66% if 4+ messes to make
    public AudioSource investigationMusic;  //for when they're on to you

    [Header("Music Settings")]
    public float fadeDuration = 0.3f;   // sets time for layers to fade in
    [SerializeField] private float levelMusicVolume = 0.7f; //set volume vs other music tracks separate from the mixer
    [SerializeField] private float InvestigationMusicVolume = 0.7f; //set volume of investigation music

    [Header("References")]
    public LevelManager levelManager;

    private float messProgress;         // this is just for music purposes. Gives us a percentage, regardless of how many messes in a level
    private int lastIntensity = -1;     // forces volume update immediately upon return, so current settings are re-checked

    private Coroutine fadeLayer1;
    private Coroutine fadeLayer2;
    private Coroutine fadeLayer3;
    private Coroutine fadeInvestigation;


    private void Update()
    {
        if (levelManager != null && levelManager.totalMessNeeded > 0)
        {
            messProgress = (float)levelManager.currentMesses / levelManager.totalMessNeeded;
            UpdateMusicLayers();
        }
    }

    public void StartLevelMusic()
    {
        if (levelManager == null) return;

        // Means music will start from the beginning, Point of difference from Resume, which will play from where we're up to in the music
        musicLayer1.Stop();
        musicLayer2.Stop();
        musicLayer3.Stop();
        investigationMusic.Stop();

        // Set volumes to starting state
        musicLayer1.volume = levelMusicVolume;
        musicLayer2.volume = 0.0001f;
        musicLayer3.volume = 0.0001f;
        investigationMusic.volume = 0.0001f;

        // Play all layers again
        musicLayer1.Play();
        musicLayer2.Play();
        musicLayer3.Play();

        // Reset tracking so fades work again
        lastIntensity = -1;
    }



    public void MuteLevelMusic()    // I figured a mute instead of pause could be good when playing the dog or human music?
    {
        FadeTo(musicLayer1, 0.0001f, false);
        FadeTo(musicLayer2, 0.0001f, false);
        FadeTo(musicLayer3, 0.0001f, false);
        FadeTo(investigationMusic, 0.0001f, false);
    }

    public void ResumeLevelMusic()    // restores volumes after music has been muted (e.g. dog or human music ends)
    {
        UpdateMusicLayers(true);
    }

    public void StopLevelMusic()    // should work for end of level? Or maybe for dog/human music if we want this to start from beginning when the alert state dies down?
    {
        if (musicLayer1 != null && musicLayer1.isPlaying) musicLayer1.Stop();
        if (musicLayer2 != null && musicLayer2.isPlaying) musicLayer2.Stop();
        if (musicLayer3 != null && musicLayer3.isPlaying) musicLayer3.Stop();
        if (investigationMusic != null && investigationMusic.isPlaying) investigationMusic.Stop();

        lastIntensity = -1; // Reset
    }

    private void UpdateMusicLayers(bool forceImmediate = false)
    {
        // If we're currently being investigated (e.g. the dog is onto you), we override all other music.
        if (beingInvestigated)
        {
            Debug.Log("being investigated");

            if (!investigationMusic.isPlaying)
            {
                Debug.Log("switched to investigate music");
                investigationMusic.Play();
            }

            FadeTo(investigationMusic, InvestigationMusicVolume, forceImmediate);

            FadeTo(musicLayer1, 0.0001f, forceImmediate);
            FadeTo(musicLayer2, 0.0001f, forceImmediate);
            FadeTo(musicLayer3, 0.0001f, forceImmediate);

            return;
        }
        else
        {
            //Debug.Log("not being investigated");

            // If investigation mode is off, fade out investigation music
            FadeTo(investigationMusic, 0.0001f, forceImmediate);

            if (investigationMusic.isPlaying)
            {
                investigationMusic.Stop();      //so it plays from the start next time
                Debug.Log("Tried to stopped playing the audio source for investigate");
            }
        }



        // Handle 1–3 mess levels (slow music)
        if (levelManager.totalMessNeeded <= 3)
        {
            FadeTo(musicLayer1, levelMusicVolume, forceImmediate);

            bool secondMessReached = levelManager.currentMesses >= 1;
            FadeTo(musicLayer2, secondMessReached ? levelMusicVolume : 0.0001f, forceImmediate);

            FadeTo(musicLayer3, 0.0001f, forceImmediate);
        }
        else
        {
            // Handle 4–8 mess levels (fast music with full intensity layering)
            int currentIntensity = GetIntensityFromProgress(messProgress);

            if (currentIntensity != lastIntensity || forceImmediate)
            {
                switch (currentIntensity)
                {
                    case 0: // 0–33% progress
                        FadeTo(musicLayer1, levelMusicVolume, forceImmediate);
                        FadeTo(musicLayer2, 0.0001f, forceImmediate);
                        FadeTo(musicLayer3, 0.0001f, forceImmediate);
                        break;

                    case 1: // 33–66% progress
                        FadeTo(musicLayer1, levelMusicVolume, forceImmediate);
                        FadeTo(musicLayer2, levelMusicVolume, forceImmediate);
                        FadeTo(musicLayer3, 0.0001f, forceImmediate);
                        break;

                    case 2: // 66–100% progress
                        FadeTo(musicLayer1, levelMusicVolume, forceImmediate);
                        FadeTo(musicLayer2, levelMusicVolume, forceImmediate);
                        FadeTo(musicLayer3, levelMusicVolume, forceImmediate);
                        break;
                }

                lastIntensity = currentIntensity;
            }
        }
    }


    private int GetIntensityFromProgress(float progress)
    {
        if (progress < 0.334f) return 0;
        else if (progress < 0.667f) return 1;
        else return 2;
    }

    private void FadeTo(AudioSource source, float targetVolume, bool immediate = false)
    {
        targetVolume = Mathf.Max(targetVolume, 0.0001f);

        if (immediate)
        {
            source.volume = targetVolume;
            return;
        }

        // Stop previous fade on this source
        Coroutine fade = null;
        if (source == musicLayer1) { if (fadeLayer1 != null) StopCoroutine(fadeLayer1); fade = fadeLayer1 = StartCoroutine(FadeVolume(source, targetVolume)); }
        else if (source == musicLayer2) { if (fadeLayer2 != null) StopCoroutine(fadeLayer2); fade = fadeLayer2 = StartCoroutine(FadeVolume(source, targetVolume)); }
        else if (source == musicLayer3) { if (fadeLayer3 != null) StopCoroutine(fadeLayer3); fade = fadeLayer3 = StartCoroutine(FadeVolume(source, targetVolume)); }
        else if (source == investigationMusic) { if (fadeInvestigation != null) StopCoroutine(fadeInvestigation); fade = fadeInvestigation = StartCoroutine(FadeVolume(source, targetVolume)); }
    }


    IEnumerator FadeVolume(AudioSource source, float targetVolume)
    {
        float startVolume = source.volume;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            source.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        source.volume = targetVolume;
    }
}
