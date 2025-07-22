using UnityEngine;

public class LevelMusicController : MonoBehaviour
{
    [Header("Slow Music Layers (plays if level has 1–3 messes to make)")]
    public AudioSource slowLayer1;  // always present
    public AudioSource slowLayer2;  // fades in when second mess is made (only on a level containing 3 messes)
    public AudioSource slowLayer3;  // unused in slow mode currently. I'll put silence here as a dummy file, but it's available if we want it later

    [Header("Fast Music Layers (plays if level has 4–8 messes to make)")]
    public AudioSource fastLayer1;  // always present
    public AudioSource fastLayer2;  // fades in at 33%
    public AudioSource fastLayer3;  // fades in at 66%

    [Header("Music Settings")]
    public float fadeDuration = 0.2f;   // adjust fade length as layers come in

    [Header("References")]
    public LevelManager levelManager;

    private float messProgress;         // this is just for fast music purposes. Gives us a percentage, regardless of how many messes in a level
    private int lastIntensity = -1;     // forces volume update immediately upon return, so current settings are re-checked
    private bool isUsingSlowMusic;      // tracks which set is active

    // Active layer references (assigned at runtime)
    private AudioSource musicLayer1;
    private AudioSource musicLayer2;
    private AudioSource musicLayer3;

    private void Update()
    {
        if (levelManager == null || levelManager.totalMessNeeded == 0)
            return;

        messProgress = (float)levelManager.currentMesses / levelManager.totalMessNeeded;
        UpdateMusicLayers();
    }

    public void StartLevelMusic()
    {
        if (levelManager == null) return;

        // Choose slow or fast set based on total messes (slow = 1–3, fast = 4–8)
        isUsingSlowMusic = levelManager.totalMessNeeded <= 3;

        musicLayer1 = isUsingSlowMusic ? slowLayer1 : fastLayer1;
        musicLayer2 = isUsingSlowMusic ? slowLayer2 : fastLayer2;
        musicLayer3 = isUsingSlowMusic ? slowLayer3 : fastLayer3;

        // Start all layers together (muted)
        if (!musicLayer1.isPlaying) musicLayer1.Play();
        if (!musicLayer2.isPlaying) musicLayer2.Play();
        if (!musicLayer3.isPlaying) musicLayer3.Play();

        // Recalculate from current progress
        UpdateMusicLayers(true);
    }

    public void MuteLevelMusic()    // I figured a mute instead of pause could be good when playing the dog or human music?
    {
        FadeTo(musicLayer1, 0.01f, false);
        FadeTo(musicLayer2, 0.01f, false);
        FadeTo(musicLayer3, 0.01f, false);
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
        if (isUsingSlowMusic)
        {
            // Only use Layer 1 and 2 in slow mode
            if (levelManager.totalMessNeeded <= 2)
            {
                FadeTo(musicLayer1, 1f, forceImmediate);
                FadeTo(musicLayer2, 0.01f, forceImmediate);
                FadeTo(musicLayer3, 0.01f, forceImmediate);
            }
            else if (levelManager.totalMessNeeded == 3)
            {
                FadeTo(musicLayer1, 1f, forceImmediate);
                bool secondMessReached = levelManager.currentMesses >= 2;
                FadeTo(musicLayer2, secondMessReached ? 1f : 0.01f, forceImmediate);
                FadeTo(musicLayer3, 0.01f, forceImmediate);
            }
        }
        else
        {
            int currentIntensity = GetIntensityFromProgress(messProgress);

            if (currentIntensity != lastIntensity || forceImmediate)
            {
                switch (currentIntensity)
                {
                    case 0:
                        FadeTo(musicLayer1, 1f, forceImmediate);
                        FadeTo(musicLayer2, 0.01f, forceImmediate);
                        FadeTo(musicLayer3, 0.01f, forceImmediate);
                        break;
                    case 1:
                        FadeTo(musicLayer1, 1f, forceImmediate);
                        FadeTo(musicLayer2, 1f, forceImmediate);
                        FadeTo(musicLayer3, 0.01f, forceImmediate);
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
        targetVolume = Mathf.Max(targetVolume, 0.01f);

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

    private System.Collections.IEnumerator FadeVolume(AudioSource source, float targetVolume)
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
