using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private string[] _allLevelScenes;

    [SerializeField] private GameObject _loadingBarPanel;
    [SerializeField] private Image _loadingBarFill;
    [SerializeField] private GameObject[] _thingsToHide;
    [SerializeField] private GameObject[] _thingsToReveal;

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
        if (_loadingBarPanel != null)
        {
            _loadingBarPanel.SetActive(true);
        }

        if (_sceneLoader != null)
        {
            _sceneLoader.LoadScenes();
        }
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
        if (_loadingBarPanel != null)
        {
            _loadingBarPanel.SetActive(false);
        }
    }

    public void SetLevelSelect(int level)
    {
        if (level < 1 || level > _allLevelScenes.Length)
        {
            return;
        }

        if(_sceneLoader != null)
        {
            string[] levelNames = new string[1];
            levelNames[0] = _allLevelScenes[level - 1];
            _sceneLoader.SetScenesToLoad(levelNames);
        }
    }
}
