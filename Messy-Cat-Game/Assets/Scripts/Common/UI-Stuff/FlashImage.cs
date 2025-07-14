using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FlashImage : MonoBehaviour
{
    public float startAlpha;
    public float endAlpha;
    public float holdMaxAmount;

    Image _image = null;
    Coroutine _currentFlashRoutine = null;

    private void Awake()
    {
        _image = GetComponent<Image>();
        startAlpha = Mathf.Clamp(startAlpha, 0, 1);
        endAlpha = Mathf.Clamp(endAlpha, 0, 1);
    }

    public void StartFlash(float secondsForOneFlash, float maxAlpha, Color newColor)
    {
        _image.color = newColor;

        maxAlpha = Mathf.Clamp(maxAlpha, 0, 1);

        if (_currentFlashRoutine != null)
        {
            StopCoroutine(_currentFlashRoutine);
        }

        _currentFlashRoutine = StartCoroutine(Flash(secondsForOneFlash, maxAlpha));

    }

    IEnumerator Flash(float secondsForOneFlash, float maxAlpha)
    {

        float flashInDuration = secondsForOneFlash / 2;

        for (float t = 0; t <= flashInDuration; t += Time.deltaTime)
        {
            Color colorThisFrame = _image.color;
            colorThisFrame.a = Mathf.Lerp(startAlpha, maxAlpha, t / flashInDuration);
            _image.color = colorThisFrame;

            yield return null;
        }

        yield return new WaitForSeconds(holdMaxAmount);

        float flashOutDuration = secondsForOneFlash / 2;

        for (float t = 0; t <= flashInDuration; t += Time.deltaTime)
        {
            Color colorThisFrame = _image.color;
            colorThisFrame.a = Mathf.Lerp(maxAlpha, endAlpha, t / flashInDuration);
            _image.color = colorThisFrame;

            yield return null;
        }

        _image.color = new Color32(0, 0, 0, 0);

    }

}
