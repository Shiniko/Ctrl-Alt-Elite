using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class LevelMusicController : MonoBehaviour
{
    public bool triggerChange;

    [Header("Music Layers")]
    public AudioSource musicLayer1;  // always present
    public AudioSource musicLayer2;  // ignored if 1 mess to make, fades in at second mess if 2-3 messes to make, fades in at 33% if 4+ messes to make
    public AudioSource musicLayer3;  // ignored if 1-3 messes to make, fades in at 66% if 4+ messes to make

    [Header("Music Settings")]
    public float fadeDuration = 0.3f;   // sets time for layers to fade in

    [Header("References")]
    public LevelManager levelManager;

    private float messProgress;         // this is just for music purposes. Gives us a percentage, regardless of how many messes in a level
    private int lastIntensity = -1;     // forces volume update immediately upon return, so current settings are re-checked

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

        // Set volumes to starting state
        musicLayer1.volume = 1f;
        musicLayer2.volume = 0.0001f;
        musicLayer3.volume = 0.0001f;

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

        lastIntensity = -1; // Reset
    }

    private void UpdateMusicLayers(bool forceImmediate = false)
    {
        if (levelManager.totalMessNeeded <= 3)
        {
            // 1-3 messes: fade in layer 2 when second mess is made
            FadeTo(musicLayer1, 1f, forceImmediate);
            bool secondMessReached = levelManager.currentMesses >= 1;
            FadeTo(musicLayer2, secondMessReached ? 1f : 0.0001f, forceImmediate);
            FadeTo(musicLayer3, 0.01f, forceImmediate);
        }
        else
        {
            // 4–8 messes: use progress-based intensity system (calculation below)
            int currentIntensity = GetIntensityFromProgress(messProgress);

            if (currentIntensity != lastIntensity || forceImmediate)
            {
                switch (currentIntensity)
                {
                    case 0:
                        FadeTo(musicLayer1, 1f, forceImmediate);
                        FadeTo(musicLayer2, 0.0001f, forceImmediate);
                        FadeTo(musicLayer3, 0.0001f, forceImmediate);
                        break;
                    case 1:
                        FadeTo(musicLayer1, 1f, forceImmediate);
                        FadeTo(musicLayer2, 1f, forceImmediate);
                        FadeTo(musicLayer3, 0.0001f, forceImmediate);
                        break;
                    case 2:
                        FadeTo(musicLayer1, 1f, forceImmediate);
                        FadeTo(musicLayer2, 1f, forceImmediate);
                        FadeTo(musicLayer3, 1f, forceImmediate);
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
        // Unity may treat 0 as inactive — minimum is 0.01
        targetVolume = Mathf.Max(targetVolume, 0.0001f);

        if (immediate)
        {
            source.volume = targetVolume;
        }
        else
        {
            StopAllCoroutines(); // Stop any previous fades on this script
            StartCoroutine(FadeVolume(source, targetVolume));
        }
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
