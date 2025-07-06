using UnityEngine;

public class DevLevelSelect : MonoBehaviour
{
    [SerializeField] private SceneLoader sl;
    [SerializeField] private GameObject levelSelectPanel;

    [SerializeField] private string levelSelect;
    [SerializeField] private bool _triggered;
    [SerializeField] private bool _levelSelected;
    [SerializeField] private bool _loaded;

    void Update()
    {
        if (_triggered) //after triggering, want to wait a frame in order to set LevelSelect scene for loading
        {
            if (!_loaded)
            {
                _loaded = true;

                sl.LoadScenes();
            }
        }

        if (!_triggered)
        {
            if(sl != null)
            {
                //setting scenesToLoad array as single levelSelect string
                string[] levelSelectScene = new string[1];
                levelSelectScene[0] = levelSelect;
                sl.SetScenesToLoad(levelSelectScene);

                _triggered = true;
            }
        }
    }

    public void SelectSceneAndLoad(int levelSuffix)
    {
        levelSelect = "Level_" + levelSuffix;

        if (!_levelSelected)
        {
            _levelSelected = true;

            if (sl != null)
            {
                //setting scenesToLoad array as single level string
                string[] levelSelectScene = new string[1];
                levelSelectScene[0] = levelSelect;
                sl.SetScenesToLoad(levelSelectScene);

                DeActivateLevelSelectPanel();

                sl.LoadScenes();

                //setting scenesToUnload array as single levelSelect string
                string[] levelUnloadScene = new string[1];
                levelUnloadScene[0] = "LevelSelect";
                sl.SetScenesToUnLoad(levelUnloadScene);

                sl.UnloadScenes();
            }
        }
    }

    public void LevelLoaded()
    {
        _levelSelected = false;
    }

    public void DeActivateLevelSelectPanel()
    {
        if (levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(false);
        }
    }

    public void ActivateLevelSelectPanel()
    {
        if (levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(true);
        }
    }
}
