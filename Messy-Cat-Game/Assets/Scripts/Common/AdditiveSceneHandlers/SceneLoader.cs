using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneLoadManager slm;
    [SerializeField] private int currentLevel;

    [SerializeField] private string[] _scenesToLoad;
    [SerializeField] private string[] _scenesToUnload;

    [SerializeField] private bool _triggered;

    private AsyncOperation asyncUnload;

    //for debugging
    public string[] _scenesToLoad_Check;
    public string[] _scenesToUnload_Check;

    void Update()
    {
        if (asyncUnload != null && asyncUnload.isDone)
        {
            //Debug.Log("Level " + currentLevel + " has finished unloading!");

            LoadSameLevel();

            asyncUnload = null; // Prevent repeated checks
        }

        //for debugging, comment in or out if you need to check load or unload scenes from this script
        _scenesToLoad_Check = _scenesToLoad;
        _scenesToUnload_Check = _scenesToUnload;
    }

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

            //Debug.Log("SL Load scenes");
        }
    }

    public void UnloadScenes()
    {
        for (int i = 0; i < _scenesToUnload.Length; i++)
        {
#pragma warning disable CS0219 // Variable is assigned but its value is never used
            bool isSceneLoaded = false;
#pragma warning restore CS0219 // Variable is assigned but its value is never used

            for (int j = 0; j < SceneManager.sceneCount; j++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(j);

                if (loadedScene.name == _scenesToUnload[i])
                {
                    SceneManager.UnloadSceneAsync(_scenesToUnload[i]);
                }
            }
        }

        //Debug.Log("SL Unload scenes");
    }

    public void UnloadSameLevel(int level)
    {
        currentLevel = level;

        asyncUnload = SceneManager.UnloadSceneAsync("Level_" + level);
    }

    private void LoadSameLevel()
    {
        if (slm != null)
        {
            slm.LoadScene();
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
