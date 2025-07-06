using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneLoadManager slm;

    [SerializeField] private string[] _scenesToLoad;
    [SerializeField] private string[] _scenesToUnload;

    [SerializeField] private bool _triggered;

    public void LoadScenes()
    {
        if (!_triggered)
        {
            _triggered = true;

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

            _triggered = false;
        }
    }

    public void UnloadScenes()
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

    public void SetScenesToLoad(string[] scenes)
    {
        _scenesToLoad = scenes;
    }

    public void SetScenesToUnLoad(string[] scenes)
    {
        _scenesToUnload = scenes;
    }
}
