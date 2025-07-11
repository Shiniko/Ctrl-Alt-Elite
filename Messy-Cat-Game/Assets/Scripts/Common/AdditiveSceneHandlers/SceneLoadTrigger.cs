using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private SceneLoadManager slm;
    [SerializeField] private GameObject triggerPair;

    [SerializeField] private bool _triggered;
    [SerializeField] private bool hideThings;
    [SerializeField] private bool revealThings;

    [SerializeField] private string[] _scenesToLoad;
    [SerializeField] private string[] _scenesToUnload;

    [SerializeField] private GameObject[] _thingsToHide;
    [SerializeField] private GameObject[] _thingsToReveal;

    void Update()
    {
        if (_player == null)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                _player = GameObject.FindGameObjectWithTag("Player");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == _player)
        {
            if (!_triggered)
            {
                _triggered = true;

                LoadScenes();
                UnloadScenes();

                if (hideThings)
                {
                    if(slm != null)
                    {
                        slm.HideThings(_thingsToHide);
                    }
                }

                if (revealThings)
                {
                    if (slm != null)
                    {
                        slm.RevealThings(_thingsToReveal);
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == _player)
        {
            if (triggerPair != null)
            {
                triggerPair.SetActive(true);

                triggerPair.GetComponent<SceneLoadTrigger>()._triggered = false;
            }
        }
    }

    private void LoadScenes()
    {
        for (int i = 0; i < _scenesToLoad.Length; i++)
        {
            bool isSceneLoaded = false;

            for (int j = 0; j < SceneManager.sceneCount; j++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(j);

                if (loadedScene.name == _scenesToLoad[i])
                {
                    isSceneLoaded = true;
                    break;
                }
            }

            if (!isSceneLoaded)
            {
                SceneManager.LoadSceneAsync(_scenesToLoad[i], LoadSceneMode.Additive);
            }
        }
    }

    private void UnloadScenes()
    {
        for (int i = 0; i < _scenesToUnload.Length; i++)
        {
            bool isSceneLoaded = false;

            for (int j = 0; j < SceneManager.sceneCount; j++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(j);

                if (loadedScene.name == _scenesToUnload[i])
                {
                    SceneManager.UnloadSceneAsync(_scenesToUnload[i]);
                }
            }
        }
    }

    private void HideThings()
    {
        for (int i = 0; i < _thingsToHide.Length; i++)
        {
            _thingsToHide[i].SetActive(false);
        }
    }

    private void RevealThings()
    {
        for (int i = 0; i < _thingsToHide.Length; i++)
        {
            _thingsToHide[i].SetActive(true);
        }
    }
}
