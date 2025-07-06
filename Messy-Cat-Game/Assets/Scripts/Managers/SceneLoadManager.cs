using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SceneLoader _sceneLoader;

    [SerializeField] private GameObject _loadingBarPanel;
    [SerializeField] private Image _loadingBarFill;
    [SerializeField] private GameObject[] _thingsToHide;
    [SerializeField] private GameObject[] _thingsToReveal;

    [SerializeField] private DevLevelSelect devSelect;

    void Awake()
    {
        LoadedScene();
    }

    public void HideThings(GameObject[] things)
    {
        _thingsToHide = things;

        if (things.Length > 0)
        {
            for (int i = 0; i < _thingsToHide.Length; i++)
            {
                _thingsToHide[i].SetActive(false);
            }
        }
    }

    public void RevealThings(GameObject[] things)
    {
        _thingsToReveal = things;

        if (things.Length > 0)
        {
            for (int i = 0; i < _thingsToReveal.Length; i++)
            {
                _thingsToReveal[i].SetActive(true);
            }
        }
    }

    public void LoadScene()
    {
        if (_sceneLoader != null)
        {
            _sceneLoader.LoadScenes();
        }

        ActivateLoadPanel();
    }

    public void UnLoadScene()
    {
        if (_sceneLoader != null)
        {
            _sceneLoader.UnloadScenes();
        }
    }

    public void LoadedScene()
    {
        DeActivateLoadPanel();

        //for dev scene select

        if (devSelect != null)
        {
            devSelect.LevelLoaded();
        }
    }

    public void ActivateLoadPanel()
    {
        if (_loadingBarPanel != null)
        {
            _loadingBarPanel.SetActive(true);
        }
    }

    public void DeActivateLoadPanel()
    {
        if (_loadingBarPanel != null)
        {
            _loadingBarPanel.SetActive(false);
        }
    }
}
